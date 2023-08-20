using Microsoft.EntityFrameworkCore;

namespace Cart_System.DAL;
public class OrderRepo : GenericRepo<Order>, IOrderRepo
{
    private readonly CartSystemContext _context;
    public OrderRepo(CartSystemContext context) : base(context)
    {
        _context = context;
    }
    // select top(1) id from orders where userid=5 order by desc
    public int GetLastUserOrder(string userId)
    {
        return _context.Set<Order>()
                        .Where(o => o.UserId == userId)
                        .OrderByDescending(o => o.OrderDate)
                        .Last().Id;

    }

    public  IEnumerable<Order> GetOrdersFiltered(FilterOrderObject filterOrder)
    {
        var Orders = _context.Set<Order>()
                .Include(o => o.User)
                .Include(o => o.OrdersProductDetails)
                .ThenInclude(op => op.Product)
                .ThenInclude(p => p.ProductImages)
                .OrderBy(o => o.OrderDate)
                .AsQueryable();
            

        if (filterOrder.UseiD != null)
        {
            Orders= Orders.Where(q => q.UserId == filterOrder.UseiD);    
        }
        if ( filterOrder.OrderState != null)
        {
            Orders= Orders.Where(q => q.OrderStatus == filterOrder.OrderState);
        }
        return Orders;

    }

    public ICollection<Order> GetOrdersWithData(int page, int countPerPage)
    {
        return _context.Set<Order>()
                .Include(o => o.User)
                .Include(o => o.OrdersProductDetails)
                    .ThenInclude(op => op.Product)
                        .ThenInclude(p=>p.ProductImages)
                        .OrderByDescending(o => o.OrderDate)
                        .Skip((page - 1) * countPerPage)
                        .Take(countPerPage)
                        .ToList();
    }

    #region Get Order With Product

    public Order GetOrderWithProducts(int OrderId)
    {
        return _context.Set<Order>()
                .Include(o => o.User)
                .Include(o => o.OrdersProductDetails)
                    .ThenInclude(op => op.Product)
                        .ThenInclude(p => p.ProductImages)

                .First(o => o.Id == OrderId);
    }

    #endregion
}

