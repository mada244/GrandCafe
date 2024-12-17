using CoffeeShop.Pages;
using CoffeeShop.Services;
using CoffeeShopClassLibrary.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CoffeeShop
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        private User _user;
        public User User
        {
            get => _user;
            private set
            {
                if (_user != value)
                {
                    _user = value;

                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public bool IsSearchEmpty { get; set; } = true;
        public bool IsNotSearchEmpty { get; set; } = false;
        public string Username => User?.Username;
        public ObservableCollection<Product> Products { get; private set; }
        private readonly ProductService _productService;
        public ObservableCollection<Product> LimitedProducts { get; set; }
        public ObservableCollection<Product> RecentProducts { get; set; }
        public UserService _userService { get; set; }
        public MainPage()
        {
            InitializeComponent();
            _userService = new UserService(App.BaseAddress);
            _productService = new ProductService(App.BaseAddress);
            Products = new ObservableCollection<Product>();
            LimitedProducts = new ObservableCollection<Product>();
            RecentProducts = new ObservableCollection<Product>();
            AddToCartCommand = new Command<Product>(AddToCart);
            AddToFavoriteCommand = new Command<Product>(AddToFavorite);
            BindingContext = this;
            LoadInitialProductsAsync();
            RecentProductsAsync();
        }
        protected override async void OnAppearing()
        {
            var userEmail = await AuthService.GetLoggedUserEmail();
            this.User = (await _userService.GetAllUsersAsync()).Where(x => x.Email == userEmail).FirstOrDefault();
            base.OnAppearing();
        }
        private async void OnGetDiscountClicked(object sender, EventArgs e)
        {
            DisplayAlert("Discount", "You have claimed the discount!", "OK");
        }
        private async void OnCoffeeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CoffeeCategory());
        }
        private async void OnDrinkClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.CategoryPages.DrinkCategory());
        }
        private async void OnJuiceClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.CategoryPages.JuiceCategory());
        }
        private async void OnCakeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.CategoryPages.CakeCategory());
        }
        private async void OnHomeClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new AppShell());
        }
        private async void OnFavClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FavoritesPage());
        }
        private async void OnMenuClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuPage());
        }
        private async void OnCartClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OrderPage());
        }
        private async void OnProfileClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage(User));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public async void ViewAllProducts(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductsPage());
        }
        public async void ViewRecentAdded(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RecentAddedProdPage());
        }
        public async void LoadInitialProductsAsync()
        {
            var allProducts = await _productService.GetAllProductsAsync();
            LimitedProducts.Clear();
            foreach (var product in allProducts.Take(5))
            {
                LimitedProducts.Add(product);
            }
        }
        public async void RecentProductsAsync()
        {
            var allProducts = await _productService.GetAllProductsAsync();
            var recentprod = allProducts.OrderByDescending(x => x.ID).Take(5);
            RecentProducts.Clear();
            foreach (var product in recentprod)
            {
                RecentProducts.Add(product);
            }
        }
        private async void OnProductTapped(object sender, EventArgs e)
        {
            var view = sender as View;
            if (view != null)
            {
                var product = view.BindingContext as Product;
                if (product != null)
                {
                    await Navigation.PushAsync(new ProductDetailsPage(product));
                }
            }
        }
        async void OnTextChanged(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            if (!string.IsNullOrEmpty(searchBar.Text))
            {

                Products.Clear();
                var products = await _productService.GetAllProductsAsync();
                var filtered = products;
                filtered = products.Where(x => x.Denumire.Contains(searchBar.Text, StringComparison.OrdinalIgnoreCase)).Take(5).ToList();
                Products = new ObservableCollection<Product>(filtered);

                IsSearchEmpty = false;
                IsNotSearchEmpty = true;
                OnPropertyChanged(nameof(IsNotSearchEmpty));
                OnPropertyChanged(nameof(IsSearchEmpty));
                OnPropertyChanged(nameof(Products));
            }
            else
            {
                IsNotSearchEmpty = false;
                IsSearchEmpty = true;
                OnPropertyChanged(nameof(IsNotSearchEmpty));
                OnPropertyChanged(nameof(IsSearchEmpty));
            }
        }

        public ICommand AddToFavoriteCommand { get; }
        private async void AddToFavorite(Product product)
        {
            var loggedUser = await AuthService.GetLoggedUser(_userService);
            loggedUser.FavouriteProducts.Add(product.ID);
            await _userService.UpdateUserAsync(loggedUser.ID, loggedUser);
        }

        public ICommand AddToCartCommand { get; }
        private void AddToCart(Product product)
        {
            OrderPage.ShoppingCart.Add(product);
            OnPropertyChanged(nameof(OrderPage.ShoppingCart));
        }

        private async void OnAddProductClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.AddProductPage());
        }


    }
}