namespace nuap.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Services;
    using System.Windows.Input;
    using System.Linq;
    using Xamarin.Forms;
    using Views;
    using nuap.Helpers;
    using System.Threading.Tasks;

    public class VerifyOtpViewModel: BaseViewModel
    {
        private ApiService apiService;

        private string phone;
        private string code;
        private bool isEnabled;
        private bool isEnabledRetry;

        public string Phone
        {
            get { return this.phone; }
            set { SetValue(ref this.phone, value); }
        }

        public string Code
        {
            get { return this.code; }
            set { SetValue(ref this.code, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }
        
        public bool IsEnabledRetry
        {
            get { return this.isEnabledRetry; }
            set { SetValue(ref this.isEnabledRetry, value); }
        }

        public ICommand GenerateOtpCommand
        {
            get
            {
                return new RelayCommand(GenerateOtp);
            }
        }

        public ICommand VerifyOtpCommand
        {
            get
            {
                return new RelayCommand(VerifyOtp);
            }
        }

        public VerifyOtpViewModel(string phone)
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
            this.IsEnabledRetry = true;
            this.Phone = phone;

            //EnabledRetry();
        }

        public async Task EnabledRetry()
        {
            await Task.Delay(1);
            this.IsEnabled = true;
        }

        private async void GenerateOtp()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.GenerateOTP = new GenerateOtpViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new GenerateOtpPage());
        }

        private async void VerifyOtp()
        {
            if (string.IsNullOrEmpty(this.Code))
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "Debes ingresar el código que te hemos enviado",
                        "Aceptar"
                    );
                return;
            }

            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var response = await this.apiService.VerifyOTP(
                "https://thenuap.com",
                this.Phone,
                this.Code);

            if (response == null)
            {
                this.IsEnabled = true;
                this.Code = string.Empty;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Ha ocurrido un error",
                    "Aceptar");
                return;
            }

            if (response.Data == null)
            {
                this.Code = string.Empty;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Errors.First().Detail,
                    "Aceptar");
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.User = response.Data;

            Settings.Token = response.Data.api_token;
            Settings.UserType = response.Data.role;
            Settings.Phone = response.Data.phone;
            Settings.Email = response.Data.email;
            Settings.Id = response.Data.id.ToString();

            if (response.Data.role == "Usuario")
            {
                mainViewModel.Home = new HomeViewModel();
                await Application.Current.MainPage.Navigation.PushAsync(new HomeTabbedPage());
            }
            else
            {
                mainViewModel.HomeCommerce = new HomeCommerceViewModel();
                await Application.Current.MainPage.Navigation.PushAsync(new HomeCommerceTabbedPage());
            }
        }
    }
}
