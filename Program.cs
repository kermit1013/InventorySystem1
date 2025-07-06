// See https://aka.ms/new-console-template for more information

using InventorySyetem1.Models;
using InventorySyetem1.Repositories;
using InventorySyetem1.Services;
using InventorySyetem1.Utils;

//Server: mysql所在伺服器位置（localhost or ip xxx.xxx.xxx.xxx）
//Port: mysql端口（預設3306）
//Database: inventory_db(CREATE DATABASE inventory_db;)
//uid: mysql使用者名稱
//pwd: mysql使用者密碼
const string MYSQL_CONNETION_STRING = "Server=localhost;Port=3306;Database=inventory_db;uid=root;pwd=kermitpassword;";

//小明注入 打掃阿姨1 (mysql實作)
MySqlProductRepository productRepo = new MySqlProductRepository(MYSQL_CONNETION_STRING);
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
            case "2": SearchProduct();
                break;
            case "3": AddProduct();
                break;
            case "4": UpdateProduct();
                break;
            case "0": 
                Console.WriteLine("Goodbye !");
                return;
        }
    }
}

void DisplayMenu()
{
    Console.WriteLine("Welcome to the inventory system!");
    Console.WriteLine("What would you like to do?");
    Console.WriteLine("1. 查看所有產品");
    Console.WriteLine("2. 查詢產品");
    Console.WriteLine("3. 新增產品");
    Console.WriteLine("4. 更新產品");
    Console.WriteLine("0. 離開");
}

void GetAllProducts()
{
    Console.WriteLine("\n--- 所有產品列表 ---");
    var products =  inventoryService.GetAllProducts();
    // var products =  productRepository.GetAllProducts();
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

void SearchProduct()
{
    Console.WriteLine("輸入欲查詢的產品編號");
    int input = ReadIntLine(1);
    var product = productRepo.GetProductById(input);
    // string input = Console.ReadLine();
    // var product = productRepository.GetProductById(ReadInt(input));
    if (product != null)
    {
        Console.WriteLine("-----------------------------------------------");
        Console.WriteLine("ID | Name | Price | Quantity | Status");
        Console.WriteLine("-----------------------------------------------");
        Console.WriteLine(product);
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
    productRepo.AddProduct(name, price, quantity);
    smsService.NotifyUser("john", "新增產品成功");
}


void UpdateProduct()
{
    
    throw new NotImplementedException();
}

int ReadInt(string input)
{
    try
    {
        return Convert.ToInt32(input);
    }
    catch (FormatException e)
    {
        Console.WriteLine("請輸入有效數字。");
        return 0;
    }
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
        else
        {
            Console.WriteLine("請輸入有效數字。");
        }
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
        else
        {
            Console.WriteLine("請輸入有效數字。");
        }
    }
}

void OOP()
{
    //實例化(new)
    Cat moew = new Cat();
    Dog bob = new Dog();
    //一隻狗bob 一隻貓meow
    
    
    Animal milk = new Cat();
    Animal john = new Dog();
    //兩隻動物 john(dog) milk(cat)
}