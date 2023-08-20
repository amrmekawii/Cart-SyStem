using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace Cart_System.DAL;
public class UserRepo : IUserRepo
{
    private readonly CartSystemContext _context;
    public UserRepo(CartSystemContext context)
    {
        _context = context;

    }
    public User? GetById(string id)
    {
        return _context.Set<User>().Find(id);      
    }
   

    public void Delete(User user)
    {
        _context.Set<User>().Remove(user);
    }


    public void Update(User user)
    {
        //
    }

    public int savechanges()
    {
        return _context.SaveChanges();
    }
    #region user order
    public IEnumerable<Order>? GetUserOrders(string id)
    {
        return _context.Orders
                .Include(x => x.OrdersProductDetails)
                    .ThenInclude(x => x.Product)
                        .ThenInclude(p => p.ProductImages)

                .Where(x => x.UserId == id);
                
           
    }

    public IEnumerable<OrderProductDetails> GetUsersOrderDetails(int id)
    {
        return _context.OrderProductDetails
            .Include(x => x.Product)
                .ThenInclude(p => p.ProductImages)
            .Include(x=>x.Order)
            .Where(x => x.OrderId == id);
    }


    #endregion
}