using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cart_System.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Intail03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
              table: "AspNetUserClaims",
              columns: new[] { "Id", "UserId", "ClaimType", "ClaimValue" },
              values:
              new object[] {
                    7,
                "1995b132-36e5-47e4-9d21-cd9a86cd0bce",
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                "581f7a57-8bc0-4a89-b5d4-72286dq4092q",
              }


          );

            migrationBuilder.InsertData(
             table: "AspNetUserClaims",
             columns: new[] { "Id", "UserId", "ClaimType", "ClaimValue" },
             values:
             new object[] {
                              8,
                "1995b132-36e5-47e4-9d21-cd9a86cd0bce",
                "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                "Admin"
                }


         );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
