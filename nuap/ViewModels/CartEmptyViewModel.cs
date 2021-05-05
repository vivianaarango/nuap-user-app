namespace nuap.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Views;

    public class CartEmptyViewModel: BaseViewModel
    {
        public ICommand InitPageCommand
        {
            get
            {
                return new RelayCommand(InitPage);
            }
        }

        public CartEmptyViewModel()
        {
        }

        private async void InitPage()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Home = new HomeViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new HomeTabbedPage());
        }
    }
}