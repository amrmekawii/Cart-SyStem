namespace Cart_System.BL;

public class UserOrderDetailsDto
{
    public IEnumerable<UserOrderProductsDetailsDto>? OrderProducts { get; set; } = null;
    public bool IsOrderDelieverd { get; set; } = false;
}

public class UserOrderProductsDetailsDto
{
    public int product_Id { get; set; }
    public string title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int Quantity { get; set; } = 0;
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public decimal PriceAfter => Math.Round(Price - (Price * Discount / 100), 0);
    public decimal ProductPriceAtThisTime { get; set; }
    public bool IsReviewed { get; set; } = false;

}

