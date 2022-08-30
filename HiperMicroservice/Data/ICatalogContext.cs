using HiperMicroservice.Model;
using MongoDB.Driver;

namespace HiperMicroservice.Data
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
