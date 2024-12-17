using CoffeeShopClassLibrary.Models;
using CoffeeShop.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Diagnostics;

namespace CoffeeShop.Pages;

public partial class EditProfilePage : ContentPage
{
    private User _user;
    private readonly UserService _userService;
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public EditProfilePage(User user)
    {
        InitializeComponent();
        _userService = new UserService(App.BaseAddress);
        _user = user;
        BindingContext = user;
    }

    //public Command SaveCommand => new Command(async () => await SaveChangesAsync());

    private async void OnEditUserClicked(object sender, EventArgs e)
    {
        bool success = await UpdateUser();
        if (success)
        {
            await DisplayAlert("Success", "User updated successfully.", "OK");
            Application.Current.MainPage = new NavigationPage(new AppShell());
        }
        else
        {
            await DisplayAlert("Error", "Failed to update user.", "OK");
        }
    }

    private async Task<bool> UpdateUser()
    {
        try
        {
            await _userService.UpdateUserAsync(_user.ID, _user);
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
        await Navigation.PushAsync(new Pages.ProfilePage(_user));
    }
}