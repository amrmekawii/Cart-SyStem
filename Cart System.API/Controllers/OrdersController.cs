using Cart_System.BL;
using Cart_System.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cart_System.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersManager _ordersManager;
        private readonly UserManager<User> _Usermanager;

        public OrdersController(IOrdersManager ordersManager, UserManager<User> manager)
        {
            _Usermanager = manager;
            _ordersManager = ordersManager;

        }

        #region Make new order (add order)

        [HttpGet]
        [Route("MakeNewOrder")]
        public ActionResult MakeNewOrder()
        {
            var userIdFromToken = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdFromToken is null)
            {
                return BadRequest("not logged in");
            }
            _ordersManager.AddNewOrder(userIdFromToken);


            return Ok("order Added Successfully");
        }

        #endregion

        #region GetUserOrders
        [HttpGet]
        [Route("orders")]
        public ActionResult<List<UserOrderDto>> GetUserOrders()
        {

            var currentUser = _Usermanager.GetUserAsync(User).Result;

            List<UserOrderDto>? userOrder = _ordersManager.GetUserOrdersDto(currentUser.Id) as List<UserOrderDto>;

            if (userOrder is null)

            { return null; }

            return Ok(userOrder);

        }
      
        #endregion

        #region getUserOrderDetails
        [HttpGet]
        [Route("orderDetails/{orderId}")]
        public ActionResult<UserOrderDetailsDto> GetOrderDetails(int orderId)
        {
       
            var orderDetails = _ordersManager.GetUserOrderDetailsDto(orderId);
            if (orderDetails is null)
            {
                return NotFound();
            }

            return Ok(orderDetails); //200 OK
        }
        #endregion region 


    }
}