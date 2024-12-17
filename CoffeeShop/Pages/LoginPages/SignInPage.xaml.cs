
using Microsoft.Maui.Controls;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using CoffeeShop.Services;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;

namespace CoffeeShop;

public partial class SignInPage : ContentPage
{
    private readonly UserService _userService;
  
    public SignInPage()
    {
        _userService = new UserService(App.BaseAddress);
        InitializeComponent();
    }

    private async void OnSignInClicked(object sender, EventArgs e)
    {
        string email = emailEntry.Text;
        string password = passwordEntry.Text;
        bool rememberMe = rememberMeCheckBox.IsChecked;

        bool success = await AuthService.SignInAsync(email, password, rememberMe, _userService);
        if (success == true)
        {
            Application.Current.MainPage = new NavigationPage(new AppShell());
        }
        else
        {
            await DisplayAlert("Error", "Invalid email or password", "OK");
        }
    }

    private async void OnForgotPasswordTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ForgotPasswordPage());
    }

    private async void OnGoogleSignInClicked(object sender, EventArgs e)
    {
    }

    private async void OnFacebookSignInClicked(object sender, EventArgs e)
    {
    }

    private async void OnSignUpTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignUpPage());
    }
}

