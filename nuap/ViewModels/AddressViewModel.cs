namespace nuap.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Services;
    using System.Collections.Generic;
    using System.Linq;
    using Xamarin.Forms;
    using Models;
    using Views;

    public class AddressViewModel: BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<Address> address;
        private List<Address> list;
        private bool isRefreshing;

        public ObservableCollection<Address> Address
        {
            get { return this.address; }
            set { SetValue(ref this.address, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadAddress);
            }
        }

        public ICommand CreateAddressCommand
        {
            get
            {
                return new RelayCommand(CreateAddress);
            }
        }

        public AddressViewModel()
        {
            this.apiService = new ApiService();
            this.LoadAddress();
        }

        private async void LoadAddress()
        {
            this.IsRefreshing = true;
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();

            var response = await this.apiService.GetAddress(
                mainViewModel.User.api_token,
                "https://thenuap.com");

            if (response == null)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Ha ocurrido un error",
                    "Aceptar");
                return;
            }

            if (response.Data == null)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Errors.First().Detail,
                    "Aceptar");
                return;
            }

            this.list = response.Data;

            this.Address = new ObservableCollection<Address>((this.list));
            this.IsRefreshing = false;
        }

        private async void CreateAddress()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.CreateAddress = new CreateAddressViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new CreateAddressPage());
        }
    }
}
