using InventorySyetem1.Models;

namespace InventorySyetem1.Repositories;

public interface IProductRepository
{
    List<Product> GetAllProducts();
    Product GetProductById(int id);
    void AddProduct(Product product);
    //合約內容如下：
    //打掃廚房
    void CleanKitchen();
    //打掃浴室
    void CleanBathroom();
    //拖地
    void CleanFloor();
    //洗衣服
    //共計三小時
    int GetNextProductId();
}