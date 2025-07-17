using InventorySyetem1.Models;
using InventorySyetem1.Repositories;
using InventorySyetem1.Utils;

namespace InventorySyetem1.Services;

public class SupplierService
{
    private readonly ISupplierRepository _supplierRepo;

    public SupplierService(ISupplierRepository supplierRepo)
    {
        _supplierRepo = supplierRepo;
    }

    public void AddSupplier(String name, String phone, String address, String email)
    {
        Supplier supplier = new Supplier(name, phone, address, email);
        _supplierRepo.AddSupplier(supplier);
    }
}