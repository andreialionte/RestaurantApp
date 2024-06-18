#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantApp.Migrations
{
    /// <inheritdoc />
    public partial class one_to_many_item_to_Category : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                schema: "RestaurantAppSchema",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "RestaurantAppSchema",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryId",
                schema: "RestaurantAppSchema",
                table: "Items",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Categories_CategoryId",
                schema: "RestaurantAppSchema",
                table: "Items",
                column: "CategoryId",
                principalSchema: "RestaurantAppSchema",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Categories_CategoryId",
                schema: "RestaurantAppSchema",
                table: "Items");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "RestaurantAppSchema");

            migrationBuilder.DropIndex(
                name: "IX_Items_CategoryId",
                schema: "RestaurantAppSchema",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "RestaurantAppSchema",
                table: "Items");
        }
    }
}
