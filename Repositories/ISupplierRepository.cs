using InventorySyetem1.Models;

namespace InventorySyetem1.Repositories;

public interface ISupplierRepository
{
    void CreateSupplierTable();
    void AddSupplier(Supplier supplier);
    List<Supplier> GetAllSuppliers();
    Supplier GetSupplierById(int id);
    void UpdateSupplier(Supplier product);
    void DeleteSupplier(Supplier product);
    void ExistSupplier(int id);
}