using HiperMicroservice.Model;
using MongoDB.Driver;

namespace HiperMicroservice.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool existProduct = productCollection.Find(p => true).Any();
            if (!existProduct)
            {
                productCollection.InsertManyAsync(GetMyProducts());
            }
        }

        private static IEnumerable<Product> GetMyProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id = "",
                    Name = "Processador AMD 5500",
                    Category = "Processer",
                    Image="product-1-1.jpg",
                    Price= 970.00M 
                }
            };
        }
    }
}
