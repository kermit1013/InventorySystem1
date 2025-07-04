using InventorySyetem1.Models;
using InventorySyetem1.Repositories;

namespace InventorySyetem1.Services;

public class InventoryService
{
    private readonly IProductRepository _productRepository;

    //透過建構子,注入介面
    public InventoryService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public List<Product> GetAllProducts()
    {
        try
        {
            // 呼叫介面 , 而非實作（DI）
            List<Product> products = _productRepository.GetAllProducts();
            if (!products.Any())
            {
                Console.WriteLine("No products found!");
            }
            return products;
        }
        catch (Exception e)
        {
            Console.WriteLine($"讀取產品列表失敗：{e.Message}");
            return new List<Product>();
        }
    }
}