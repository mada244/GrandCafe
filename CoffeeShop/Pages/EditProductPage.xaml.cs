using CoffeeShop.Services;
using CoffeeShopClassLibrary.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CoffeeShop.Pages;

public partial class EditProductPage : ContentPage, INotifyPropertyChanged
{
    private ProductService _productService;
    private Product _product;

    public EditProductPage(Product product)
    {
        InitializeComponent();
        _productService = new ProductService(App.BaseAddress);
        _product = product;
        BindingContext = _product;
    }

    private async void OnEditProductClicked(object sender, EventArgs e)
    {
        bool success = await UpdateProduct();
        if (success)
        {
            await DisplayAlert("Success", "Product updated successfully.", "OK");
            await Navigation.PushAsync(new MainPage()); 
        }
        else
        {
            await DisplayAlert("Error", "Failed to update product.", "OK");
        }
    }

    private async Task<bool> UpdateProduct()
    {
        try
        {
            await _productService.UpdateProductAsync(_product.ID, _product);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to edit product: {ex.Message}");
            return false;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
        //await Navigation.PushAsync(new Pages.OrderPage(ShoppingCart));
    }
    private async void OnProfileClicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new Pages.ProfilePage(_user));
    }
}
