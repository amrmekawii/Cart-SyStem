using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cart_System.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Intial02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               table: "AspNetRoles",
               columns: new[] { "Id", "Name", "NormalizedName" },
               values: new object[]
               {
                    "581f7a57-8dm0-4a89-a5a4-72286da4092a",
                    "Admin",
                    "ADMIN"
               }

               );
            migrationBuilder.InsertData(
        table: "AspNetUsers",
        columns: new[] { "Id","Role" , "FullName", "UserImage", "Role", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp" , "PhoneNumber" , "PhoneNumberConfirmed" , "TwoFactorEnabled" , "LockoutEnabled", "AccessFailedCount" },
        values: new object[] {
             "1995b132-36e5-47e4-9d21-cd9a86cd0bce", 
             0,
            "Admin", // Name of the admin
            "", // URL to admin's profile picture
            1, // Role: 1 for Admin, 0 for Regular user
            "admin", // Username
            "ADMIN", // Normalized username
            "admin@admin.com", // Email
            "ADMIN@ADMIN.COM", // Normalized email
            true, // EmailConfirmed
            HashPassword("admin"), // Hashed password
            Guid.NewGuid().ToString(),
            "12345678",
            true,
            true,
            true,
            0
        });

          

            // Associate Admin user with the Admin role
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "1995b132-36e5-47e4-9d21-cd9a86cd0bce", "581f7a57-8dm0-4a89-a5a4-72286da4092a" }); // Replace with the actual role ID for Admin

            string HashPassword(string password)
            {
                var passwordHasher = new PasswordHasher<IdentityUser>(); // Change to your identity user class
                return passwordHasher.HashPassword(null, password);
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
