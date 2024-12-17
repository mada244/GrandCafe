using CoffeeShop.Services;
using Microsoft.Maui.Controls;

namespace CoffeeShop;

public partial class WelcomePage : ContentPage
{
    private readonly MongoDBService _mongoDBService;
    public WelcomePage()
	{
		InitializeComponent();
        _mongoDBService = new MongoDBService();
	}

    private async void OnGetStartedClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GetStartedPage());
    }
}