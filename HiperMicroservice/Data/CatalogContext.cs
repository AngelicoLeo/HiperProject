using HiperMicroservice.Model;
using MongoDB.Driver;

namespace HiperMicroservice.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>
                    ("DataBaseSettings:ConnectionString"));

            var dataBase = client.GetDatabase(configuration.GetValue<string>
                    ("DataBaseSettings:DataBaseName"));
            Products = dataBase.GetCollection<Product>(configuration.GetValue<string>
                ("DataBaseSettings:CollectionName"));

            CatalogContextSeed.SeedData(Products);
        }

        public IMongoCollection<Product> Products { get; }
    }
}
