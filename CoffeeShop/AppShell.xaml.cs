using CoffeeShop.Services;
using CoffeeShopClassLibrary.Models;
using System.Windows.Input;

namespace CoffeeShop
{
    public partial class AppShell : Shell
    {
        public ICommand LogoutCommand { get; }
        private User _user;
        private string _activeTab = "home";
        public string Username => User?.Username;
        public UserService _userService { get; set; }
        public User User
        {
            get => _user;
            private set
            {
                if (_user != value)
                {
                    _user = value;

                    OnPropertyChanged(nameof(Username));
                }
            }
        }
        public AppShell()
        {
            _userService = new UserService(App.BaseAddress);
            BindingContext = this;
            InitializeComponent();
            LogoutCommand = new Command(ExecuteLogout);
        }

        protected override async void OnAppearing()
        {
            var userEmail = await AuthService.GetLoggedUserEmail();
            this.User = (await _userService.GetAllUsersAsync()).Where(x => x.Email == userEmail).FirstOrDefault();
            base.OnAppearing();
        }

        private async void LogoutClicked(object sender, EventArgs e)
        {
            ExecuteLogout();
        }

        private async void ExecuteLogout()
        {
            await SecureStorage.SetAsync("auth_token", "");
            Application.Current.MainPage = new NavigationPage(new WelcomePage());
        }
    }
}
