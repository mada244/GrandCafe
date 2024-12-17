using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Net;  // Required for Android's Uri class

namespace CoffeeShop
{
    [Activity(Label = "CoffeeShop", Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            HandleIntent(intent);
        }

        private void HandleIntent(Intent intent)
        {
            var androidUri = intent.Data;
            if (androidUri != null)
            {
                // Convert Android.Net.Uri to System.Uri
                System.Uri uri = new System.Uri(androidUri.ToString());

                // Use System.Uri methods to parse the URL
                var queryParameters = System.Web.HttpUtility.ParseQueryString(uri.Query);

                var page = queryParameters["page"].ToString();
                if (uri.Host == "open" && page == "HomePage")
                {
                    // Navigate to the Home Page in your app
                    // Example: await Shell.Current.GoToAsync("//HomePage");
                }
            }
        }
    }
}
