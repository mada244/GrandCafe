using CoffeeShop.Services;
using CoffeeShopClassLibrary.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CoffeeShop
{
    public partial class ProductDetailsPage : ContentPage, INotifyPropertyChanged
    {
        private Product _product;
        private UserService _userService;
        private ProductService _productService;
        private User _user;
        public ObservableCollection<Product> ShoppingCart { get; private set; }
        private bool _isAdmin;

        private ToolbarItem editToolbarItem;
        private ToolbarItem deleteToolbarItem;

        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                if (_isAdmin != value)
                {
                    _isAdmin = value;
                    OnPropertyChanged(nameof(IsAdmin));
                    UpdateToolbarItems();
                }
            }
        }

        public ProductDetailsPage(Product product)
        {
            InitializeComponent();
            _product = product;
            _userService = new UserService(App.BaseAddress);
            _productService = new ProductService(App.BaseAddress);
            BindingContext = this;

            InitializeToolbarItems();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CheckIfUserIsAdmin();
        }

        public Product Product
        {
            get => _product;
            private set
            {
                if (_product != value)
                {
                    _product = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Denumire));
                    OnPropertyChanged(nameof(Descriere));
                    OnPropertyChanged(nameof(Pret));
                    OnPropertyChanged(nameof(ImageUrl));
                }
            }
        }

        public string Denumire => Product?.Denumire;
        public string Descriere => Product?.Descriere;
        public string Pret => Product?.Pret;
        public string ImageUrl => Product?.ImageUrl;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void InitializeToolbarItems()
        {
            editToolbarItem = new ToolbarItem
            {
                Text = "Edit",
                IconImageSource = "edit_icon.png",
                Command = new Command(OnEditClicked)
            };

            deleteToolbarItem = new ToolbarItem
            {
                Text = "Delete",
                IconImageSource = "bin_icon.png",
                Command = new Command(OnDeleteClicked)
            };
        }

        private void UpdateToolbarItems()
        {
            if (IsAdmin)
            {
                if (!ToolbarItems.Contains(editToolbarItem))
                {
                    ToolbarItems.Add(editToolbarItem);
                }
                if (!ToolbarItems.Contains(deleteToolbarItem))
                {
                    ToolbarItems.Add(deleteToolbarItem);
                }
            }
            else
            {
                if (ToolbarItems.Contains(editToolbarItem))
                {
                    ToolbarItems.Remove(editToolbarItem);
                }
                if (ToolbarItems.Contains(deleteToolbarItem))
                {
                    ToolbarItems.Remove(deleteToolbarItem);
                }
            }
        }

        private async Task CheckIfUserIsAdmin()
        {
            var user = await AuthService.GetLoggedUser(_userService);
            if (user != null)
            {
                IsAdmin = string.Equals(user.Roles, "admin", StringComparison.OrdinalIgnoreCase);
            }
        }

        private void OnEditClicked()
        {
            // Handle the edit button click event here
            Navigation.PushAsync(new Pages.EditProductPage(_product));
        }

        private async void OnDeleteClicked()
        {
            await _productService.DeleteProductAsync(_product.ID);
            Application.Current.MainPage = new NavigationPage(new ProductsPage());
        }

        private void OnSizeClicked(object sender, EventArgs e)
        {
            ResetButtonBackgrounds(new[] { SizeSmall, SizeRegular, SizeLarge });
            HighlightSelectedButton(sender);
        }

        private void OnSugarClicked(object sender, EventArgs e)
        {
            ResetButtonBackgrounds(new[] { SugarNormal, SugarLess, SugarNo });
            HighlightSelectedButton(sender);
        }

        private void OnIceClicked(object sender, EventArgs e)
        {
            ResetButtonBackgrounds(new[] { IceNormal, IceLess, IceNo });
            HighlightSelectedButton(sender);
        }

        private void ResetButtonBackgrounds(Button[] buttons)
        {
            foreach (var button in buttons)
            {
                button.BackgroundColor = Colors.LightGray;
            }
        }

        private void HighlightSelectedButton(object sender)
        {
            var button = sender as Button;
            if (button != null)
            {
                button.BackgroundColor = Color.FromArgb("#C68017");
            }
        }

    private async void OnAddToOrderClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Pages.OrderPage());
    }
    private async void OnEditClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Pages.EditProductPage(_product));
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
            await Navigation.PushAsync(new Pages.ProfilePage(_user));
        }
    }
}
