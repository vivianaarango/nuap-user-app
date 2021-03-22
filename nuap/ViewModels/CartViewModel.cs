namespace nuap.ViewModels
{
    using System.Collections.ObjectModel;
    using Models;

    public class CartViewModel: BaseViewModel
    {
        private ObservableCollection<Cart> cart;
        private bool isRefreshing;

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        public ObservableCollection<Cart> Cart
        {
            get { return this.cart; }
            set { SetValue(ref this.cart, value); }
        }

        public CartViewModel()
        {
            this.LoadCart();
        }

        private void LoadCart()
        {
            var mainViewModel = MainViewModel.GetInstance();

            foreach (var item in mainViewModel.CartList)
            {
                item.Price = item.Quantity * item.Price;
            }

            this.Cart = new ObservableCollection<Cart>(mainViewModel.CartList);
            this.IsRefreshing = false;
        }
    }
}
