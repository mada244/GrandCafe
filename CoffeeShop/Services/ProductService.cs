using CoffeeShopClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoffeeShop.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(string baseAddress)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress),
            };
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var response = await _httpClient.GetAsync("Product");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"Product/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Product>();
        }

        public async Task CreateProductAsync(Product product)
        {
            string json = JsonSerializer.Serialize(product);
            using var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Product", httpContent);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateProductAsync(string id, Product product)
        {
            var response = await _httpClient.PutAsJsonAsync($"Product/{id}", product);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProductAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"Product/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> ProductExistsAsync(string denumire)
        {
            var allProducts = await GetAllProductsAsync();
            var productCount = allProducts.Where(x => x.Denumire == denumire).Count();
            return productCount > 0;
        }

        public async Task<string> CreateCheckoutSession(List<Product> products)
        {
            var json = JsonSerializer.Serialize(products);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Product/create-checkout-session", content);
            var sessionUrl = await response.Content.ReadAsStringAsync();
            return sessionUrl.Trim('\"');
        }
        //public async Task<List<Product>> GetFavoriteProductsAsync(string userId)
        //{
        //    var allProducts = await GetAllProductsAsync();
        //    var favoriteProducts = allProducts.Where(p => IsFavoriteForUser(p, userId)).ToList();
        //    return favoriteProducts;
        //}
    }
}
