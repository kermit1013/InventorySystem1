using InventorySyetem1.Models;
using InventorySyetem1.Repositories;
using InventorySyetem1.Utils;

namespace InventorySyetem1.Services;

public class InventoryService
{
    // 1. 資料庫相關
    private readonly IProductRepository _productRepository;

    //透過建構子,注入介面
    // InventoryService（小明） 需要IProductRepository 合約
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