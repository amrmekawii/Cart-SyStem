using Cart_System.DAL;

namespace Cart_System.DAL;
public class UnitOfWork : IUnitOfWork
{
    private readonly CartSystemContext context;

    public IUserRepo UserRepo { get; }
    public IProductRepo ProductRepo { get; }
    public ICategoryRepo CategoryRepo { get; }
    public IOrderRepo OrderRepo { get; }
    public IOrdersDetailsRepo OrdersDetailsRepo { get; }
    public IUserProductsCartRepo UserProdutsCartRepo { get; }
    public IDashboardUserRepo DashboardUserRepo { get; }


    public UnitOfWork(CartSystemContext context, IUserRepo userRepo, IProductRepo productRepo, ICategoryRepo categoryRepo, IOrderRepo orderRepo, IOrdersDetailsRepo ordersDetailsRepo, IUserProductsCartRepo userProdutsCartRepo, IDashboardUserRepo dashboardUserRepo)
    {
        this.context = context;
        UserRepo = userRepo;
        ProductRepo = productRepo;
        CategoryRepo = categoryRepo;
        OrderRepo = orderRepo;
        OrdersDetailsRepo = ordersDetailsRepo;
        UserProdutsCartRepo = userProdutsCartRepo;
        DashboardUserRepo = dashboardUserRepo;
    }

    public int Savechanges()
    {
        return context.SaveChanges();
    }
}
