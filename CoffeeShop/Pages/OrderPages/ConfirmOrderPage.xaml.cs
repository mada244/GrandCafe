namespace CoffeeShop.Pages;

public partial class ConfirmOrderPage : ContentPage
{
	public ConfirmOrderPage()
	{
		InitializeComponent();
	}

    private async void OnTrackOrderClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new NavigationPage(new MainPage());
    }
}