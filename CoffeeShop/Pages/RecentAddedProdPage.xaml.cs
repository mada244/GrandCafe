using CoffeeShop.Services;
using CoffeeShopClassLibrary.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CoffeeShop.Pages;

public partial class RecentAddedProdPage : ContentPage
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
    public RecentAddedProdPage()
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
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadLastProductsAsync();
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
    private async Task LoadLastProductsAsync()
    {
        var products = await _productService.GetAllProductsAsync();
        var lastProd = products.OrderByDescending(x => x.ID);
        Products.Clear();
        foreach (var product in lastProd)
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
        //await Navigation.PushAsync(new Pages.ProfilePage(User));
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
}