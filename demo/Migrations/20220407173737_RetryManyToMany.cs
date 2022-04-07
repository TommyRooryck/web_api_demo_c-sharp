using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace demo.Migrations
{
    public partial class RetryManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_carts_Cartid",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_Cartid",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Cartid",
                table: "Item");

            migrationBuilder.CreateTable(
                name: "cartItems",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    itemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cartItems", x => x.id);
                    table.ForeignKey(
                        name: "FK_cartItems_carts_cartId",
                        column: x => x.cartId,
                        principalTable: "carts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cartItems_Item_itemId",
                        column: x => x.itemId,
                        principalTable: "Item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cartItems_cartId",
                table: "cartItems",
                column: "cartId");

            migrationBuilder.CreateIndex(
                name: "IX_cartItems_itemId",
                table: "cartItems",
                column: "itemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cartItems");

            migrationBuilder.AddColumn<Guid>(
                name: "Cartid",
                table: "Item",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Item_Cartid",
                table: "Item",
                column: "Cartid");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_carts_Cartid",
                table: "Item",
                column: "Cartid",
                principalTable: "carts",
                principalColumn: "id");
        }
    }
}
