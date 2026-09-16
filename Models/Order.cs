namespace ECommerceInventory.Models;

public class Order
{
    public int Id { get; set; }

    public string CustomerEmail { get; set; } = string.Empty;

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
