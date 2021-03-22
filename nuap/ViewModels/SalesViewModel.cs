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

    public class SalesViewModel: BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection<OrderItemViewModel> ordersDelivered;
        private ObservableCollection<OrderItemViewModel> ordersInitiated;
        private ObservableCollection<OrderItemViewModel> ordersEnlistment;
        private ObservableCollection<OrderItemViewModel> ordersAcepted;
        private ObservableCollection<OrderItemViewModel> ordersCanceled;
        private ObservableCollection<OrderItemViewModel> ordersCirculation;
        private int ordersDeliveredCount;
        private int ordersInitiatedCount;
        private int ordersEnlistmentCount;
        private int ordersAceptedCount;
        private int ordersCanceledCount;
        private int ordersCirculationCount;
        private string ordersDeliveredText;
        private string ordersInitiatedText;
        private string ordersEnlistmentText;
        private string ordersAceptedText;
        private string ordersCanceledText;
        private string ordersCirculationText;
        private string ordersDeliveredList;
        private string ordersInitiatedList;
        private string ordersEnlistmentList;
        private string ordersAceptedList;
        private string ordersCanceledList;
        private string ordersCirculationList;
        List<Order> listDelivered = new List<Order>();
        List<Order> listInitiated = new List<Order>();
        List<Order> listCanceled = new List<Order>();
        List<Order> listAcepted = new List<Order>();
        List<Order> listEnlistment = new List<Order>();
        List<Order> listCirculation = new List<Order>();
        private bool isRefreshing;

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        //listas activas
        public string OrdersDeliveredList
        {
            get { return this.ordersDeliveredList; }
            set { SetValue(ref this.ordersDeliveredList, value); }
        }

        public string OrdersInitiatedList
        {
            get { return this.ordersInitiatedList; }
            set { SetValue(ref this.ordersInitiatedList, value); }
        }

        public string OrdersEnlistmentList
        {
            get { return this.ordersEnlistmentList; }
            set { SetValue(ref this.ordersEnlistmentList, value); }
        }

        public string OrdersAceptedList
        {
            get { return this.ordersAceptedList; }
            set { SetValue(ref this.ordersAceptedList, value); }
        }

        public string OrdersCanceledList
        {
            get { return this.ordersCanceledList; }
            set { SetValue(ref this.ordersCanceledList, value); }
        }

        public string OrdersCirculationList
        {
            get { return this.ordersCirculationList; }
            set { SetValue(ref this.ordersCirculationList, value); }
        }

        //validación 
        public string OrdersDeliveredText
        {
            get { return this.ordersDeliveredText; }
            set { SetValue(ref this.ordersDeliveredText, value); }
        }

        public string OrdersInitiatedText
        {
            get { return this.ordersInitiatedText; }
            set { SetValue(ref this.ordersInitiatedText, value); }
        }

        public string OrdersEnlistmentText
        {
            get { return this.ordersEnlistmentText; }
            set { SetValue(ref this.ordersEnlistmentText, value); }
        }

        public string OrdersAceptedText
        {
            get { return this.ordersAceptedText; }
            set { SetValue(ref this.ordersAceptedText, value); }
        }

        public string OrdersCanceledText
        {
            get { return this.ordersCanceledText; }
            set { SetValue(ref this.ordersCanceledText, value); }
        }

        public string OrdersCirculationText
        {
            get { return this.ordersCirculationText; }
            set { SetValue(ref this.ordersCirculationText, value); }
        }

        public int OrdersDeliveredCount
        {
            get { return this.ordersDeliveredCount; }
            set { SetValue(ref this.ordersDeliveredCount, value); }
        }

        public int OrdersInitiatedCount
        {
            get { return this.ordersInitiatedCount; }
            set { SetValue(ref this.ordersInitiatedCount, value); }
        }

        public int OrdersEnlistmentCount
        {
            get { return this.ordersEnlistmentCount; }
            set { SetValue(ref this.ordersEnlistmentCount, value); }
        }

        public int OrdersAceptedCount
        {
            get { return this.ordersAceptedCount; }
            set { SetValue(ref this.ordersAceptedCount, value); }
        }

        public int OrdersCanceledCount
        {
            get { return this.ordersCanceledCount; }
            set { SetValue(ref this.ordersCanceledCount, value); }
        }

        public int OrdersCirculationCount
        {
            get { return this.ordersCirculationCount; }
            set { SetValue(ref this.ordersCirculationCount, value); }
        }

        public ObservableCollection<OrderItemViewModel> OrdersDelivered
        {
            get { return this.ordersDelivered; }
            set { SetValue(ref this.ordersDelivered, value); }
        }

        public ObservableCollection<OrderItemViewModel> OrdersInitiated
        {
            get { return this.ordersInitiated; }
            set { SetValue(ref this.ordersInitiated, value); }
        }

        public ObservableCollection<OrderItemViewModel> OrdersEnlistment
        {
            get { return this.ordersEnlistment; }
            set { SetValue(ref this.ordersEnlistment, value); }
        }

        public ObservableCollection<OrderItemViewModel> OrdersAcepted
        {
            get { return this.ordersAcepted; }
            set { SetValue(ref this.ordersAcepted, value); }
        }

        public ObservableCollection<OrderItemViewModel> OrdersCanceled
        {
            get { return this.ordersCanceled; }
            set { SetValue(ref this.ordersCanceled, value); }
        }

        public ObservableCollection<OrderItemViewModel> OrdersCirculation
        {
            get { return this.ordersCirculation; }
            set { SetValue(ref this.ordersCirculation, value); }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadSales);
            }
        }

        public ICommand GetAllCommand
        {
            get
            {
                return new RelayCommand(LoadSales);
            }
        }

        public ICommand GetInitiatedCommand
        {
            get
            {
                return new RelayCommand(GetInitiated);
            }
        }

        public ICommand GetAceptedCommand
        {
            get
            {
                return new RelayCommand(GetAcepted);
            }
        }

        public ICommand GetEnlistmentCommand
        {
            get
            {
                return new RelayCommand(GetEnlistment);
            }
        }

        public ICommand GetCirculationCommand
        {
            get
            {
                return new RelayCommand(GetCirculation);
            }
        }

        public ICommand GetDeliveredCommand
        {
            get
            {
                return new RelayCommand(GetDelivered);
            }
        }

        public ICommand GetCanceledCommand
        {
            get
            {
                return new RelayCommand(GetCanceled);
            }
        }

        public SalesViewModel()
        {
            this.apiService = new ApiService();
            this.LoadSales();
        }

        private async void LoadSales()
        {
            this.OrdersInitiatedList = "true";
            this.OrdersEnlistmentList = "true";
            this.OrdersDeliveredList = "true";
            this.OrdersAceptedList = "true";
            this.OrdersCanceledList = "true";
            this.OrdersCirculationList = "true";

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

            var response = await this.apiService.GetSales(
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

            var list = (List<Order>)response.Data;
            this.listDelivered = new List<Order>();
            this.listInitiated = new List<Order>();
            this.listCanceled = new List<Order>();
            this.listAcepted = new List<Order>();
            this.listEnlistment = new List<Order>();
            this.listCirculation = new List<Order>();

            foreach (var item in list)
            {
                if (item.Status == "Iniciado")
                {
                    item.StatusColor = "#ffc33a";
                    this.listInitiated.Add(item);
                }

                if (item.Status == "Aceptado")
                {
                    item.StatusColor = "#ffc33a";
                    this.listAcepted.Add(item);
                }

                if (item.Status == "Circulación")
                {
                    item.StatusColor = "#a3cafa";
                    this.listCirculation.Add(item);
                }

                if (item.Status == "Alistamiento")
                {
                    item.StatusColor = "#a3cafa";
                    this.listEnlistment.Add(item);
                }

                if (item.Status == "Entregado")
                {
                    item.StatusColor = "#95dc4e";
                    this.listDelivered.Add(item);
                }

                if (item.Status == "Cancelado")
                {
                    item.StatusColor = "#f78d8d";
                    this.listCanceled.Add(item);
                }
            }

            this.OrdersInitiatedCount = this.listInitiated.Count() * 140;
            this.OrdersDeliveredCount = this.listDelivered.Count() * 140;
            this.OrdersCanceledCount = this.listCanceled.Count() * 140;
            this.OrdersAceptedCount = this.listAcepted.Count() * 140;
            this.OrdersCirculationCount = this.listCirculation.Count() * 140;
            this.OrdersEnlistmentCount = this.listEnlistment.Count() * 140;

            if (this.listEnlistment.Count() == 0)
            {
                this.OrdersEnlistmentText = "true";
            } 
            else
            {
                this.OrdersEnlistmentText = "false";
            }


            if (this.listInitiated.Count() == 0)
            {
                this.OrdersInitiatedText = "true";
            }
            else
            {
                this.OrdersInitiatedText = "false";
            }


            if (this.listDelivered.Count() == 0)
            {
                this.OrdersDeliveredText = "true";
            }
            else
            {
                this.OrdersDeliveredText = "false";
            }

            if (this.listAcepted.Count() == 0)
            {
                this.OrdersAceptedText = "true";
            }
            else
            {
                this.OrdersAceptedText = "false";
            }

            if (this.listCanceled.Count() == 0)
            {
                this.OrdersCanceledText = "true";
            }
            else
            {
                this.OrdersCanceledText = "false";
            }

            if (this.listCirculation.Count() == 0)
            {
                this.OrdersCirculationText = "true";
            }
            else
            {
                this.OrdersCirculationText = "false";
            }

            this.OrdersAcepted = new ObservableCollection<OrderItemViewModel>(ToOrderViewModel(this.listAcepted));
            this.OrdersDelivered = new ObservableCollection<OrderItemViewModel>(ToOrderViewModel(this.listDelivered));
            this.OrdersCanceled = new ObservableCollection<OrderItemViewModel>(ToOrderViewModel(this.listCanceled));
            this.OrdersCirculation = new ObservableCollection<OrderItemViewModel>(ToOrderViewModel(this.listCirculation));
            this.OrdersEnlistment = new ObservableCollection<OrderItemViewModel>(ToOrderViewModel(this.listEnlistment));
            this.OrdersInitiated = new ObservableCollection<OrderItemViewModel>(ToOrderViewModel(this.listInitiated));

            this.IsRefreshing = false;
        }

        private IEnumerable<OrderItemViewModel> ToOrderViewModel(List<Order> list)
        {
            return list.Select(o => new OrderItemViewModel
            {
                Id = o.Id,
                UserId = o.UserId,
                UserType = o.UserType,
                CancelReason = o.CancelReason,
                ClientId = o.ClientId,
                ClientType = o.ClientType,
                TotalProducts = o.TotalProducts,
                TotalAmount = o.TotalAmount,
                DeliveryAmount = o.DeliveryAmount,
                TotalDiscount = o.TotalDiscount,
                Total = o.Total,
                DeliveryDate = o.DeliveryDate,
                Status = o.Status,
                Rating = o.Rating,
                RatingImage = o.RatingImage,
                StatusColor = o.StatusColor,
                AddressId = o.AddressId,
                Address = o.Address,
                DistributorName = o.DistributorName
            });
        }

        private void GetInitiated()
        {
            this.GetByStatus("iniciado");
        }

        private void GetAcepted()
        {
            this.GetByStatus("aceptado");
        }

        private void GetDelivered()
        {
            this.GetByStatus("entregado");
        }

        private void GetCanceled()
        {
            this.GetByStatus("cancelado");
        }

        private void GetEnlistment()
        {
            this.GetByStatus("alistamiento");
        }

        private void GetCirculation()
        {
            this.GetByStatus("circulación");
        }


        private void GetByStatus(string status)
        {
            if (status == "iniciado")
            {
                this.OrdersInitiatedText = "false";
                this.OrdersEnlistmentText = "true";
                this.OrdersDeliveredText = "true";
                this.OrdersAceptedText = "true";
                this.OrdersCanceledText = "true";
                this.OrdersCirculationText = "true";

                this.OrdersInitiatedList = "true";
                this.OrdersEnlistmentList = "false";
                this.OrdersDeliveredList = "false";
                this.OrdersAceptedList = "false";
                this.OrdersCanceledList = "false";
                this.OrdersCirculationList = "false";


                if (this.listInitiated.Count() == 0)
                {
                    this.OrdersInitiatedText = "true";
                    this.OrdersInitiatedList = "false";
                }
            }

            if (status == "aceptado")
            {
                this.OrdersInitiatedText = "true";
                this.OrdersEnlistmentText = "true";
                this.OrdersDeliveredText = "true";
                this.OrdersAceptedText = "false";
                this.OrdersCanceledText = "true";
                this.OrdersCirculationText = "true";

                this.OrdersInitiatedList = "false";
                this.OrdersEnlistmentList = "false";
                this.OrdersDeliveredList = "false";
                this.OrdersAceptedList = "true";
                this.OrdersCanceledList = "false";
                this.OrdersCirculationList = "false";

                if (this.listAcepted.Count() == 0)
                {
                    this.OrdersAceptedText = "true";
                    this.OrdersAceptedList = "false";
                }
            }

            if (status == "cancelado")
            {
                this.OrdersInitiatedText = "true";
                this.OrdersEnlistmentText = "true";
                this.OrdersDeliveredText = "true";
                this.OrdersAceptedText = "true";
                this.OrdersCanceledText = "false";
                this.OrdersCirculationText = "true";

                this.OrdersInitiatedList = "false";
                this.OrdersEnlistmentList = "false";
                this.OrdersDeliveredList = "false";
                this.OrdersAceptedList = "false";
                this.OrdersCanceledList = "true";
                this.OrdersCirculationList = "false";


                if (this.listCanceled.Count() == 0)
                {
                    this.OrdersCanceledText = "true";
                    this.OrdersCanceledList = "false";
                }
            }

            if (status == "circulación")
            {
                this.OrdersInitiatedText = "true";
                this.OrdersEnlistmentText = "true";
                this.OrdersDeliveredText = "true";
                this.OrdersAceptedText = "true";
                this.OrdersCanceledText = "true";
                this.OrdersCirculationText = "false";

                
                this.OrdersInitiatedList = "false";
                this.OrdersEnlistmentList = "false";
                this.OrdersDeliveredList = "false";
                this.OrdersAceptedList = "false";
                this.OrdersCanceledList = "false";
                this.OrdersCirculationList = "true";

                if (this.listCirculation.Count() == 0)
                {
                    this.OrdersCirculationText = "true";
                    this.OrdersCirculationList = "false";
                }
            }

            if (status == "alistamiento")
            {
                this.OrdersInitiatedText = "true";
                this.OrdersEnlistmentText = "false";
                this.OrdersDeliveredText = "true";
                this.OrdersAceptedText = "true";
                this.OrdersCanceledText = "true";
                this.OrdersCirculationText = "true";

                this.OrdersInitiatedList = "false";
                this.OrdersEnlistmentList = "true";
                this.OrdersDeliveredList = "false";
                this.OrdersAceptedList = "false";
                this.OrdersCanceledList = "false";
                this.OrdersCirculationList = "false";

                if (this.listEnlistment.Count() == 0)
                {
                    this.OrdersEnlistmentText = "true";
                    this.OrdersEnlistmentList = "false";
                }
            }

            if (status == "entregado")
            {
                this.OrdersInitiatedText = "true";
                this.OrdersEnlistmentText = "true";
                this.OrdersDeliveredText = "false";
                this.OrdersAceptedText = "true";
                this.OrdersCanceledText = "true";
                this.OrdersCirculationText = "true";

                this.OrdersInitiatedList = "false";
                this.OrdersEnlistmentList = "false";
                this.OrdersDeliveredList = "true";
                this.OrdersAceptedList = "false";
                this.OrdersCanceledList = "false";
                this.OrdersCirculationList = "false";

                if (this.listDelivered.Count() == 0)
                {
                    this.OrdersDeliveredText = "true";
                    this.OrdersDeliveredList = "false";
                }
            }
        }
    }
}
