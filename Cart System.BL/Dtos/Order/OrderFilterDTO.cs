
using Cart_System.DAL;

namespace Cart_System.BL
{
    public class OrderFilterDTO
    {
        public string? UseiD { get; set; }
        public OrderStatus? OrderState { get; set; } 
    }
}
