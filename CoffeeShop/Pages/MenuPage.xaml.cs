using CoffeeShop.Services;
using CoffeeShopClassLibrary.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CoffeeShop.Pages;

public partial class MenuPage : ContentPage
{
    private readonly ProductService _productService;
    private User User;
    private UserService _userService;
    public ObservableCollection<Product> LimitedProducts { get; private set; }
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
    public MenuPage()
	{
		InitializeComponent();
        _productService = new ProductService(App.BaseAddress);
        _userService = new UserService(App.BaseAddress);
        LimitedProducts = new ObservableCollection<Product>();
        BindingContext = this;
        FavoriteProducts = new ObservableCollection<Product>();
        AddToCartCommand = new Command<Product>(AddToCart);
        AddToFavoriteCommand = new Command<Product>(AddToFavorite);
        OnCoffeeClicked(null, null);
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
    private async void OnCoffeeClicked(object sender, EventArgs e)
    {
        var allProducts = await _productService.GetAllProductsAsync();
        var coffee = allProducts.Where(p => p.Category == "Coffee");
        LimitedProducts.Clear();
        foreach (var product in coffee)
        {
            LimitedProducts.Add(product);
        }
    }

    private async void OnDrinkClicked(object sender, EventArgs e)
    {
        var allProducts = await _productService.GetAllProductsAsync();
        var drinks = allProducts.Where(p => p.Category == "Drinks");
        LimitedProducts.Clear();
        foreach (var product in drinks)
        {
            LimitedProducts.Add(product);
        }
    }
    private async void OnJuiceClicked(object sender, EventArgs e)
    {
        var allProducts = await _productService.GetAllProductsAsync();
        var juice = allProducts.Where(p => p.Category == "Juice");
        LimitedProducts.Clear();
        foreach (var product in juice)
        {
            LimitedProducts.Add(product);
        }
    }
    private async void OnCakeClicked(object sender, EventArgs e)
    {
        var allProducts = await _productService.GetAllProductsAsync();
        var cakes = allProducts.Where(p => p.Category == "Cake");
        LimitedProducts.Clear();
        foreach (var product in cakes)
        {
            LimitedProducts.Add(product);
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
}