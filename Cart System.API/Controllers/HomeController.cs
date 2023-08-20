
using Cart_System.BL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cart_System.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ICategoriesManager _categoriesManager;



        public HomeController( ICategoriesManager categoriesManager)
        {
            _categoriesManager = categoriesManager;

        }



        #region Get All Categories With All Products
        [HttpGet]
        [Route("CategoriesWithProducts")]
        public ActionResult<IEnumerable<CategoryDetailsDto>> GetAllCategoriesWithProducts()
        {
            IEnumerable<CategoryDetailsDto> categories = _categoriesManager.GetAllCategoriesWithProducts();

            return Ok(categories);
        }

        #endregion

      
    }
}
