using CoffeeShop.Services;

namespace CoffeeShop;

public partial class GetStartedPage : ContentPage
{
    public GetStartedPage()
    {
        InitializeComponent();
    }

    private async void OnSignInClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignInPage());
    }

    private async void OnCreateAccountClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignUpPage());
    }

    private async void OnSkipForNowTapped(object sender, EventArgs e)
    {
        await SecureStorage.SetAsync("auth_token", "");
        Application.Current.MainPage = new AppShell();
    }
}
