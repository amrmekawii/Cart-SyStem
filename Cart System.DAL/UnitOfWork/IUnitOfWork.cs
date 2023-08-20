using Cart_System.DAL;

namespace Cart_System.DAL;
public interface IUnitOfWork
{
    public IUserRepo UserRepo { get; }
    public IProductRepo ProductRepo { get; }
    public ICategoryRepo CategoryRepo { get; }
    public IOrderRepo OrderRepo { get; }
    public IOrdersDetailsRepo OrdersDetailsRepo { get; }
    public IUserProductsCartRepo UserProdutsCartRepo { get; }
    public IDashboardUserRepo DashboardUserRepo { get; }





    int Savechanges();
}
