using Microsoft.AspNetCore.Identity;

namespace Cart_System.DAL;
public class User : IdentityUser
{
  
    public string FullName { get; set; } = string.Empty;
    public string UserImage { get; set; } = string.Empty;
    public Role Role { get; set; }
    public IEnumerable<UserProductsCart> UsersProductsCarts { get; set; } = new HashSet<UserProductsCart>();
    public IEnumerable<Order> Orders { get; set; } = new HashSet<Order>();
 



}

public enum Role
{
    Admin,
    Customer,
}


