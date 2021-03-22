namespace nuap.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Views;

    public class OrderDetailViewModel: BaseViewModel
    {
        private ApiService apiService;
        private string status;
        private string statusColor;
        private string initText;
        private string distributorName;
        private string deliveryDate;
        private string address;
        private bool isEnabled;
        private int orderId;
        private ObservableCollection<Product> products;
        private bool isRefreshing;

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public ObservableCollection<Product> Products
        {
            get { return this.products; }
            set { SetValue(ref this.products, value); }
        }

        public string Status
        {
            get { return this.status; }
            set { SetValue(ref this.status, value); }
        }

        public string DistributorName
        {
            get { return this.distributorName; }
            set { SetValue(ref this.distributorName, value); }
        }

        public string DeliveryDate
        {
            get { return this.deliveryDate; }
            set { SetValue(ref this.deliveryDate, value); }
        }

        public string Address
        {
            get { return this.address; }
            set { SetValue(ref this.address, value); }
        }

        public int OrderId
        {
            get { return this.orderId; }
            set { SetValue(ref this.orderId, value); }
        }

        public string StatusColor
        {
            get { return this.statusColor; }
            set { SetValue(ref this.statusColor, value); }
        }

        public string InitText
        {
            get { return this.initText; }
            set { SetValue(ref this.initText, value); }
        }

        public Order Order
        {
            get;
            set;
        }

        public OrderDetailViewModel(Order order)
        {
            this.Order = order;
            this.apiService = new ApiService();
            this.LoadDetailOrder();
            this.LoadProducts();
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadProducts);
            }
        }

        public ICommand ViewSellerCommand
        {
            get
            {
                return new RelayCommand(ViewSeller);
            }
        }

        public ICommand RatingCommand
        {
            get
            {
                return new RelayCommand(Rating);
            }
        }

        private async void ViewSeller()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.SellerID = this.Order.UserId;
            mainViewModel.SellerDetail = new SellerDetailViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new SellerDetailPage());
        }

        private async void Rating()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.OrderID = this.Order.Id;
            mainViewModel.Rating = new RatingViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new RatingPage());
        }

        private void LoadDetailOrder()
        {
            var mainViewModel = MainViewModel.GetInstance();
            if (mainViewModel.User.role == "Comercio")
            {
                this.InitText = "Genial, ya casi abasteces tu negocio";
            } else
            {
                this.InitText = "Genial, ya casi obtienes tus productos";
            }

            this.Address = this.Order.Address;
            this.DistributorName = this.Order.DistributorName;
            this.DeliveryDate = this.Order.DeliveryDate;
            this.Status = this.Order.Status;
            this.StatusColor = this.Order.StatusColor;
            this.OrderId = this.Order.Id;
    

            if (this.Order.Status == "Entregado" && this.Order.Rating == 0)
            {
                this.IsEnabled = true;
            } else
            {
                this.IsEnabled = false;
            }
        }

        private async void LoadProducts()
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

            var response = await this.apiService.GetProductsByOrder(
                mainViewModel.User.api_token,
                "https://thenuap.com",
                this.Order.Id);

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

            var list = (List<Product>)response.Data;
            List<Product> newList = new List<Product>();

            foreach (var item in list)
            {
                item.Image = "https://thenuap.com/"+item.Image;
                newList.Add(item);
            }

            this.Products = new ObservableCollection<Product>(newList);
            this.IsRefreshing = false;
        }
    }
}
