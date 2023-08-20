
using Cart_System.BL;
using Cart_System.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Text.Encodings.Web;

namespace Cart_System.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<User> manager;
        private readonly ILogger<UserController> logger;
        private readonly IWebHostEnvironment _hostEnvironment;


        public UserController(IConfiguration configuration,
             IWebHostEnvironment hostEnvironment,
                                 UserManager<User> manager,
                                 ILogger<UserController> logger
                                  )
        {
            this.configuration = configuration;
            _hostEnvironment = hostEnvironment;

            this.manager = manager;
            this.logger = logger;
        }

        #region Login
        [HttpPost]
        [Route("Login")]
        public ActionResult<TokenDto> Login([FromForm]LoginDto loginCredientials)
        {
            // Search by Email and check if user found or Not 
            User? user = manager.FindByEmailAsync(loginCredientials.Email).Result;
            if (user is null) { return NotFound("Invalid Email!"); }

            // Check On Password
            bool isValiduser = manager.CheckPasswordAsync(user, loginCredientials.Password).Result;
            if (!isValiduser)
            {
                return BadRequest("Invalid Password!");
            }

            // Get claims
            List<Claim> claims = manager.GetClaimsAsync(user).Result.ToList();

            // get Secret Key
            string? secretKey = configuration.GetValue<string>("SecretKey");
            byte[] keyAsBytes = Encoding.ASCII.GetBytes(secretKey!);
            SymmetricSecurityKey key = new SymmetricSecurityKey(keyAsBytes);

            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            DateTime exp = DateTime.Now.AddDays(20);//expire after 20days
            JwtSecurityToken jwtSecurity = new JwtSecurityToken(claims: claims, signingCredentials: signingCredentials, expires: exp);

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwtSecurity);

            var currentUser = manager.GetUserAsync(User).Result;
            return new TokenDto
            {
                Token = token,
                Role = user.Role.ToString()
            };
        }
        #endregion

        #region Register

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<TokenDto>> Register([FromForm] RegisterDto credentials)
        {
            var newFileName = $"{Guid.NewGuid()}{Path.GetExtension(credentials.Image.FileName)}";
            var imagesPath = Path.Combine(Environment.CurrentDirectory, "Images");
            var fulFilePath = Path.Combine(imagesPath, newFileName);
            using var stream = new FileStream(fulFilePath, FileMode.Create);
            credentials.Image.CopyTo(stream);

            var url = $"{Request.Scheme}://{Request.Host}/Images/{newFileName}";

            User user = new User
            {
                FullName = credentials.FullName,
                PhoneNumber = credentials.PhoneNum,
                Email = credentials.Email,
                UserImage = url,
                UserName = credentials.Email,
                Role = Role.Customer,
               
            };

            var result = await manager.CreateAsync(user, credentials.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            var claimsResult = await manager.AddClaimsAsync(user, claims);

            if (!claimsResult.Succeeded)
            {
                return BadRequest(claimsResult.Errors);
            }



            string? secretKey = configuration.GetValue<string>("SecretKey");
            byte[] keyAsBytes = Encoding.ASCII.GetBytes(secretKey!);
            SymmetricSecurityKey key = new SymmetricSecurityKey(keyAsBytes);

            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            DateTime exp = DateTime.Now.AddDays(20);//expire after 20days
            JwtSecurityToken jwtSecurity = new JwtSecurityToken(claims: claims, signingCredentials: signingCredentials, expires: exp);

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwtSecurity);

            return new TokenDto
            {
                Token = token,
                Role = user.Role.ToString()
            };
        }


        #endregion





  

    }
}