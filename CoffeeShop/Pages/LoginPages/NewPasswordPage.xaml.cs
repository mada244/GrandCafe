using CoffeeShop.Services;

namespace CoffeeShop.Pages.Logare;

public partial class NewPasswordPage : ContentPage
{
    private readonly UserService _userService;
    private string email;
    public NewPasswordPage(string _email)
	{
        _userService = new UserService(App.BaseAddress);
        InitializeComponent();
        email = _email;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        string newPassword = NewPasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

        if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
        {
            DisplayAlert("Error", "Please enter both password fields.", "OK");
        }

        if (newPassword != confirmPassword)
        {
            DisplayAlert("Error", "Passwords do not match. Please try again.", "OK");
        }

        //string UpdateUser = await _userService.UpdateUserAsync(newPassword);
        var users = await _userService.GetAllUsersAsync();
        var user = users.FirstOrDefault(x => x.Email == email);
        if (user == null)
        {
            await DisplayAlert("Error", "User not found.", "OK");
        }
        user.Password = newPassword; 

        try
        {
            await _userService.UpdateUserAsync(user.ID, user);
            await DisplayAlert("Success", "Password updated successfully.", "OK");
            await Navigation.PushAsync(new MainPage());
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Failed to update password: " + ex.Message, "OK");
        }
    }
    private void SaveNewPassword(string newPassword)
    {
        Console.WriteLine($"New password saved: {newPassword}");
    }
}
