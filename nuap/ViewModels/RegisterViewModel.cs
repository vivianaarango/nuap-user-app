namespace nuap.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using System.Linq;
    using Xamarin.Forms;
    using Views;
    using Services;
    using Helpers;

    public class RegisterViewModel: BaseViewModel
    {
        private ApiService apiService;

        private string email;
        private string password;
        private string confirm_password;
        private string phone;
        private string name;
        private string last_name;
        private string identity_number;
        private bool isRunning;
        private bool isEnabled;

        public string Email
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }

        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }

        public string ConfirmPassword
        {
            get { return this.confirm_password; }
            set { SetValue(ref this.confirm_password, value); }
        }

        public string Phone
        {
            get { return this.phone; }
            set { SetValue(ref this.phone, value); }
        }

        public string Name
        {
            get { return this.name; }
            set { SetValue(ref this.name, value); }
        }

        public string LastName
        {
            get { return this.last_name; }
            set { SetValue(ref this.last_name, value); }
        }

        public string IdentityNumber
        {
            get { return this.identity_number; }
            set { SetValue(ref this.identity_number, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public ICommand RegisterCommand
        {
            get
            {
                return new RelayCommand(Register);
            }
        }


        public RegisterViewModel()
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
        }

        private async void Register()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "Debes ingrsar tu nombre",
                        "Aceptar"
                    );
                return;
            }

            if (string.IsNullOrEmpty(this.LastName))
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "Debes ingrsar tu apellido",
                        "Aceptar"
                    );
                return;
            }

            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "Debes ingresar tu correo electrónico",
                        "Aceptar"
                    );
                return;
            }

            if (string.IsNullOrEmpty(this.Phone))
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "Debes ingresar tu celular",
                        "Aceptar"
                    );
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "Debes ingresar tu contraseña",
                        "Aceptar"
                    );
                return;
            }

            if (string.IsNullOrEmpty(this.ConfirmPassword))
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "Debes confirmar tu contraseña",
                        "Aceptar"
                    );
                return;
            }

            if (string.IsNullOrEmpty(this.IdentityNumber))
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "Debes ingrsar tu número de identificación",
                        "Aceptar"
                    );
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                this.Password = string.Empty;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var response = await this.apiService.RegisterUser(
                "https://thenuap.com",
                this.Email,
                this.Password,
                this.Phone,
                this.Name,
                this.LastName,
                this.IdentityNumber);

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
                 await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Errors.First().Detail,
                    "Aceptar");
                return;
            }

            await Application.Current.MainPage.DisplayAlert(
                    "",
                    "Usuario registrado exitosamente, ingresa tu usuario y contraseña",
                    "Aceptar");

            this.IsEnabled = true;
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Login = new LoginViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }
    }
}
