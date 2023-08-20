using Cart_System.DAL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart_System.BL;

public class AdminDashboardManager : IAdminDashboardManager
{
    private readonly IUnitOfWork _unitOfWork;

    public AdminDashboardManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

    }

    public bool DeleteUser(string userId)
    {
        User? user = _unitOfWork.UserRepo.GetById(userId);

        if (user is null) { return false; }


        _unitOfWork.UserRepo.Delete(user);
        _unitOfWork.Savechanges();
        return true;
    }

    public IEnumerable<UserDashboardReadDto> GetAllUsers()
    {
        IEnumerable<User> UsersFromDB = _unitOfWork.DashboardUserRepo.GetAll();
        IEnumerable<UserDashboardReadDto> AllUsersDetails = UsersFromDB.Select(u => new UserDashboardReadDto
        {
            Id = u.Id,
            FullName = u.FullName,
            Image = u.UserImage,
            PhoneNum = u.PhoneNumber,
            Email = u.Email,
            Role = u.Role.ToString()
        }) ;
        return AllUsersDetails;
    }

    public UserDashboardReadDto GetUserById(string userId)
    {
        User userFromDB = _unitOfWork.DashboardUserRepo.GetByUserId(userId);
        UserDashboardReadDto user = new UserDashboardReadDto
        {
            Id = userFromDB.Id,
            Email = userFromDB.Email,
            FullName = userFromDB.FullName,
          
            Role = userFromDB.Role.ToString(),
            Orders = userFromDB.Orders.Select(o => new UserDashboardOrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                OrderStatus = Enum.GetName(typeof(OrderStatus), o.OrderStatus),
                DeliverdDate = o.DeliverdDate,
            }),
          
        };
        return user;
    }


}
