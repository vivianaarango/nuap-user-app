namespace nuap.ViewModels
{
    using Services;
    using Models;
    using Xamarin.Forms;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using System.Collections.Generic;
    using Views;

    public class ProductDetailViewModel: BaseViewModel
    {
        private ApiService apiService;

        private string name;
        private string image;
        private string brand;
        private string description;
        private int stock;
        private int quantity;
        private double total;
        private double weight;
        private double length;
        private double width;
        private double height;
        private double price;
        private int hasSpecialPrice;
        private Product product;

        public Product Product
        {
            get { return this.product; }
            set { SetValue(ref this.product, value); }
        }

        public string Name
        {
            get { return this.name; }
            set { SetValue(ref this.name, value); }
        }

        public string Image
        {
            get { return this.image; }
            set { SetValue(ref this.image, value); }
        }

        public string Brand
        {
            get { return this.brand; }
            set { SetValue(ref this.brand, value); }
        }

        public string Description
        {
            get { return this.description; }
            set { SetValue(ref this.description, value); }
        }

        public int HasSpecialPrice
        {
            get { return this.hasSpecialPrice; }
            set { SetValue(ref this.hasSpecialPrice, value); }
        }

        public int Stock
        {
            get { return this.stock; }
            set { SetValue(ref this.stock, value); }
        }

        public int Quantity
        {
            get { return this.quantity; }
            set
            {
                SetValue(ref this.quantity, value);
                this.Validate();
            }
        }

        public double Total
        {
            get { return this.total; }
            set { SetValue(ref this.total, value); }
        }

        public double Weight
        {
            get { return this.weight; }
            set { SetValue(ref this.weight, value); }
        }

        public double Length
        {
            get { return this.length; }
            set { SetValue(ref this.length, value); }
        }

        public double Width
        {
            get { return this.width; }
            set { SetValue(ref this.width, value); }
        }

        public double Height
        {
            get { return this.height; }
            set { SetValue(ref this.height, value); }
        }

        public double Price
        {
            get { return this.price; }
            set { SetValue(ref this.price, value); }
        }

        public ICommand DiminishCommand
        {
            get
            {
                return new RelayCommand(Diminish);
            }
        }

        public ICommand IncreaseCommand
        {
            get
            {
                return new RelayCommand(Increase);
            }
        }

        public ICommand AddCartCommand
        {
            get
            {
                return new RelayCommand(AddCart);
            }
        }

        public ProductDetailViewModel(Product product)
        {
            this.Product = product;
            this.apiService = new ApiService();
            this.SetProduct();
        }

        private async void Diminish()
        {
            if ((this.Quantity - 1) < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "No puedes agregar menos de 0 productos",
                    "Aceptar");
                return;
            } else
            {
                this.Quantity = this.Quantity - 1;
                this.Total = this.Price * this.Quantity;
            }
        }

        private async void Increase()
        {
            if ((this.Quantity + 1) > this.Product.Stock)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "No puedes agregar más de "+ this.Product.Stock + " productos",
                    "Aceptar");
                return;
            }
            else
            {
                this.Quantity = this.Quantity + 1;
                this.Total = this.Price * this.Quantity;
            }
        }

        private async void Validate()
        {
            if ((this.Quantity) > this.Product.Stock)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "No puedes agregar más de " + this.Product.Stock + " productos",
                    "Aceptar");
                return;
            }
            else if (this.Quantity < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "No puedes agregar menos de 0 productos",
                    "Aceptar");
                return;
            } else
            {
                this.Total = this.Price * this.Quantity;
            }
        }

        private async void AddCart()
        {
            Cart cart = new Cart();
            cart.Id = this.Product.Id;
            cart.Quantity = this.Quantity;
            cart.Image = this.Image;
            cart.Price = this.Price;
            cart.Name = this.Name; 

            var mainViewModel = MainViewModel.GetInstance();
            if (mainViewModel.CartList == null)
            {
                mainViewModel.CartList = new List<Cart>();
                mainViewModel.CartList.Add(cart);
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Hemos agregado este producto a tu carrito",
                    "Aceptar");

                mainViewModel.HomeCommerce = new HomeCommerceViewModel();
                await Application.Current.MainPage.Navigation.PushAsync(new HomeCommerceTabbedPage());
            } else
            {
                List<Cart> cartList = new List<Cart>();
                foreach (var item in mainViewModel.CartList)
                {
                    if (item.Id != cart.Id)
                    {
                        cartList.Add(item);
                        continue;
                    } else
                    {
                        cart.Quantity = cart.Quantity + item.Quantity;
                    }
                }

                cartList.Add(cart);
                mainViewModel.CartList = new List<Cart>();
                mainViewModel.CartList = cartList;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Hemos agregado este producto a tu carrito",
                    "Aceptar");

                mainViewModel.HomeCommerce = new HomeCommerceViewModel();
                await Application.Current.MainPage.Navigation.PushAsync(new HomeCommerceTabbedPage());
            }
        }

        private void SetProduct()
        {
            this.Quantity = 0;
            this.Name = this.Product.Name;
            this.Brand = this.Product.Brand;
            this.Description = this.Product.Description;
            this.Stock = this.Product.Stock;
            this.Weight = this.Product.Weight;
            this.Length = this.Product.Length;
            this.Width = this.Product.Width;
            this.Height = this.Product.Height;
            if (this.Product.HasSpecialPrice == 1)
            {
                this.Price = this.Product.SalePrice - (this.Product.SalePrice * this.Product.SpecialPrice / 100);
            } else
            {
                this.Price = this.Product.SalePrice;
            }
            this.HasSpecialPrice = this.Product.HasSpecialPrice;

            if (this.Product.Image.Contains("https://thenuap.com/"))
            {
                this.Image = this.Product.Image;
            } else
            {
                this.Image = "https://thenuap.com/" + this.Product.Image;
            }
            
        }
    }
}
