using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart_System.BL;

public interface IOrdersManager
{
    public void AddNewOrder(string userId);

    IEnumerable<OrderReadDto> GetAllOrders(int page, int countPerPage);
    OrderDetailsDto GetOrderDetails(int OrderId);
    bool UpdateOrder(OrderEditDto orderEdit);
    bool DeleteOrder(int Id);

    UserOrderDetailsDto GetUserOrderDetailsDto(int id);
    IEnumerable<UserOrderDto> GetUserOrdersDto(string id);
    IEnumerable<OrderReadDto> GetOrdersFiltred(OrderFilterDTO filterDTO);

}
