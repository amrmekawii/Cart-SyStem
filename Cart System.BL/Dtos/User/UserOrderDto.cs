
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart_System.BL;

public class UserOrderDto
{
    public int Id { get; set; }
    public string OrderStatus { get; set; }
    public DateOnly? DeliverdDate { get; set; } = null;
    public IEnumerable<UserProductDto> Products { get; set; } = new HashSet<UserProductDto>();

}
