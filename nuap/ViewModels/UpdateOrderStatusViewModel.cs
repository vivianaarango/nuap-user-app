namespace nuap.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Services;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Views;

    public class UpdateOrderStatusViewModel: BaseViewModel
    {
        private ApiService apiService;
        private int orderID;
        private string orderAddress;
        private string orderStatus;
        private string circleOne;
        private string circleTwo;
        private string circleThree;
        private string circleFour;
        private string circleFive;


        public string OrderStatus
        {
            get { return this.orderStatus; }
            set { SetValue(ref this.orderStatus, value); }
        }

        public string OrderAddress
        {
            get { return this.orderAddress; }
            set { SetValue(ref this.orderAddress, value); }
        }

        public string CircleOne
        {
            get { return this.circleOne; }
            set { SetValue(ref this.circleOne, value); }
        }
        public string CircleTwo
        {
            get { return this.circleTwo; }
            set { SetValue(ref this.circleTwo, value); }
        }
        public string CircleThree
        {
            get { return this.circleThree; }
            set { SetValue(ref this.circleThree, value); }
        }
        public string CircleFour
        {
            get { return this.circleFour; }
            set { SetValue(ref this.circleFour, value); }
        }
        public string CircleFive
        {
            get { return this.circleFive; }
            set { SetValue(ref this.circleFive, value); }
        }

        public int OrderID
        {
            get { return this.orderID; }
            set { SetValue(ref this.orderID, value); }
        }

        public ICommand UpdateStatusCommand
        {
            get
            {
                return new RelayCommand(UpdateStatus);
            }
        }

        public UpdateOrderStatusViewModel()
        {
            this.apiService = new ApiService();
            this.LoadStatus();
        }

        private void LoadStatus()
        {
            var mainViewModel = MainViewModel.GetInstance();
            this.OrderID = mainViewModel.OrderID;
            this.OrderAddress = mainViewModel.OrderAddress;
            this.OrderStatus = mainViewModel.OrderStatus;

            if (this.OrderStatus == "Iniciado")
            {
                this.CircleOne = "linea_red";
                this.CircleTwo = "linea_gray";
                this.CircleThree = "linea_gray";
                this.CircleFour = "linea_gray";
                this.CircleFive = "circle_gray";
            }

            if (this.OrderStatus == "Aceptado")
            {
                this.CircleOne = "linea_azul";
                this.CircleTwo = "linea_red";
                this.CircleThree = "linea_gray";
                this.CircleFour = "linea_gray";
                this.CircleFive = "circle_gray";
            }

            if (this.OrderStatus == "Alistamiento")
            {
                this.CircleOne = "linea_azul";
                this.CircleTwo = "linea_azul";
                this.CircleThree = "linea_red";
                this.CircleFour = "linea_gray";
                this.CircleFive = "circle_gray";
            }

            if (this.OrderStatus == "Circulación")
            {
                this.CircleOne = "linea_azul";
                this.CircleTwo = "linea_azul";
                this.CircleThree = "linea_azul";
                this.CircleFour = "linea_red";
                this.CircleFive = "circle_gray";
            }

            if (this.OrderStatus == "Entregado")
            {
                this.CircleOne = "linea_azul";
                this.CircleTwo = "linea_azul";
                this.CircleThree = "linea_azul";
                this.CircleFour = "linea_azul";
                this.CircleFive = "circle_red";
            }
        }

        private async void UpdateStatus()
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

            var response = await this.apiService.UpdateStatus(
                mainViewModel.User.api_token,
                "https://thenuap.com",
                this.OrderID);

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

          
            await Application.Current.MainPage.DisplayAlert(
                "",
                "Se ha actualizado el pedido exitosamente.",
                "Aceptar");
            

            mainViewModel.Sales = new SalesViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new SalesPage());
        }
    }
}
