using CoffeeShop.Services;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Controls;
using System;
using System.Diagnostics;

namespace CoffeeShop;

public partial class VerificationPage : ContentPage
{
    string _sit = string.Empty;
    private readonly EmailService _emailService;
    private string _verificationCode;
    private string _email;

    public VerificationPage(string sit, string email)
    {
        InitializeComponent();
        _email = email;
        _sit = sit;
        _emailService = new EmailService(email);
        GenerateAndSendCode();
    }

    private async void GenerateAndSendCode()
    {
        _verificationCode = await _emailService.GenerateCode();
        await _emailService.SendEmail(_email, "Verification Code", $"Your verification code is: {_verificationCode}");
    }

    private async void OnResendCodeClicked(object sender, EventArgs e)
    {
        GenerateAndSendCode();
        await DisplayAlert("Resend Code", "The verification code has been resent to your email.", "OK");
    }

    private async void OnVerifyClicked(object sender, EventArgs e)
    {
        string code = VerificationCodeEntry.Text;
        if (string.IsNullOrEmpty(code) || code.Length != 6 || code != _verificationCode)
        {
            await DisplayAlert("Error", "Please enter a valid 6-digit verification code.", "OK");
            return;
        }

        if (_sit == "Forgot")
        {
            await Navigation.PushAsync(new Pages.Logare.NewPasswordPage(_email));
        }
        else
        {
            try
            {
                var token = AuthService.GenerateSimpleToken(_email);
                await SecureStorage.SetAsync("auth_token", token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving token: {ex.Message}");
            }
            await Navigation.PushAsync(new AppShell());
        }
    }
}
