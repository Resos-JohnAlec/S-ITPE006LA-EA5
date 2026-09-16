using ECommerceInventory.Models;
using ECommerceInventory.Repositories.Interfaces;

namespace ECommerceInventory.UnitOfWork.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Product> Products { get; }

    IRepository<Order> Orders { get; }

    IRepository<OrderItem> OrderItems { get; }

    Task<int> SaveAsync();
}
