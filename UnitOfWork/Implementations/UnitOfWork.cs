using ECommerceInventory.Data;
using ECommerceInventory.Models;
using ECommerceInventory.Repositories.Implementations;
using ECommerceInventory.Repositories.Interfaces;
using ECommerceInventory.UnitOfWork.Interfaces;

namespace ECommerceInventory.UnitOfWork.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Products = new Repository<Product>(context);
        Orders = new Repository<Order>(context);
        OrderItems = new Repository<OrderItem>(context);
    }

    public IRepository<Product> Products { get; }

    public IRepository<Order> Orders { get; }

    public IRepository<OrderItem> OrderItems { get; }

    public Task<int> SaveAsync()
    {
        return _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
