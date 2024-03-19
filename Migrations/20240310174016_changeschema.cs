using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantApp.Migrations
{
    /// <inheritdoc />
    public partial class changeschema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "RestaurantAppSchema");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "RestaurantAppDb",
                newName: "Users",
                newSchema: "RestaurantAppSchema");

            migrationBuilder.RenameTable(
                name: "Payments",
                schema: "RestaurantAppDb",
                newName: "Payments",
                newSchema: "RestaurantAppSchema");

            migrationBuilder.RenameTable(
                name: "Orders",
                schema: "RestaurantAppDb",
                newName: "Orders",
                newSchema: "RestaurantAppSchema");

            migrationBuilder.RenameTable(
                name: "Items",
                schema: "RestaurantAppDb",
                newName: "Items",
                newSchema: "RestaurantAppSchema");

            migrationBuilder.RenameTable(
                name: "Auth",
                schema: "RestaurantAppDb",
                newName: "Auth",
                newSchema: "RestaurantAppSchema");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "RestaurantAppDb");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "RestaurantAppSchema",
                newName: "Users",
                newSchema: "RestaurantAppDb");

            migrationBuilder.RenameTable(
                name: "Payments",
                schema: "RestaurantAppSchema",
                newName: "Payments",
                newSchema: "RestaurantAppDb");

            migrationBuilder.RenameTable(
                name: "Orders",
                schema: "RestaurantAppSchema",
                newName: "Orders",
                newSchema: "RestaurantAppDb");

            migrationBuilder.RenameTable(
                name: "Items",
                schema: "RestaurantAppSchema",
                newName: "Items",
                newSchema: "RestaurantAppDb");

            migrationBuilder.RenameTable(
                name: "Auth",
                schema: "RestaurantAppSchema",
                newName: "Auth",
                newSchema: "RestaurantAppDb");
        }
    }
}
