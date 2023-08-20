using Microsoft.EntityFrameworkCore;

namespace Cart_System.DAL;
public interface IOrdersDetailsRepo : IGenericRepo<OrderProductDetails>
{
    void AddRange(IEnumerable<OrderProductDetails> orderProducts);
    public IEnumerable<OrderProductDetails> GetTopProducts();
    public OrderProductDetails? GetByCompositeId(int ProductId, int orderId);
    

}
