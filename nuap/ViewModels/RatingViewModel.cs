namespace nuap.ViewModels
{
    using Services;
    using Models;
    using Xamarin.Forms;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using System.Linq;
    using Views;

    public class RatingViewModel: BaseViewModel
    {
        private ApiService apiService;
        private int orderId;
        private int rating;
        private bool isEnabled;
        private string oneStar;
        private string twoStars;
        private string threeStars;
        private string fourStars;
        private string fiveStars;
        private string comment;

        public Order Order
        {
            get;
            set;
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public int OrderId
        {
            get { return this.orderId; }
            set { SetValue(ref this.orderId, value); }
        }

        public int Rating
        {
            get { return this.rating; }
            set { SetValue(ref this.rating, value); }
        }

        public string Comment
        {
            get { return this.comment; }
            set { SetValue(ref this.comment, value); }
        }

        public string OneStar
        {
            get { return this.oneStar; }
            set { SetValue(ref this.oneStar, value); }
        }

        public string TwoStars
        {
            get { return this.twoStars; }
            set { SetValue(ref this.twoStars, value); }
        }

        public string ThreeStars
        {
            get { return this.threeStars; }
            set { SetValue(ref this.threeStars, value); }
        }

        public string FourStars
        {
            get { return this.fourStars; }
            set { SetValue(ref this.fourStars, value); }
        }

        public string FiveStars
        {
            get { return this.fiveStars; }
            set { SetValue(ref this.fiveStars, value); }
        }

        public ICommand OneStarCommand
        {
            get
            {
                return new RelayCommand(OneStarSelect);
            }
        }

        public ICommand TwoStarsCommand
        {
            get
            {
                return new RelayCommand(TwoStarsSelect);
            }
        }

        public ICommand ThreeStarsCommand
        {
            get
            {
                return new RelayCommand(ThreeStarsSelect);
            }
        }

        public ICommand FourStarsCommand
        {
            get
            {
                return new RelayCommand(FourStarsSelect);
            }
        }

        public ICommand FiveStarsCommand
        {
            get
            {
                return new RelayCommand(FiveStarsSelect);
            }
        }

        public ICommand SendRatingCommand
        {
            get
            {
                return new RelayCommand(SendRating);
            }
        }

        public RatingViewModel()
        {
            this.apiService = new ApiService();
            this.LoadData();
        }

        private void LoadData()
        {
            var mainViewModel = MainViewModel.GetInstance();
            this.OrderId = mainViewModel.OrderID;
            this.IsEnabled = true;
            this.OneStar = "ic_estrella_disabled.png";
            this.TwoStars = "ic_estrella_disabled.png";
            this.ThreeStars = "ic_estrella_disabled.png";
            this.FourStars = "ic_estrella_disabled.png";
            this.FiveStars = "ic_estrella_disabled.png";
            this.Rating = 0;
        }

        private void OneStarSelect()
        {
            this.OneStar = "ic_estrella.png";
            this.TwoStars = "ic_estrella_disabled.png";
            this.ThreeStars = "ic_estrella_disabled.png";
            this.FourStars = "ic_estrella_disabled.png";
            this.FiveStars = "ic_estrella_disabled.png";
            this.Rating = 1;
        }

        private void TwoStarsSelect()
        {
            this.OneStar = "ic_estrella.png";
            this.TwoStars = "ic_estrella.png";
            this.ThreeStars = "ic_estrella_disabled.png";
            this.FourStars = "ic_estrella_disabled.png";
            this.FiveStars = "ic_estrella_disabled.png";
            this.Rating = 2;
        }

        private void ThreeStarsSelect()
        {
            this.OneStar = "ic_estrella.png";
            this.TwoStars = "ic_estrella.png";
            this.ThreeStars = "ic_estrella.png";
            this.FourStars = "ic_estrella_disabled.png";
            this.FiveStars = "ic_estrella_disabled.png";
            this.Rating = 3;
        }

        private void FourStarsSelect()
        {
            this.OneStar = "ic_estrella.png";
            this.TwoStars = "ic_estrella.png";
            this.ThreeStars = "ic_estrella.png";
            this.FourStars = "ic_estrella.png";
            this.FiveStars = "ic_estrella_disabled.png";
            this.Rating = 4;
        }

        private void FiveStarsSelect()
        {
            this.OneStar = "ic_estrella.png";
            this.TwoStars = "ic_estrella.png";
            this.ThreeStars = "ic_estrella.png";
            this.FourStars = "ic_estrella.png";
            this.FiveStars = "ic_estrella.png";
            this.Rating = 5;
        }

        private async void SendRating()
        {
            if (string.IsNullOrEmpty(this.Comment))
            {
                await Application.Current.MainPage.DisplayAlert(
                        "",
                        "Por favor dejanos un comentario para poder mejorar",
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

            var mainViewModel = MainViewModel.GetInstance();
            var response = await this.apiService.RatingOrder(
                mainViewModel.User.api_token,
                "https://thenuap.com",
                this.OrderId,
                this.Rating,
                this.Comment);

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
                this.Comment = "";
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Errors.First().Detail,
                    "Aceptar");
                return;
            }

            this.IsEnabled = false;
            this.Comment = "";
            this.Rating = 0;

            await Application.Current.MainPage.DisplayAlert(
                "",
                "Haz calificado tu pedido exitosamente",
                "Aceptar");

            mainViewModel.Orders = new OrdersViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new OrdersPage());
        }
    }
}
