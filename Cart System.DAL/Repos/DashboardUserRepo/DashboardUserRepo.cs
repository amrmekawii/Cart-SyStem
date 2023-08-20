using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart_System.DAL;

public class DashboardUserRepo : GenericRepo<User>, IDashboardUserRepo
{
    private readonly CartSystemContext _context;

    public DashboardUserRepo(CartSystemContext context) : base(context)
    {
        _context = context;
    }

    public User GetByUserId(string userId)
    {
        return _context.Set<User>()
            .Include(u => u.Orders)
            .First(u=>u.Id == userId);
    }
}
