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
   [Authorize(Policy = "ForAdmin")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardManager _adminDashboardManager;
        private readonly UserManager<User> _manager;
        private readonly IOrdersManager _ordersManager;
        private readonly ILogger<AdminDashboardController> _logger;



        public AdminDashboardController(IAdminDashboardManager adminDashboardManager,
            ILogger<AdminDashboardController> logger,
        UserManager<User> manager, IOrdersManager ordersManager)
        {
            
            _adminDashboardManager = adminDashboardManager;
            _manager = manager;
            _ordersManager = ordersManager;
                _logger = logger;

        }

        #region Get All Users
        [HttpGet]
        [Route("GetAllUsers")]
        public ActionResult GetAllUsers()
        {
            IEnumerable<UserDashboardReadDto> allUsers = _adminDashboardManager.GetAllUsers();
            return Ok(allUsers);
        }

        #endregion

        #region Get User By Id
        [HttpGet]
        [Route("User/{userId}")]
        public ActionResult GetUserById(string userId)
        {
            UserDashboardReadDto user = _adminDashboardManager.GetUserById(userId);
            return Ok(user);
        }

        #endregion

        #region Get all Orders Dashboard order by date and used Pagination
        [HttpGet]
        [Route("Dashboard/GetAllOrdersPagination")]
        public ActionResult<IEnumerable<OrderReadDto>> GetAllOrdersPagination(int page, int countPerPage)
        {
            IEnumerable<OrderReadDto> orderReadDtos = _ordersManager.GetAllOrders(page, countPerPage);
            if (orderReadDtos is null)
            {
                return BadRequest();
            }

            return Ok(orderReadDtos);
        }

        #endregion

        #region Firter order by userid and orderstate
        [HttpPost]
        [Route("GetFiltreOrders")]
        public ActionResult<IEnumerable<OrderReadDto>> GetFiltreOrders([FromForm] OrderFilterDTO filterDT)
        {

            IEnumerable<OrderReadDto> orderReadDtos = _ordersManager.GetOrdersFiltred(filterDT);
            if (orderReadDtos is null)
            {
                return BadRequest();
            }

            return Ok(orderReadDtos);
        }
        #endregion

        #region Get order details Dashboard

        [HttpGet]
        [Route("Dashboard/GetOrderDetails/{Id}")]
        public ActionResult<OrderDetailsDto> GetOrderDetails(int Id)
        {
            OrderDetailsDto orderDetailsDto = _ordersManager.GetOrderDetails(Id);
            if (orderDetailsDto is null)
            {
                return BadRequest();
            }

            return Ok(orderDetailsDto);
        }

        #endregion

        #region Edit Order Dashboard

        [HttpPut]
        [Route("Dashboard/EditOrder")]
        public ActionResult Edit(OrderEditDto orderEditDto)
        {
            bool isEdited = _ordersManager.UpdateOrder(orderEditDto);

            return isEdited ? NoContent() : BadRequest();
        }

        #endregion

        #region Delete Order Dashboard

        [HttpDelete]
        [Route("Dashboard/DeleteOrder/{Id}")]
        public ActionResult Delete(int Id)
        {
            bool isDeleted = _ordersManager.DeleteOrder(Id);

            return isDeleted ? NoContent() : BadRequest();
        }

        #endregion

        #region Delete User

        [HttpDelete]
        [Route("DeleteUser/{userId}")]
        public ActionResult DeleteUserFromDashboard(string userId)
        {
            var isfound = _adminDashboardManager.DeleteUser(userId);

            if (!isfound) { return NotFound(); }

            return NoContent();

        } 
        #endregion

    }
}
