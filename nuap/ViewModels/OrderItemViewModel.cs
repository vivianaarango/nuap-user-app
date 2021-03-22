namespace nuap.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Views;

    public class OrderItemViewModel : Order
    {
        public ICommand OrderDetailCommand
        {
            get
            {
                return new RelayCommand(OrderDetail);
            }
        }

        private async void OrderDetail()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.OrderDetail = new OrderDetailViewModel(this);
            await Application.Current.MainPage.Navigation.PushAsync(new OrderDetailPage());
        }

        public ICommand SalesDetailCommand
        {
            get
            {
                return new RelayCommand(SalesDetail);
            }
        }

        private async void SalesDetail()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.SalesDetail = new SalesDetailViewModel(this);
            await Application.Current.MainPage.Navigation.PushAsync(new SalesDetailPage());
        }
    }
}
