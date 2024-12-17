using CoffeeShop.Services;
using CoffeeShopClassLibrary.Models;
using Microsoft.Maui.Controls;
using MongoDB.Bson;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CoffeeShop;

public partial class SignUpPage : ContentPage
{
    private readonly UserService _userService;
    public SignUpPage()
    {
        _userService = new UserService(App.BaseAddress);
        InitializeComponent();
    }

    private async void OnSignUpClicked(object sender, EventArgs e)
    {
        string username = NameEntry.Text;
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;
        bool rememberMe = RememberMeCheckBox.IsChecked;
        DateTime createdAt = DateTime.Now;

        if (!email.Contains("@") || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            await DisplayAlert("Error", "Please enter a valid email address.", "OK");
            return;
        }

        bool success = await SignUpAsync(email, password, username, createdAt);
        if (success)
        {
            await Navigation.PushAsync(new VerificationPage("SignUp", email));
        }
        else
        {
            await DisplayAlert("Error", "Registration failed. Email may already be in use or other error occurred.", "OK");
        }
    }

    private async Task<bool> SignUpAsync(string email, string password, string username, DateTime createdAt)
    {
        var userExists = await _userService.UserExistsAsync(email);
        if (userExists)
        {
            return false;
        }

        var newUser = new User
        {
            ID = Utils.Utils.GenerateHexId(12), // used like this with value 12 for 24 character hex id
            Username = username,
            Email = email,
            Password = password,
            CreatedAt = createdAt,
            IsActive = "true", 
            Roles = "user" ,
            FavouriteProducts = new List<string>()
        };

        try
        {
            await _userService.CreateUserAsync(newUser);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to create user: {ex.Message}");
            return false;
        }
    }

    private async void OnGoogleSignUpClicked(object sender, EventArgs e)
    {
    }

    private async void OnFacebookSignUpClicked(object sender, EventArgs e)
    {
    }

    private async void OnSignInTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignInPage());
    }
}
