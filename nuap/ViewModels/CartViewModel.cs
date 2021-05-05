namespace nuap.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Views;
    using Xamarin.Forms;
    using Services;
    using System.Linq;
    using System.Collections.Generic;

    public class CartViewModel: BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<Cart> cart;
        private bool isRefreshing;
        private double total;
        private int delivery;

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        public double Total
        {
            get { return this.total; }
            set { SetValue(ref this.total, value); }
        }

        public int Delivery
        {
            get { return this.delivery; }
            set { SetValue(ref this.delivery, value); }
        }

        public ObservableCollection<Cart> Cart
        {
            get { return this.cart; }
            set { SetValue(ref this.cart, value); }
        }

        public ICommand DiminishCommand
        {
            get
            {
                //return new RelayCommand(Diminish);
                return new Command<string>((x) => Diminish(x));
            }
        }

        public ICommand CreateOrderCommand
        {
            get
            {
                return new RelayCommand(CreateOrder);
            }
        }


        public ICommand IncreaseCommand
        {
            get
            {
                return new RelayCommand(Increase);
            }
        }

        public CartViewModel()
        {
            this.apiService = new ApiService();
            this.LoadCart();
        }

        private async void Diminish(string Id)
        {
            await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Si sirvio" + Id,
                    "Aceptar");
            return;

            /*if ((this.Quantity - 1) < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "No puedes agregar menos de 0 productos",
                    "Aceptar");
                return;
            }
            else
            {
                this.Quantity = this.Quantity - 1;
                this.Total = this.Price * this.Quantity;
            }*/
        }

        private async void Increase()
        {
            /*if ((this.Quantity + 1) > this.Product.Stock)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "No puedes agregar más de " + this.Product.Stock + " productos",
                    "Aceptar");
                return;
            }
            else
            {
                this.Quantity = this.Quantity + 1;
                this.Total = this.Price * this.Quantity;
            }*/
        }

        private void LoadCart()
        {
            var mainViewModel = MainViewModel.GetInstance();
            this.Total = 0;

            foreach (var item in mainViewModel.CartList)
            {
                item.Price = item.Quantity * item.Price;
                this.Total += item.Price;
            }

            this.Cart = new ObservableCollection<Cart>(mainViewModel.CartList);
            this.IsRefreshing = false;
            this.CalculateDelivery();

            this.Total = this.Total + this.Delivery;
        }

        private async void CalculateDelivery()
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();
            var data = new CartDataService();
            data.Products = mainViewModel.CartList;


            var response = await this.apiService.CalculateDelivery(
                 "https://thenuap.com",
                 mainViewModel.User.api_token,
                 data);

            if (response == null)
            {
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

            this.Delivery = response.Data.Discount;
        }

        private async void CreateOrder()
        {
            
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Aceptar");
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();
            var data = new CartDataService();
            data.Products = mainViewModel.CartList;

            var response = await this.apiService.CreateOrder(
                 "https://thenuap.com",
                 mainViewModel.User.api_token,
                 19,
                 data);

            if (response == null)
            {
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

            List<int> orders = response.Data.Orders;
            string url = "https://thenuap.com/payment-gateway?";
            string user = "user=" + mainViewModel.User.id;
            int count = 0;
            foreach (var item in orders)
            {
                url += "orders["+count+"]="+item+"&";
                count++;
            }

            url += user;

            mainViewModel.PaymentGateway = new PaymentGatewayViewModel(url);
            await Application.Current.MainPage.Navigation.PushAsync(new PaymentGatewayPage());
        }
    }
}
