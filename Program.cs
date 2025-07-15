// See https://aka.ms/new-console-template for more information

using InventorySyetem1.Models;
using InventorySyetem1.Repositories;
using InventorySyetem1.Services;
using InventorySyetem1.Utils;
using ZstdSharp.Unsafe;

//Server: mysql所在伺服器位置（localhost or ip xxx.xxx.xxx.xxx）
//Port: mysql端口（預設3306）
//Database: inventory_db(CREATE DATABASE inventory_db;)
//uid: mysql使用者名稱
//pwd: mysql使用者密碼
const string MYSQL_CONNETION_STRING = "Server=localhost;Port=3306;Database=inventory_db;uid=root;pwd=kermitpassword;";
string connectionString = "";
string configFile = "appsettings.ini";

if (File.Exists(configFile))
{
    Console.WriteLine($"Reading {configFile} file");
    try
    {
        Dictionary<string, Dictionary<string, string>> config = ReadFile(configFile);
        
        if (config.ContainsKey("Database"))
        {
            var dbConfig = config["Database"];
            connectionString=$"Server={dbConfig["Server"]};Port={dbConfig["Port"]};Database={dbConfig["Database"]};uid={dbConfig["Uid"]};pwd={dbConfig["Pwd"]};";
            Console.WriteLine($"讀取資料庫連接字串成功！:{connectionString}");
        }
    }
    catch (Exception e)
    {
        Console.WriteLine($"錯誤：讀取配置檔案失敗：{e}");
        // throw;
        connectionString = MYSQL_CONNETION_STRING;
    }
}
else
{
    Console.WriteLine($"錯誤：配置檔案 {configFile} 不存在");
    connectionString = MYSQL_CONNETION_STRING;
}



//小明注入 打掃阿姨1 (mysql實作)
MySqlProductRepository productRepo = new MySqlProductRepository(connectionString);
InventoryService inventoryService = new InventoryService(productRepo);

// 通知功能相關
// 使用EmailNotifier
INotifier emailNotifier = new EmailNotifier();
NotificationService emailService = new NotificationService(emailNotifier);
// 使用SmsNotifier
INotifier smsNotifier = new SmsNotifier();
NotificationService smsService = new NotificationService(smsNotifier);

//小明注入 打掃阿姨2 (mongo實作) 
// MongoDBProductRepository mongoRepo = new MongoDBProductRepository();
// InventoryService inventoryService = new InventoryService(mongoRepo);
// inventoryService.GetAllProducts();

RunMenu();
//test
void RunMenu()
{
    while (true)
    {
        DisplayMenu();
        string input = Console.ReadLine();
        switch (input) 
        {
            case "1": GetAllProducts();
                break;
            case "2": SearchProductById();
                break;
            case "3": AddProduct();
                break;
            case "4": UpdateProduct();
                break;
            case "5": SearchProduct();
                break;
            case "6": SearchLowProduct();
                break;
            case "7": SearchOutOfStockProduct();
                break;
            case "8": AdjustProductQuantity();
                break;
            case "0": 
                Console.WriteLine("Goodbye !");
                return;
        }
    }
}

void DisplayMenu()
{
    Console.WriteLine("歡迎使用InventorySyetem！");
    Console.WriteLine("What would you like to do?");
    Console.WriteLine("1. 查看所有產品");
    Console.WriteLine("2. 查詢產品ID");
    Console.WriteLine("3. 新增產品");
    Console.WriteLine("4. 更新產品");
    Console.WriteLine("5. 查詢產品");
    Console.WriteLine("6. 查詢庫存偏低");
    Console.WriteLine("7. 查詢已缺貨產品");
    Console.WriteLine("8. 調整產品庫存(出庫/入庫)");
    Console.WriteLine("0. 離開");
}

void GetAllProducts()
{
    Console.WriteLine("\n--- 所有產品列表 ---");
    var products =  inventoryService.GetAllProducts();
    Console.WriteLine("-----------------------------------------------");
    Console.WriteLine("ID | Name | Price | Quantity | Status");
    Console.WriteLine("-----------------------------------------------");
    foreach (var product in products)
    {
        Console.WriteLine(product);
    }
    Console.WriteLine("-----------------------------------------------");
  
    emailService.NotifyUser("user", "查詢已完成");
}

void SearchProductById()
{
    Console.WriteLine("輸入欲查詢的產品編號");
    int input = ReadIntLine(1);
    // var product = productRepo.GetProductById(input);
    OperationResult<Product> result = inventoryService.GetProductById(input);
    if (result != null)
    {
        Console.WriteLine("-----------------------------------------------");
        Console.WriteLine("ID | Name | Price | Quantity | Status");
        Console.WriteLine("-----------------------------------------------");
        Console.WriteLine(result.Data);
        Console.WriteLine("-----------------------------------------------");
    }
}

void SearchProduct()
{
   Console.WriteLine("查詢產品名稱關鍵字：");
   string input = Console.ReadLine();
   OperationResult<List<Product>> results = inventoryService.SearchProduct(input);
   if (!results.Success)
   {
       Console.WriteLine(results.Message);
   }
   

   if (results.Data.Any())
   {
       Console.WriteLine($"-------------查詢條件為：（{input}）------------");
       Console.WriteLine("-----------------------------------------------");
       Console.WriteLine("ID | Name | Price | Quantity | Status");
       Console.WriteLine("-----------------------------------------------");
       foreach (var product in results.Data)
       {
           Console.WriteLine(product);
       }
       Console.WriteLine("-----------------------------------------------");
   }
}

void SearchLowProduct()
{
    List<Product> products = inventoryService.SearchLowProduct();
    if (products.Any())
    {
        Console.WriteLine($"-------------查詢條件為：（低庫存）------------");
        Console.WriteLine("-----------------------------------------------");
        Console.WriteLine("ID | Name | Price | Quantity | Status");
        Console.WriteLine("-----------------------------------------------");
        foreach (var product in products)
        {
            Console.WriteLine(product);
        }
        Console.WriteLine("-----------------------------------------------");
    }
}

void SearchOutOfStockProduct()
{
    List<Product> products = inventoryService.SearchOutOfStockProduct();
    if (products.Any())
    {
        Console.WriteLine($"-------------查詢條件為：（缺貨）------------");
        Console.WriteLine("-----------------------------------------------");
        Console.WriteLine("ID | Name | Price | Quantity | Status");
        Console.WriteLine("-----------------------------------------------");
        foreach (var product in products)
        {
            Console.WriteLine(product);
        }
        Console.WriteLine("-----------------------------------------------");
    }
}

void AddProduct()
{
    Console.WriteLine("輸入產品名稱：");
    string name = Console.ReadLine();
    Console.WriteLine("輸入產品價格：");
    decimal price = ReadDecimalLine();
    Console.WriteLine("輸入產品數量：");
    int quantity = ReadIntLine();
    inventoryService.AddProduct(name, price, quantity);
    smsService.NotifyUser("john", "新增產品成功");
}


void UpdateProduct()
{
    Console.WriteLine("請輸入要更新的產品id");
    int id = ReadIntLine();
    //找到對應產品
    OperationResult<Product> product = inventoryService.GetProductById(id);
    if (!product.Success)// 若為錯誤
    {
        Console.WriteLine(product.Message);
        return;
    }
    Console.WriteLine("輸入產品名稱：");
    string name = Console.ReadLine();
    Console.WriteLine("輸入產品價格：");
    decimal price = ReadDecimalLine();
    Console.WriteLine("輸入產品數量：");
    int quantity = ReadIntLine();
    
    inventoryService.UpdateProduct(product.Data, name, price, quantity);
}

void AdjustProductQuantity()
{
    Console.WriteLine("請輸入要調整庫存的產品id");
    int id = ReadIntLine();
    //找到對應產品
    var product = inventoryService.GetProductById(id);
    if (!product.Success)// 若為錯誤
    {
        Console.WriteLine(product.Message);
        return;
    }
    Console.WriteLine("輸入調整數量（正數入庫/負數出庫）：");
    int quantity = ReadIntLine();
    inventoryService.AdjustProductQuantity(product.Data, quantity);
}

int ReadIntLine(int defaultValue = 0)
{
    while (true)
    {
        
        String input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input) && defaultValue != 0)
        {
            return defaultValue;
        }
        //string parsing to int 
        if (int.TryParse(input,out int value))
        {
            return value;
        }
        Console.WriteLine("請輸入有效數字。");
    }
}

decimal ReadDecimalLine(decimal defaultValue = 0.0m)
{
    while (true)
    {
        
        String input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input) && defaultValue != 0.0m)
        {
            return defaultValue;
        }
        //string parsing to int 
        if (decimal.TryParse(input,out decimal value))
        {
            return value;
        }
        Console.WriteLine("請輸入有效數字。");
    }
}


Dictionary<string, Dictionary<string, string>> ReadFile(string s)
{
    var config = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
    string currentSection = "";

    foreach (string line in File.ReadLines(s))
    {
        string trimmedLine = line.Trim();
        if (trimmedLine.StartsWith("#") || string.IsNullOrWhiteSpace(trimmedLine))
        {
            continue; // 跳過註釋和空行
        }

        if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
        {
            currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2);
            config[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
        else if (currentSection != "" && trimmedLine.Contains("="))
        {
            int equalsIndex = trimmedLine.IndexOf('=');
            string key = trimmedLine.Substring(0, equalsIndex).Trim();
            string value = trimmedLine.Substring(equalsIndex + 1).Trim();
            config[currentSection][key] = value;
        }
    }
    return config;
}