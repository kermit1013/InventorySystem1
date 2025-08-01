using System.Collections;
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

    public OperationResult<Product> GetProductById(int id)
    {
        try
        {
            // 呼叫介面 , 而非實作（DI）
            Product product = _productRepository.GetProductById(id);
            if (product == null)
            {
                return OperationResult<Product>.ErrorResult("查無該產品");
            }
            return OperationResult<Product>.SuccessResult("操作成功", product); ;
            
        }
        catch (Exception e)
        {
            return OperationResult<Product>.ErrorResult($"讀取產品列表失敗：{e.Message}"); ;
        }
    }

    public void AddProduct(string? name, decimal price, int quantity)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("產品名稱不能為空。");
            }
            //價格必須大於零
            if (price <= 0)
            {
                throw new ArgumentException("價格必須大於零。");
            }
            //數量不能小於零
            if (quantity < 0)
            {
                throw new ArgumentException("數量不能小於零。");
            }
            // 嘗試透過repo 新增產品
            var product = new Product(_productRepository.GetNextProductId(), name, price, quantity);
            _productRepository.AddProduct(product);
        }
        catch (Exception e)
        {
            Console.WriteLine($"\n 錯誤：{e.Message}");
        }
    }

    public void UpdateProduct(Product product, string? name, decimal price, int quantity)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("產品名稱不能為空。");
            }
            //價格必須大於零
            if (price <= 0)
            {
                throw new ArgumentException("價格必須大於零。");
            }
            //數量不能小於零
            if (quantity < 0)
            {
                throw new ArgumentException("數量不能小於零。");
            }
            //執行更新(覆蓋(賦值) origin product 的屬性)
            product.Name = name;
            product.Price = price;
            product.Quantity = quantity;
            product.UpdateStatus();
            //呼叫repo
            _productRepository.UpdateProduct(product);
            Console.WriteLine($"產品Id: {product.Id} 已更新");
        }
        catch (Exception e)
        {
            Console.WriteLine($"錯誤：更新產品失敗：{e.Message}");
        }
    }

    public OperationResult<List<Product>> SearchProduct(string? input)
    {
        try
        {
            List<Product> products = _productRepository.GetAllProducts();
            if (string.IsNullOrWhiteSpace(input))
            {
                return OperationResult<List<Product>>.ErrorResult("請勿使用空字串");
            }
            
            var results = products
                .Where(product => product.Name.ToLower().Contains(input.ToLower()))
                .OrderBy(product => product.Name)
                .ToList();
        
            if (!results.Any())
            {
                OperationResult<List<Product>>.ErrorResult("No products found!");
            }
            return OperationResult<List<Product>>.SuccessResult("操作成功", results);
        }
        catch (Exception e)
        {
            return OperationResult<List<Product>>.ErrorResult($"讀取產品列表失敗：{e.Message}");
        }
    }

    public List<Product> SearchLowProduct()
    {
        try
        {
            //區別為資料數量先存進記憶體
            //1.
            List<Product> products = _productRepository.GetAllProducts();
            //2.
            //List<Product> lowProducts = _productRepository.GetLowProducts();
            //linq
            var results = products
                .Where(product => product.Quantity < 10 )
                .Where(product => product.Status == Product.ProductStatus.LowStock)
                .OrderBy(product => product.Name)
                .ToList();
            
            if (!results.Any())
            {
                Console.WriteLine("No products found!");
            }
            return results;
        }
        catch (Exception e)
        {
            Console.WriteLine($"讀取產品列表失敗：{e.Message}");
            return new List<Product>();
        }
    }

    public List<Product> SearchOutOfStockProduct()
    {
        try
        {
            List<Product> outOfStocks = _productRepository.GetAllOutOfStockProducts();
            
            if (!outOfStocks.Any())
            {
                Console.WriteLine("No products found!");
            }
            return outOfStocks;
        }
        catch (Exception e)
        {
            Console.WriteLine($"讀取產品列表失敗：{e.Message}");
            return new List<Product>();
        }
    }

    public void AdjustProductQuantity(Product product, int quantity)
    {
        int newQuantity = product.Quantity + quantity;
        if (newQuantity < 0)
        {
            Console.WriteLine($"庫存不足，嘗試操作 出庫 {quantity}。");
        } 

        //update product 
        product.Quantity = newQuantity;
        product.UpdateStatus();
        _productRepository.UpdateProduct(product);
        
        Console.WriteLine($"成功更新產品: {product.Name} ，當前庫存為 {newQuantity}");
    }
}