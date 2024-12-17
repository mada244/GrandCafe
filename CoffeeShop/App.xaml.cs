using CoffeeShop.Services;

namespace CoffeeShop
{
    public partial class App : Application
    {
        public static string BaseAddress = "http://10.0.2.2:5081";
        //public static string BaseAddress = "http://localhost:5081";
        public static bool AutoLogin = false;
        public App()
        {
            InitializeComponent();

            if (!AutoLogin)
            {
                MainPage = new NavigationPage(new WelcomePage());
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(new AppShell());
            }
        }

        protected async override void OnStart()
        {
            if (AutoLogin)
            {
                var token = AuthService.GenerateSimpleToken("mada.244@yahoo.com");
                await SecureStorage.SetAsync("auth_token", token);
            }
            base.OnStart();
        }
    }
}
