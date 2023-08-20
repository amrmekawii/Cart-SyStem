
namespace Cart_System.BL;

public class OrderDetailsDto
{
    public int Id { get; set; }
    public string OrderStatus { get; set; }
    public DateOnly OrderDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly? DeliverdDate { get; set; } = null;
    public string UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FormattedCreationDate => OrderDate.ToString("dd-MM-yyyy");
    public string FormattedDeliveredDate => DeliverdDate.HasValue ? DeliverdDate.Value.ToString("dd-MM-yyyy") : "";
    public string Phone { get; set; } = string.Empty;
    public int ProductCount { get; set; }
    public decimal TotalPrice { get; set; }
    public IEnumerable<ProductsInOrder> ProductsInOrder { get; set; } = new HashSet<ProductsInOrder>();
}

public class ProductsInOrder
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; }
    public string ProductImage { get; set; } = string.Empty;
    public decimal Discount { get; set; }
}