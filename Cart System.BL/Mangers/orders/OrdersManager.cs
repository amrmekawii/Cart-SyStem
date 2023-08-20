using Cart_System.DAL;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart_System.BL;

public class OrdersManager : IOrdersManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<OrdersManager> _logger;

    public OrdersManager(IUnitOfWork unitOfWork, ILogger<OrdersManager> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #region Add Order

    public void AddNewOrder(string userId)
    {
        //1-Add new order in order table
        Order newOrder = new Order
        {
            OrderStatus = OrderStatus.Pending,
            OrderDate = DateOnly.FromDateTime(DateTime.Now),
            UserId = userId,
        };
        _unitOfWork.OrderRepo.Add(newOrder);
        _unitOfWork.Savechanges();
        //2-we need the orderId of the new order to use it in orderdetails table
        //so lets get that 
        int LastOrderId = _unitOfWork.OrderRepo.GetLastUserOrder(userId);

        //3-transfer products from cart to order 
        IEnumerable<UserProductsCart> productsFromCart = _unitOfWork.UserProdutsCartRepo.GetAllProductsByUserId(userId);

        var orderProducts = productsFromCart.Select(p => new OrderProductDetails
        {
            OrderId = LastOrderId+1,
            ProductId = p.ProductId,
            Quantity = p.Quantity,
            ProductPriceAtThisTime=p.Product.Price*(1- (p.Product.Discount/100))
        });

        _unitOfWork.OrdersDetailsRepo.AddRange(orderProducts);

        //5-Make The UserCart Empty

        _unitOfWork.UserProdutsCartRepo.DeleteAllProductsFromUserCart(userId);
        _unitOfWork.Savechanges();
    }

    #endregion

    #region Get all Orders

    public IEnumerable<OrderReadDto> GetAllOrders(int page, int countPerPage)
    {
        var orderFromDb = _unitOfWork.OrderRepo.GetOrdersWithData(page, countPerPage);

        if(orderFromDb is null)
        {
            return null;
        }
        var orderReadDto = orderFromDb
            .Select(o => new OrderReadDto
            {
                Id = o.Id,
                OrderStatus = Enum.GetName(typeof(OrderStatus), o.OrderStatus),
                OrderDate = o.OrderDate,
                UserId = o.User.Id,
                UserName = (o.User.FullName),
                ProductCount = o.OrdersProductDetails.Count(),
                TotalPrice = o.OrdersProductDetails.Sum(op => Math.Round( (op.Product.Price - (op.Product.Price * (op.Product.Discount/100))) * op.Quantity, 0)),
            });

        return orderReadDto;
    }

    #endregion

    #region Get Order Details

    public OrderDetailsDto GetOrderDetails(int OrderId)
    {
        Order order = _unitOfWork.OrderRepo.GetOrderWithProducts(OrderId);

        if(order is null)
        {
            return null;
        }

        OrderDetailsDto orderDetails = new OrderDetailsDto
        {
            Id = order.Id,
            OrderStatus = Enum.GetName(typeof(OrderStatus), order.OrderStatus),
            OrderDate = order.OrderDate,
            DeliverdDate = order.DeliverdDate,
            UserId = order.User.Id,
            UserName = (order.User.FullName),
            ProductCount = order.OrdersProductDetails.Count(),
            TotalPrice = order.OrdersProductDetails.Sum(op => Math.Round((op.Product.Price - (op.Product.Price * (op.Product.Discount / 100))) * op.Quantity, 0)),
            ProductsInOrder = order.OrdersProductDetails.Select(op => new ProductsInOrder
            {
                Quantity = op.Quantity,
                ProductName = op.Product.Name,
                ProductPrice = op.Product.Price,
                ProductImage = op.Product.ProductImages.FirstOrDefault()?.ImageUrl??"",
                Discount = op.Product.Discount,
                ProductId = op.ProductId
            }),
        

        };

        return orderDetails;
    }

    #endregion

    #region Update Order

    public bool UpdateOrder(OrderEditDto orderEdit)
    {
        var order = _unitOfWork.OrderRepo.GetById(orderEdit.Id);
        if (order is null)
        {
            return false;
        }

        order.OrderStatus = (OrderStatus)Enum.ToObject(typeof(OrderStatus), orderEdit.OrderStatus);

        if ((OrderStatus)Enum.ToObject(typeof(OrderStatus), orderEdit.OrderStatus) == OrderStatus.Fullfilled)
        {
            order.DeliverdDate = DateOnly.FromDateTime(DateTime.Now);
        }

        return _unitOfWork.Savechanges() > 0;
    }

    #endregion

    #region Delete Order

    public bool DeleteOrder(int Id)
    {
        var order = _unitOfWork.OrderRepo.GetById(Id);
        if (order is null)
        {
            return false;
        }

        _unitOfWork.OrderRepo.Delete(order);
        return _unitOfWork.Savechanges() > 0;
    }

    #endregion


    #region UserOrderDetails
    public UserOrderDetailsDto? GetUserOrderDetailsDto(int id)
    {

        List<OrderProductDetails> OrderProductDetails = _unitOfWork.UserRepo.GetUsersOrderDetails(id).ToList();
        if (OrderProductDetails == null)
        {
            return null;
        }

        IEnumerable<UserOrderProductsDetailsDto> products = OrderProductDetails.Select(p => new UserOrderProductsDetailsDto
        {
            product_Id = p.ProductId,
            Image = p.Product.ProductImages.FirstOrDefault()?.ImageUrl ?? "",
            Price = p.Product.Price,
            Discount = p.Product.Discount,
            Quantity = p.Quantity,
            title = p.Product.Name,
            IsReviewed = p.IsReviewed,
            ProductPriceAtThisTime = p.ProductPriceAtThisTime


        });

    

        //check order status is delievered or not 
        bool OrderStatus = ((int)OrderProductDetails.FirstOrDefault().Order.OrderStatus) == 3;


        UserOrderDetailsDto orderDetails = new UserOrderDetailsDto
        {
            OrderProducts = products,
            IsOrderDelieverd = OrderStatus
        };


        return orderDetails;

    }
    #endregion

    #region UserOrder
    public IEnumerable<UserOrderDto> GetUserOrdersDto(string id)
    {
        IEnumerable<Order>? ordersFromDB = _unitOfWork.UserRepo.GetUserOrders(id);
        if (ordersFromDB == null) { return null; }
        IEnumerable<UserOrderDto> ordersDto = ordersFromDB.Select(order => new UserOrderDto
        {
            Id = order.Id,
            OrderStatus = Enum.GetName(typeof(OrderStatus), order.OrderStatus),
            DeliverdDate = order.DeliverdDate,
            Products = order.OrdersProductDetails.Select(ip => new UserProductDto
            {

                Image = ip.Product.ProductImages.FirstOrDefault()?.ImageUrl ?? "",
                title = ip.Product.Name,
            }

            )

        });
        return ordersDto.ToList();
    }

    public IEnumerable<OrderReadDto> GetOrdersFiltred(OrderFilterDTO filterDTO)
    {

        FilterOrderObject PathDataToRepo = new FilterOrderObject
        {
            UseiD = filterDTO?.UseiD,
            OrderState = filterDTO?.OrderState,

        };

      var Ordersdto = _unitOfWork.OrderRepo.GetOrdersFiltered(PathDataToRepo);
        return Ordersdto.Select(o => new OrderReadDto
        {
            Id = o.Id,
            OrderStatus = Enum.GetName(typeof(OrderStatus), o.OrderStatus),
            OrderDate = o.OrderDate,
            UserId = o.User.Id,
            UserName = (o.User.FullName),
            ProductCount = o.OrdersProductDetails.Count(),
            TotalPrice = o.OrdersProductDetails.Sum(op => Math.Round((op.Product.Price - (op.Product.Price * (op.Product.Discount / 100))) * op.Quantity, 0)),
        });
    }

    #endregion
}
