﻿using Cart_System.BL;
using Cart_System.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    public class UserProductsCartController : ControllerBase
    {
        private readonly IUserProductsCartsManager _userProductsCartsManager;
        private readonly UserManager<User> _manager;

        public UserProductsCartController(IUserProductsCartsManager userProductsCartsManager,
            UserManager<User> manager)
        {
            _userProductsCartsManager = userProductsCartsManager;
            _manager = manager;
        }


        #region Get All Products In Cart

        [HttpGet]
        public ActionResult GetAllProductsInCart()
        {
            //temp user id until we use authentication and then
            // we will get that userId from token by this function
            //var currentUser = _userManager.GetUserAsync(User).Result;
            //then get user id from current user details
            var userIdFromToken = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var currentUser = _manager.GetUserAsync(User).Result;
            if (userIdFromToken is null)
            {
                return BadRequest("not logged in");
            }
            //string? userId = "18c2ddd6-ec81-4e72-ab47-88958cd1e43a";
            IEnumerable<AllProductsReadDto> Products = _userProductsCartsManager.GetAllUserProductsInCart(userIdFromToken);

            return Ok(Products);

        }


        #endregion

        #region Add Product To Cart

        [HttpPost]
        [Route("AddProduct")]
        public ActionResult AddProductToCart(productToAddToCartDto product)
        {
            var userIdFromToken = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var currentUser = _manager.GetUserAsync(User).Result;
            if (userIdFromToken is null)
            {
                return BadRequest("not logged in");

            }
            string status = _userProductsCartsManager.AddProductToCart(product, userIdFromToken);

            return Ok(new
            {
                message=status
            });

        }

        #endregion

        #region Update Product Quantity In Cart
        [HttpPut]
        [Route("UpdateProduct")]

        public ActionResult UpdateProductQuantityInCart(ProductQuantityinCartUpdateDto product)
        {
            var userIdFromToken = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var currentUser = _manager.GetUserAsync(User).Result;
            if (userIdFromToken is null)
            {
                return BadRequest("not logged in");
            }
            string status = _userProductsCartsManager.UpdateProductQuantityInCart(product, userIdFromToken);
            return Ok(status);

        
        }

        #endregion

        #region Delete Product
        [HttpDelete]
        [Route("DeleteProduct/{id}")]

        public ActionResult DeleteProduct(int id)
        {
            var userIdFromToken = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var currentUser = _manager.GetUserAsync(User).Result;
            if (userIdFromToken is null)
            {
                return BadRequest("not logged in");
            }

            _userProductsCartsManager.DeleteProductFromCart(id, userIdFromToken);
            return Ok("product Deleted from Cart");

            
      

            

        }

        #endregion

    }
}
