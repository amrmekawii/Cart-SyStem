using Microsoft.AspNetCore.Http;

namespace Cart_System.BL;
public class RegisterDto
{

    public string FullName { get; set; } = string.Empty;
    public string Email  { get; set; } = string.Empty;
    public string PhoneNum { get; set; } = string.Empty;
    public IFormFile Image { get; set; } 
    public string Password  { get; set; } = string.Empty;
}
