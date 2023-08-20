using Cart_System.API;
using Cart_System.BL;
using Cart_System.DAL;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace Final.Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsManager _productsManager;
        private readonly CartSystemContext context;
        private readonly IHelper helper;

        public ProductsController(IProductsManager productsManager, CartSystemContext context, IHelper Helper)
        {
            _productsManager = productsManager;
            this.context = context;
            helper = Helper;
        }

        #region Get Product By Id
        [HttpGet]
        [Route("{id}")]
        public ActionResult<ProductDetailsDto> GetProductbyid(int id)
        {
            ProductDetailsDto product = _productsManager.GetProductByID(id);
            if (product == null) { return NotFound(); }
            return Ok(product);
        }

        #endregion

        #region Get all Products
        [HttpGet]
        [Route("Dashboard/GetAllProducts")]
       [Authorize(Policy = "ForAdmin")]

        public ActionResult<IEnumerable<ProductReadDto>> GetAllProducts()
        {
            IEnumerable<ProductReadDto> products = _productsManager.GetAllProducts();

            if (products is null)
            {
                return NotFound();
            }

            return Ok(products);
        }

        #endregion

        #region Add Product
        [Authorize(Policy = "ForAdmin")]
        [HttpPost]
        [Route("Dashboard/addProduct")]
        public ActionResult Add([FromForm]ProductAddDto product)
        {
            if (product == null) return BadRequest("no contant");
            _productsManager.AddProduct(product);
            return NoContent();

        }



        #endregion

        #region Edit Product

        [HttpPatch]
        [Route("/dashboard/editproduct")]
       [Authorize(Policy = "ForAdmin")]

        public ActionResult<Product> Edit([FromForm]ProductEditDto product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            _productsManager.EditProduct(product);
            return NoContent();

        }

        #endregion

        #region Delete Product
        [HttpDelete]
        [Route("Dashboard/DeleteProduct/{Id}")]
        [Authorize(Policy = "ForAdmin")]

        public ActionResult Delete(int Id)
        {
            bool isDeleted = _productsManager.DeleteProduct(Id);

            return isDeleted ? NoContent() : BadRequest();
        }

        #endregion

    }
}