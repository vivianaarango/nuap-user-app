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
    using Views;

    public class CategoryProductViewModel: BaseViewModel
    {
        private ApiService apiService;
        private ObservableCollection <ProductCommerceItemViewModel> salesProducts;
        private ObservableCollection <ProductCommerceItemViewModel> productsCategory;
        private ObservableCollection <ProductCommerceItemViewModel> productsSubcategory;
        private ObservableCollection <ProductCommerceItemViewModel> productsSubcategoryOne;
        private List <Product> salesProductsList;
        private List <Product> productsCategoryList;
        private List <Product> productsSubcategoryList;
        private List <Product> productsSubcategoryOneList;
        private bool salesProductsVisibility;
        private bool productsCategoryVisibility;
        private bool productsSubcategoryVisibility;
        private bool productsSubcategoryOneVisibility;
        private int salesProductsCount;
        private int productsCategoryCount;
        private int productsSubcategoryCount;
        private int productsSubcategoryOneCount;
        private bool isRefreshing;
        private string categoryName;
        private string filter;
        private int subcategoryName;
        private int subcategoryNameOne;

        public Category Category
        {
            get;
            set;
        }

        public string Filter
        {
            get { return this.filter; }
            set
            {
                SetValue(ref this.filter, value);
                //this.Search();
            }
        }

        public int SalesProductsCount
        {
            get { return this.salesProductsCount; }
            set { SetValue(ref this.salesProductsCount, value); }
        }

        public int ProductsCategoryCount
        {
            get { return this.productsCategoryCount; }
            set { SetValue(ref this.productsCategoryCount, value); }
        }

        public int ProductsSubcategoryCount
        {
            get { return this.productsSubcategoryCount; }
            set { SetValue(ref this.productsSubcategoryCount, value); }
        }

        public int ProductsSubcategoryOneCount
        {
            get { return this.productsSubcategoryOneCount; }
            set { SetValue(ref this.productsSubcategoryOneCount, value); }
        }

        public bool SalesProductsVisibility
        {
            get { return this.salesProductsVisibility; }
            set { SetValue(ref this.salesProductsVisibility, value); }
        }

        public bool ProductsCategoryVisibility
        {
            get { return this.productsCategoryVisibility; }
            set { SetValue(ref this.productsCategoryVisibility, value); }
        }

        public bool ProductsSubcategoryVisibility
        {
            get { return this.productsSubcategoryVisibility; }
            set { SetValue(ref this.productsSubcategoryVisibility, value); }
        }

        public bool ProductsSubcategoryOneVisibility
        {
            get { return this.productsSubcategoryOneVisibility; }
            set { SetValue(ref this.productsSubcategoryOneVisibility, value); }
        }

        public ObservableCollection<ProductCommerceItemViewModel> SalesProducts
        {
            get { return this.salesProducts; }
            set { SetValue(ref this.salesProducts, value); }
        }

        public ObservableCollection<ProductCommerceItemViewModel> ProductsCategory
        {
            get { return this.productsCategory; }
            set { SetValue(ref this.productsCategory, value); }
        }

        public ObservableCollection<ProductCommerceItemViewModel> ProductsSubcategory
        {
            get { return this.productsSubcategory; }
            set { SetValue(ref this.productsSubcategory, value); }
        }

        public ObservableCollection<ProductCommerceItemViewModel> ProductsSubcategoryOne
        {
            get { return this.productsSubcategoryOne; }
            set { SetValue(ref this.productsSubcategoryOne, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        public string CategoryName
        {
            get { return this.categoryName; }
            set { SetValue(ref this.categoryName, value); }
        }

        public int SubcategoryName
        {
            get { return this.subcategoryName; }
            set { SetValue(ref this.subcategoryName, value); }
        }

        public int SubcategoryNameOne
        {
            get { return this.subcategoryNameOne; }
            set { SetValue(ref this.subcategoryNameOne , value); }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadProducts);
            }
        }

        public CategoryProductViewModel(Category category)
        {
            this.Category = category;
            this.CategoryName = this.Category.Name;
            this.apiService = new ApiService();
            this.LoadProducts();
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

            var response = await this.apiService.GetCategoryProducts(
                mainViewModel.User.api_token,
                "https://thenuap.com",
                this.Category.Id,
                19);

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
            mainViewModel.ProductCategoryList = list;

            this.salesProductsList = new List<Product>();
            this.productsCategoryList = new List<Product>();
            this.productsSubcategoryList = new List<Product>();
            this.productsSubcategoryOneList = new List<Product>();

            this.SalesProductsVisibility = false;
            this.ProductsCategoryVisibility = false;
            this.ProductsSubcategoryVisibility = false;
            this.ProductsSubcategoryOneVisibility = false;

            this.SubcategoryName = 0;
            this.SubcategoryNameOne = 0;

            foreach (var item in list)
            {
                if (item.CategoryId != this.Category.Id && this.SubcategoryName == 0)
                {
                    this.SubcategoryName = item.CategoryId;
                    continue;
                }

                if (item.CategoryId != this.Category.Id && this.SubcategoryNameOne == 0)
                {
                    this.SubcategoryNameOne = item.CategoryId;
                }
            }

            foreach (var item in list)
            {
                if (item.CategoryId == this.Category.Id)
                {
                    this.ProductsCategoryVisibility = true;
                    productsCategoryList.Add(item);
                }

                if (item.CategoryId == this.SubcategoryName)
                {
                    this.ProductsSubcategoryVisibility = true;
                    productsSubcategoryList.Add(item);
                }

                if (item.CategoryId == this.SubcategoryNameOne)
                {
                    this.ProductsSubcategoryOneVisibility = true;
                    productsSubcategoryOneList.Add(item);
                }

                if (item.HasSpecialPrice == 1)
                {
                    this.SalesProductsVisibility = true;
                    item.Status = item.SalePrice - (item.SalePrice * item.SpecialPrice / 100);
                    salesProductsList.Add(item);
                }
            }

            this.SalesProductsCount = (this.salesProductsList.Count() * 115) + 115;
            this.ProductsCategoryCount = (this.productsCategoryList.Count() * 115) + 115;
            this.ProductsSubcategoryCount = (this.productsSubcategoryList.Count() * 115) + 115;
            this.ProductsSubcategoryOneCount = (this.productsSubcategoryOneList.Count() * 115) + 115;

            this.SalesProducts = new ObservableCollection <ProductCommerceItemViewModel>(ToProductCommerceViewModel(this.salesProductsList));
            this.ProductsCategory = new ObservableCollection <ProductCommerceItemViewModel>(ToProductCommerceViewModel(this.productsCategoryList));
            this.ProductsSubcategory = new ObservableCollection <ProductCommerceItemViewModel>(ToProductCommerceViewModel(this.productsSubcategoryList));
            this.ProductsSubcategoryOne = new ObservableCollection <ProductCommerceItemViewModel>(ToProductCommerceViewModel(this.productsSubcategoryOneList));
            this.IsRefreshing = false;
        }

        private IEnumerable<ProductCommerceItemViewModel> ToProductCommerceViewModel(List<Product> list)
        {
            return list.Select(p => new ProductCommerceItemViewModel
            {
                Id = p.Id,
                Name = p.Name,
                CategoryId = p.CategoryId,
                UserId = p.UserId,
                Sku = p.Sku,
                Brand = p.Brand,
                Description = p.Description,
                Status = p.Status,
                IsFeatured = p.IsFeatured,
                Stock = p.Stock,
                Weight = p.Weight,
                Length = p.Length,
                Width = p.Width,
                Height = p.Height,
                PurchasePrice = p.PurchasePrice,
                SalePrice = p.SalePrice,
                SpecialPrice = p.SpecialPrice,
                HasSpecialPrice = p.HasSpecialPrice,
                Image = "https://thenuap.com/" + p.Image,
                Position = p.Position,
                StatusText = p.StatusText,
                StatusColor = p.StatusColor
            });
        }
    }
}
