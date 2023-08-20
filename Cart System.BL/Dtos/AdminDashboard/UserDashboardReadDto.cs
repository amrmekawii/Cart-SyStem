using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart_System.BL;

public class UserDashboardReadDto
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNum { get; set; } = string.Empty;
    public string Image { get; set; }
    public string Role { get; set; } = string.Empty;
    public IEnumerable<UserDashboardOrderDto> Orders { get; set; } = new HashSet<UserDashboardOrderDto>();
}



public class UserDashboardOrderDto
{
    public int Id { get; set; }
    public string OrderStatus { get; set; }
    public DateOnly OrderDate { get; set; } =  DateOnly.FromDateTime(DateTime.Now);
    public DateOnly? DeliverdDate { get; set; } = null;

}

