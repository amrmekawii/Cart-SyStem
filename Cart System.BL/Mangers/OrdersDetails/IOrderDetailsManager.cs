

namespace Cart_System.BL;

public interface IOrderDetailsManager
{
    public IEnumerable<OrderProductDetailsDto> GetTopProducts();

}
