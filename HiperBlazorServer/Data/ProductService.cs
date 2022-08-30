using HiperShared.Model;
using System.Text;
using System.Text.Json;

namespace HiperBlazorServer.Data
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<Product> CreateProduct(Product product)
        {
            try
            {
                var productJson = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/v1/Catalog/", productJson);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStreamAsync();
                    
                    return await JsonSerializer.DeserializeAsync<Product>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
                }
                var responseError = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseError);
                return null;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> DeleteProduct(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/v1/catalog/{id}");
                return response.IsSuccessStatusCode ? true : false;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public async Task<Product> GetProduct(string id)
        {
            var apiResponse = await _httpClient.GetStreamAsync($"api/v1/catalog/{id}");
            return await JsonSerializer.DeserializeAsync<Product>
                    (apiResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            var apiResponse = await _httpClient.GetStreamAsync($"api/v1/catalog/GetProductsByName/{name}");
            return await JsonSerializer.DeserializeAsync<IEnumerable<Product>>
                    (apiResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var apiResponse = await _httpClient.GetStreamAsync($"api/v1/catalog/");
            return await JsonSerializer.DeserializeAsync<IEnumerable<Product>>
                     (apiResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(string categoryName)
        {
            var apiResponse = await _httpClient.GetStreamAsync($"api/v1/catalog/GetProductsByCategory/{categoryName}");
            return await JsonSerializer.DeserializeAsync<IEnumerable<Product>>
                     (apiResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            try
            {
                var itemJson = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"api/v1/Catalog/{product.Id}", itemJson);
                return response.IsSuccessStatusCode ? true : false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
