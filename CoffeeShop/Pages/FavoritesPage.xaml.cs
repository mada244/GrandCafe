using CoffeeShop.Services;
using CoffeeShopClassLibrary.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CoffeeShop.Pages;

public partial class FavoritesPage : ContentPage, INotifyPropertyChanged
{
    private readonly ProductService _productService;
    private readonly UserService _userService;
    public ObservableCollection<Product> FavoriteProducts { get; set; }
    public ICommand AddToCartCommand { get; }
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public FavoritesPage()
    {
        _productService = new ProductService(App.BaseAddress);
        _userService = new UserService(App.BaseAddress);
        FavoriteProducts = new ObservableCollection<Product>();
        AddToCartCommand = new Command<Product>(AddToCart);
        RemoveProductCommand = new Command<Product>(RemoveProduct);
        InitializeComponent();
        BindingContext = this;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await InitializeProducts();
    }
    private void AddToCart(Product product)
    {
        OrderPage.ShoppingCart.Add(product);
        OnPropertyChanged(nameof(OrderPage.ShoppingCart));
    }
    public async Task InitializeProducts()
    {
        FavoriteProducts.Clear();
        var loggedUser = await AuthService.GetLoggedUserEmail();
        var user = (await _userService.GetAllUsersAsync()).FirstOrDefault(x => x.Email == loggedUser);

        if (user != null && user.FavouriteProducts != null)
        {
            foreach (var item in user.FavouriteProducts)
            {
                var product = await _productService.GetProductByIdAsync(item);
                FavoriteProducts.Add(product);
            }
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
    public ICommand RemoveProductCommand { get; }
    private async void RemoveProduct(Product product)
    {
        if (product != null)
        {
            FavoriteProducts.Remove(product);
            var loggedUser = await AuthService.GetLoggedUserEmail();
            var user = (await _userService.GetAllUsersAsync()).FirstOrDefault(x => x.Email == loggedUser);
            user.FavouriteProducts.Remove(product.ID);
            await _userService.UpdateUserAsync(user.ID, user);
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
        //await Navigation.PushAsync(new Pages.ProfilePage(User));
    }
}