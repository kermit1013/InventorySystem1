using InventorySyetem1.Models;

namespace InventorySyetem1.Repositories;

public interface IProductRepository
{
    List<Product> GetAllProducts();
    List<Product> GetLowProducts();
    Product GetProductById(int id);
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    int GetNextProductId();
    
    List<Product> GetAllOutOfStockProducts();
    
    
    //合約內容如下：
    //打掃廚房
    void CleanKitchen();
    //打掃浴室
    void CleanBathroom();
    //拖地
    void CleanFloor();

}