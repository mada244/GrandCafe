using CoffeeShopClassLibrary.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CoffeeShop.Pages;

public partial class ProfilePage : ContentPage
{
    private User _currentUser { get; set; }
    public ICommand LogoutCommand { get; }
    public ObservableCollection<Product> ShoppingCart { get; private set; }
    public ProfilePage(User user)
    {
       InitializeComponent();
        _currentUser = user;
        ShoppingCart = new ObservableCollection<Product>();
        LogoutCommand = new Command(ExecuteLogout);
        BindingContext = this;
        //InitializeComponent();
    }
    private async void ExecuteLogout()
    {
        await SecureStorage.SetAsync("auth_token", "");
        Application.Current.MainPage = new NavigationPage(new WelcomePage());
    }
    public string Initials => $"{_currentUser?.Username[0]}";
    public string Username => _currentUser?.Username;
    public string Email => _currentUser?.Email;
    public string RegistrationDate => _currentUser?.CreatedAt.ToString("dd MMM yyyy");

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private async void OnEditClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EditProfilePage(_currentUser));
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
        await Navigation.PushAsync(new Pages.OrderPage());
    }
    private async void OnProfileClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Pages.ProfilePage(_currentUser));
    }
}
