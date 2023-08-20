using System.ComponentModel.DataAnnotations.Schema;

namespace Cart_System.DAL;
public class Order
{
    public int Id { get; set; }
    public OrderStatus OrderStatus  { get; set; }
    public DateOnly OrderDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly? DeliverdDate { get; set; } = null;
    public string? UserId { get; set; }=string.Empty; 
    public User? User { get; set; } 
    public IEnumerable<OrderProductDetails> OrdersProductDetails { get; set; } = new HashSet<OrderProductDetails>();

}

public enum OrderStatus
{
    Pending,
    Delivering,
    Fullfilled
}