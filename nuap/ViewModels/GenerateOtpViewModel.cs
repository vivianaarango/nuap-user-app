namespace nuap.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Services;
    using System.Windows.Input;
    using System.Linq;
    using Xamarin.Forms;
    using Views;
    using System.Threading.Tasks;

    public class GenerateOtpViewModel: BaseViewModel
    {
        private ApiService apiService;

        private string phone;
        private bool isEnabled;

        public string Phone
        {
            get { return this.phone; }
            set { SetValue(ref this.phone, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public ICommand GenerateOtpCommand
        {
            get
            {
                return new RelayCommand(GenerateOtp);
            }
        }

        public GenerateOtpViewModel()
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
        }

        private async void GenerateOtp()
        {
            if (string.IsNullOrEmpty(this.Phone))
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "Debes ingresar tu celular",
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

            var response = await this.apiService.GenerateOTP(
                "https://thenuap.com",
                this.Phone);

            if (response == null)
            {
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Ha ocurrido un error",
                    "Aceptar");
                return;
            }

            if (response.Errors != null)
            {
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Errors.First().Detail,
                    "Aceptar");
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.VerifyOTP = new VerifyOtpViewModel(this.Phone);
            await Application.Current.MainPage.Navigation.PushAsync(new VerifyOtpPage());
        }
    }
}
