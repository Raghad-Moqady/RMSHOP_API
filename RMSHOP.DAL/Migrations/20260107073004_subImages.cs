using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMSHOP.DAL.Migrations
{
    /// <inheritdoc />
    public partial class subImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductSubImages",
                columns: table => new
                {
                    ImageName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSubImages", x => new { x.ImageName, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ProductSubImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubImages_ProductId",
                table: "ProductSubImages",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSubImages");
        }
    }
}
