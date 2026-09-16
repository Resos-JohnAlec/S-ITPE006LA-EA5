using ECommerceInventory.Models;
using ECommerceInventory.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceInventory.Controllers;

public class OrdersController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public OrdersController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index()
    {
        var orders = await _unitOfWork.Orders.GetAllAsync();
        return View(orders);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var order = await _unitOfWork.Orders.GetByIdAsync(id.Value);
        if (order is null)
        {
            return NotFound();
        }

        return View(order);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Order { OrderDate = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Order order)
    {
        if (!ModelState.IsValid)
        {
            return View(order);
        }

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.SaveAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var order = await _unitOfWork.Orders.GetByIdAsync(id.Value);
        if (order is null)
        {
            return NotFound();
        }

        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Order order)
    {
        if (id != order.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(order);
        }

        _unitOfWork.Orders.Update(order);
        await _unitOfWork.SaveAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var order = await _unitOfWork.Orders.GetByIdAsync(id.Value);
        if (order is null)
        {
            return NotFound();
        }

        return View(order);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);
        if (order is null)
        {
            return NotFound();
        }

        _unitOfWork.Orders.Delete(order);
        await _unitOfWork.SaveAsync();
        return RedirectToAction(nameof(Index));
    }
}
