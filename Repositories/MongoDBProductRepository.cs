using InventorySyetem1.Models;

namespace InventorySyetem1.Repositories;

public class MongoDBProductRepository  : IProductRepository
{
    public void UpdateProduct(Product product)
    {
        throw new NotImplementedException();
    }

    public List<Product> GetLowProducts()
    {
        throw new NotImplementedException();
    }

    public List<Product> GetAllOutOfStockProducts()
    {
        throw new NotImplementedException();
    }

    public MongoDBProductRepository()
    {
    }

    public int GetNextProductId()
    {
        throw new NotImplementedException();
    }

    public List<Product> GetAllProducts()
    {
        throw new NotImplementedException();
    }

    public Product GetProductById(int id)
    {
        throw new NotImplementedException();
    }

    public void AddProduct(Product product)
    {
        throw new NotImplementedException();
    }

    public void CleanKitchen()
    {
        throw new NotImplementedException();
    }

    public void CleanBathroom()
    {
        throw new NotImplementedException();
    }

    public void CleanFloor()
    {
        throw new NotImplementedException();
    }
}