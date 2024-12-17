using CoffeeShopClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Text.Json;

namespace CoffeeShop.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService(string baseAddress)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseAddress) };
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var response = await _httpClient.GetAsync("User");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<User>>();
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"User/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<User>();
        }

        public async Task CreateUserAsync(User user)
        {
            string json = JsonSerializer.Serialize(user);
            using var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("User", httpContent);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateUserAsync(string id, User user)
        {
            var response = await _httpClient.PutAsJsonAsync($"User/{id}", user);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteUserAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"User/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            var AllUsers = await GetAllUsersAsync();
            var userCount = AllUsers.Where(x => x.Email == email).Count();
            return userCount > 0;
        }
    }
}
  