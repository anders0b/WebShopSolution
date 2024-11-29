using WebShop.Repository.Models;

namespace Repository.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public int Customer { get; set; } = new(); // Navigation property
    public List<OrderItem> OrderItems { get; set; } = new(); // Navigation property
    public bool IsShipped { get; set; }
}
