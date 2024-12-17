using CoffeeShopClassLibrary.Models;
using CoffeeShop.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CoffeeShop.Pages.CategoryPages;

public partial class DrinkCategory : ContentPage
{
    private readonly ProductService _productService;
    private User User;
    private UserService _userService;
    public ICommand AddToCartCommand { get; }
    public ObservableCollection<Product> Products { get; private set; }
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
    public DrinkCategory()
    {
        InitializeComponent();
        _productService = new ProductService(App.BaseAddress);
        _userService = new UserService(App.BaseAddress);
        Products = new ObservableCollection<Product>();
        AddToCartCommand = new Command<Product>(AddToCart);
        AddToFavoriteCommand = new Command<Product>(AddToFavorite);
        FavoriteProducts = new ObservableCollection<Product>();
        BindingContext = this;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadProductsAsync();
    }
    async void OnTextChanged(object sender, EventArgs e)
    {
        SearchBar searchBar = (SearchBar)sender;
        var products = await _productService.GetAllProductsAsync();
        var filtered = products.Where(p => p.Category == "Drinks");
        if (!string.IsNullOrEmpty(searchBar.Text))
            filtered = products.Where(x => x.Denumire.Contains(searchBar.Text, StringComparison.OrdinalIgnoreCase)).ToList();
        Products = new ObservableCollection<Product>(filtered);
        OnPropertyChanged(nameof(Products));
    }
    private void AddToCart(Product product)
    {
        OrderPage.ShoppingCart.Add(product);
        OnPropertyChanged(nameof(OrderPage.ShoppingCart));
    }
    private async Task LoadProductsAsync()
    {
        var allProducts = await _productService.GetAllProductsAsync();
        var drinks = allProducts.Where(p => p.Category == "Drinks");
        Products.Clear();
        foreach (var product in drinks)
        {
            Products.Add(product);
        }
    }
    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }
    private async void OnFavClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FavoritesPage());
    }
    private async void OnMenuClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Pages.MenuPage());
    }
    private async void OnCartClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new OrderPage());
    }
    private async void OnProfileClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage(User));
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
    public ICommand AddToFavoriteCommand { get; }
    private async void AddToFavorite(Product product)
    {
        var loggedUser = await AuthService.GetLoggedUser(_userService);
        loggedUser.FavouriteProducts.Add(product.ID);
        await _userService.UpdateUserAsync(loggedUser.ID, loggedUser);
    }
}
