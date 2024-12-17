using CoffeeShopClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Services
{
    public class AuthService
    {
        public static string Username { get; set; } = null;

        public static async Task<bool> SignInAsync(string email, string password, bool rememberMe, UserService _userService)
        {
            var users = await _userService.GetAllUsersAsync();
            var FindUser = users.Where(x => x.Email == email && x.Password == password).FirstOrDefault();

            if (FindUser == null)
            {
                return false;
            }
            else
            {
                try
                {
                    var token = GenerateSimpleToken(email);
                    await SecureStorage.SetAsync("auth_token", token);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error saving token: {ex.Message}");
                }
                return true;
            }
        }

        public static string GenerateSimpleToken(string username)
        {
            var token = $"{username}:{DateTime.UtcNow.Ticks}";
            var encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
            return encodedToken;
        }

        public static async Task<bool> ValidateToken()
        {
            try
            {
                Username = null;
                var token = await SecureStorage.GetAsync("auth_token");
                var decodedBytes = Convert.FromBase64String(token);
                var decodedToken = Encoding.UTF8.GetString(decodedBytes);
                var parts = decodedToken.Split(':');
                var username = parts[0];
                var ticks = long.Parse(parts[1]);
                var tokenDate = new DateTime(ticks);
                if ((DateTime.UtcNow - tokenDate).TotalMinutes <= 30)
                {
                    Username = username;
                }
            }
            catch
            {
                return false;
            }
            return true; // user is logged
        }

        public static async Task<string?> GetLoggedUserEmail()
        {
            await ValidateToken();
            return Username;
        }

        public static async Task<User> GetLoggedUser(UserService userService)
        {
            var email = await GetLoggedUserEmail();
            if (string.IsNullOrEmpty(email))
                return null;
            var users = await userService.GetAllUsersAsync();
            var user = users.FirstOrDefault(users => users.Email == email);

            return user;
        }
    }
}
