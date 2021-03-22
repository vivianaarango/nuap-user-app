namespace nuap.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Views;

    public class ProductCommerceItemViewModel : Product
    {
        public ICommand EditProductCommand
        {
            get
            {
                return new RelayCommand(EditProduct);
            }
        }

        public ICommand ProductDetailCommand
        {
            get
            {
                return new RelayCommand(ProductDetail);
            }
        }

        private async void EditProduct()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.EditProduct = new EditProductViewModel(this);
            await Application.Current.MainPage.Navigation.PushAsync(new EditProductPage());
        }

        private async void ProductDetail()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.ProductDetail = new ProductDetailViewModel(this);
            await Application.Current.MainPage.Navigation.PushAsync(new ProductDetailPage());
        }
    }
}
