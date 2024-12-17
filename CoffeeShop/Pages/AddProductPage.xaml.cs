using CoffeeShop.Services;
using CoffeeShopClassLibrary.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CoffeeShop.Pages;

public partial class AddProductPage : ContentPage
{
    private ProductService _productService;
    public AddProductPage()
    {
        _productService = new ProductService(App.BaseAddress);
        InitializeComponent();
    }
    private async void OnAddProductClicked(object sender, EventArgs e)
    {
        var nume = nameEntry.Text;
        var pret = priceEntry.Text;
        var descriere = descriptionEntry.Text;
        var categ = categoryEntry.Text;

        bool success = await CreateProduct(nume, pret, descriere, categ);
        if (success)
        {
            await Navigation.PushAsync(new MainPage());
        }
        else
        {
            await DisplayAlert("Error", "Registration failed. Email may already be in use or other error occurred.", "OK");
        }
    }

    private async Task<bool> CreateProduct(string nume, string pret, string descriere, string category)
    {
        var productExists = await _productService.ProductExistsAsync(nume);
        if (productExists)
        {
            return false;
        }

        var newProduct = new Product
        {
            ID = Utils.Utils.GenerateHexId(12), // used like this with value 12 for 24 character hex id
            Denumire = nume,
            Pret = pret,
            Descriere = descriere,
            Category = category,
            ImageUrl = "",
        };

        try
        {
            await _productService.CreateProductAsync(newProduct);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to create user: {ex.Message}");
            return false;
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
        //await Navigation.PushAsync(new ProfilePage(User));
    }
}