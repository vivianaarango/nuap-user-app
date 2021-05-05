namespace nuap
{
    using Xamarin.Forms;
    using Views;
    using Helpers;
    using ViewModels;
    using Newtonsoft.Json;
    using Models;
    using System.Collections.Generic;
    using System;

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (string.IsNullOrEmpty(Settings.Token))
            {
                MainPage = new NavigationPage(new LoginPage());
            } else
            {
                var mainViewModel = MainViewModel.GetInstance();

                User Usuario = new User();
                Usuario.api_token = Settings.Token;
                Usuario.role = Settings.UserType;
                Usuario.phone = Settings.Phone;
                Usuario.email = Settings.Email;
                Usuario.id = Int32.Parse(Settings.Id);

                mainViewModel.User = Usuario;

                if (! string.IsNullOrEmpty(Settings.CartList))
                {
                    mainViewModel.CartList = JsonConvert.DeserializeObject<List<Cart>>(Settings.CartList);
                }

                mainViewModel.Home = new HomeViewModel();
                MainPage = new NavigationPage(new HomeTabbedPage());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
