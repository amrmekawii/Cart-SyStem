
using Cart_System.BL;
using Cart_System.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Cart_System.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddScoped<UserManager<User>>();
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<IProductRepo, ProductRepo>();
            builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
            builder.Services.AddScoped<IUserProductsCartRepo, UserProdutsCartRepo>();
            builder.Services.AddScoped<IOrderRepo, OrderRepo>();
            builder.Services.AddScoped<IOrdersDetailsRepo, OrdersDetailsRepo>();
            builder.Services.AddScoped<IDashboardUserRepo, DashboardUserRepo>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IHelper, Helper>();
            builder.Services.AddScoped<IProductsManager, ProductsManager>();
            builder.Services.AddScoped<ICategoriesManager, CategoriesManager>();
            builder.Services.AddScoped<IOrderDetailsManager, OrderDetailsManager>();
            builder.Services.AddScoped<IDashboardUserRepo, DashboardUserRepo>();
            builder.Services.AddScoped<IAdminDashboardManager, AdminDashboardManager>();
            builder.Services.AddScoped<IOrderRepo, OrderRepo>();
            builder.Services.AddScoped<IOrdersManager, OrdersManager>();
            builder.Services.AddScoped<IUserProductsCartsManager, UserProductsCartsManager>();


            #region Database

            builder.Services.AddDbContext<CartSystemContext>(options => options
            .UseNpgsql("Server=localhost;Database=CartSystem;Port=5432; Username=postgres;Password=123"));
            #endregion
            #region Identity
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequiredLength = 5;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;


            }).AddEntityFrameworkStores<CartSystemContext>()
              .AddDefaultTokenProviders();
            #endregion

            #region Authentication

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "UserScema"; // for handling Authentication
                options.DefaultChallengeScheme = "UserScema"; // for handling Challenge
            }).AddJwtBearer("UserScema", options =>
            {

                // Access secretkey to use it to validate Requests
                string? stringKey = builder.Configuration.GetValue<string>("SecretKey");
                byte[] keyASBytes = Encoding.ASCII.GetBytes(stringKey!);
                SymmetricSecurityKey key = new SymmetricSecurityKey(keyASBytes);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });


            #endregion
            #region Authorization

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ForCustomer", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "Customer");
                });

                options.AddPolicy("ForAdmin", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "Admin");
                });
            });


            #endregion
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            #region make static file 
            var staticFilesPath = Path.Combine(Environment.CurrentDirectory, "Images");
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(staticFilesPath),
                RequestPath = "/Images"
            });
            #endregion
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}