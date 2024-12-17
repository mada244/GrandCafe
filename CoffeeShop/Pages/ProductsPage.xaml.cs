using CoffeeShop.Pages;
using CoffeeShop.Services;
using CoffeeShopClassLibrary.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CoffeeShop;

public partial class ProductsPage : ContentPage, INotifyPropertyChanged
{
    private UserService _userService;
    private User _user;
    private readonly ProductService _productService;
    public ObservableCollection<Product> Products { get; private set; }
    public ICommand AddToCartCommand { get; }
    public ObservableCollection<Product> FavoriteProducts { get; set; }

    private string _searchText;

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText != value)
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _isAdmin = false;
    public bool IsAdmin
    {
        get => _isAdmin;
        set
        {
            if (_isAdmin != value)
            {
                _isAdmin = value;
                OnPropertyChanged(nameof(IsAdmin));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public ProductsPage()
    {
        InitializeComponent();
        _productService = new ProductService(App.BaseAddress);
        _userService = new UserService(App.BaseAddress);
        Products = new ObservableCollection<Product>();
        FavoriteProducts = new ObservableCollection<Product>();
        AddToCartCommand = new Command<Product>(AddToCart);
        AddToFavoriteCommand = new Command<Product>(AddToFavorite);
        BindingContext = this;
    }

    async void OnTextChanged(object sender, EventArgs e)
    {
        SearchBar searchBar = (SearchBar)sender;
        var products = await _productService.GetAllProductsAsync();
        var filtered = products;
        if (!string.IsNullOrEmpty(searchBar.Text))
            filtered = products.Where(x => x.Denumire.Contains(searchBar.Text, StringComparison.OrdinalIgnoreCase)).ToList();
        Products = new ObservableCollection<Product>(filtered);
        OnPropertyChanged(nameof(Products));
    }
    public ICommand AddToFavoriteCommand { get; }
    private async void AddToFavorite(Product product)
    {
        var loggedUser = await AuthService.GetLoggedUser(_userService);
        loggedUser.FavouriteProducts.Add(product.ID);
        await _userService.UpdateUserAsync(loggedUser.ID, loggedUser);
    }
    private void AddToCart(Product product)
    {
        OrderPage.ShoppingCart.Add(product);
        OnPropertyChanged(nameof(OrderPage.ShoppingCart));
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CheckIfUserIsAdmin();
        await LoadProductsAsync();
    }

    private async Task LoadProductsAsync()
    {
        var products = await _productService.GetAllProductsAsync();
        Products.Clear();
        foreach (var product in products)
        {
            Products.Add(product);
        }
    }
    private async void OnHomeClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new NavigationPage(new AppShell());
    }
    private async void OnFavClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Pages.FavoritesPage());
    }
    private async void OnMenuClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Pages.MenuPage());
    }
    private async void OnCartClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Pages.OrderPage());
    }
    private async void OnProfileClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Pages.ProfilePage(_user));
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
    private async void OnAddProductClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Pages.AddProductPage());
    }
    private async Task CheckIfUserIsAdmin()
    {
        var user = await AuthService.GetLoggedUser(_userService);
        if (user != null)
        {
            IsAdmin = string.Equals(user.Roles, "admin", StringComparison.OrdinalIgnoreCase);
        }
    }
}

