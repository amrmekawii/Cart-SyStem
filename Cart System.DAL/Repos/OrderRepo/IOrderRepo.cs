
namespace Cart_System.DAL;
public interface IOrderRepo : IGenericRepo<Order>
{
    public int GetLastUserOrder(string userId);

    public ICollection<Order> GetOrdersWithData(int page, int countPerPage);
    Order GetOrderWithProducts(int OrderId);
    public IEnumerable<Order> GetOrdersFiltered(FilterOrderObject filterOrder);


}
