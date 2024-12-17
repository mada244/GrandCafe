using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using System.Text.Json;
using CoffeeShopClassLibrary.Models;

namespace CoffeeShop.Services
{
    public class MongoDBService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public MongoDBService()
        {

        }

        public static async Task<List<User>> GetUsersAsync()
        {
            string url = "http://10.0.2.2:5081/Mongo/Users";
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<User>>(jsonResponse);
                }
                else
                {
                    Console.WriteLine($"Failed to fetch data: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception when calling API: {ex.Message}");
            }
            return new List<User>();
        }

        //public async Task<User> GetUserByIdAsync(string id)
        //{
        //    return await _users.Find<User>(user => user.ID == id).FirstOrDefaultAsync();
        //}

        //public async Task CreateUserAsync(User user)
        //{
        //    await _users.InsertOneAsync(user);
        //}

        //public async Task UpdateUserAsync(string id, User userIn)
        //{
        //    await _users.ReplaceOneAsync(user => user.ID == id, userIn);
        //}

        //public async Task DeleteUserAsync(string id)
        //{
        //    await _users.DeleteOneAsync(user => user.ID == id);
        //}
    }
}