using InventorySyetem1.Models;
using MySql.Data.MySqlClient;

namespace InventorySyetem1.Repositories;

public class MySqlSupplierRepository : ISupplierRepository
{
    
    private readonly string _connectionString;
    //constructor
    public MySqlSupplierRepository(string connectionString)
    {
        _connectionString = connectionString;
        CreateSupplierTable();
    }

    
    public void CreateSupplierTable()
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            try
            {
                connection.Open();
                string createTableSql = @"
                 create table if not exists suppliers(
                     id int primary key auto_increment,
                     name varchar(100) not null,
                     address varchar(100) not null,
                     phone varchar(100) not null,
                     email varchar(100) not null
                 );";
                using (MySqlCommand cmd = new MySqlCommand(createTableSql, connection))
                {
                    cmd.ExecuteNonQuery();
                }
                Console.WriteLine("[Supplier]MySql初始化成功或已存在");
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"[Supplier]初始化MySql失敗：{e.Message}");
            }
        }
    }

    public void AddSupplier(Supplier supplier)
    {
        throw new NotImplementedException();
    }

    public List<Supplier> GetAllSuppliers()
    {
        throw new NotImplementedException();
    }

    public Supplier GetSupplierById(int id)
    {
        throw new NotImplementedException();
    }

    public void UpdateSupplier(Supplier product)
    {
        throw new NotImplementedException();
    }

    public void DeleteSupplier(Supplier product)
    {
        throw new NotImplementedException();
    }

    public void ExistSupplier(int id)
    {
        throw new NotImplementedException();
    }
}