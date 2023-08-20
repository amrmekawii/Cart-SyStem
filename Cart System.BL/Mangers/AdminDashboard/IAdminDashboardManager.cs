using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart_System.BL;

public interface IAdminDashboardManager
{
    bool DeleteUser(string userId);
    IEnumerable<UserDashboardReadDto> GetAllUsers();
    UserDashboardReadDto GetUserById(string userId);
}
