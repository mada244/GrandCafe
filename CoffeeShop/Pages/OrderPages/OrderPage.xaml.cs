using CoffeeShop.Services;
using CoffeeShopClassLibrary.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CoffeeShop.Pages;

public partial class OrderPage : ContentPage, INotifyPropertyChanged
{
    private User user;
    public ProductService ProductService { get; set; }
    public static ObservableCollection<Product> ShoppingCart { get; set; } = new ObservableCollection<Product>();
    public ObservableCollection<Product> ActualShoppingCart { get; set; }

    public decimal Total { get; set; } = 0m;

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public OrderPage()
    {
        ProductService = new ProductService(App.BaseAddress);
        ActualShoppingCart = new ObservableCollection<Product>();
        foreach (var item in ShoppingCart)
        {
            ActualShoppingCart.Add(item);
        }
        if (ActualShoppingCart is not null && ActualShoppingCart.Count > 0)
        {
            Total = ActualShoppingCart.Sum(x => decimal.Parse(x.Pret.Split(" ")[0]));
        }
        OnPropertyChanged(nameof(ActualShoppingCart));
        InitializeComponent();
        BindingContext = this;
    }
    private void IncreaseQuantity(object sender, EventArgs e)
    {
        var quantityLabel = this.FindByName<Label>("quantityLabel");
        int currentQuantity = int.Parse(quantityLabel.Text);
        quantityLabel.Text = (currentQuantity + 1).ToString();
    }

    private void DecreaseQuantity(object sender, EventArgs e)
    {
        var quantityLabel = this.FindByName<Label>("quantityLabel");
        int currentQuantity = int.Parse(quantityLabel.Text);
        if (currentQuantity > 1)
        {
            quantityLabel.Text = (currentQuantity - 1).ToString();
        }
    }
    private async void CheckoutClicked(object sender, EventArgs e)
    {
        var url = await ProductService.CreateCheckoutSession(ShoppingCart.ToList());
        await Launcher.OpenAsync(url);
        await Navigation.PushAsync(new ConfirmOrderPage());
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
        await Navigation.PushAsync(new ProfilePage(user));
    }
}