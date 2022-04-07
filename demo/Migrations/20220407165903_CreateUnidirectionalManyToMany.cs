using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace demo.Migrations
{
    public partial class CreateUnidirectionalManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItem");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "CartItem",
                columns: table => new
                {
                    cartsid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    itemsid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem", x => new { x.cartsid, x.itemsid });
                    table.ForeignKey(
                        name: "FK_CartItem_carts_cartsid",
                        column: x => x.cartsid,
                        principalTable: "carts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItem_Item_itemsid",
                        column: x => x.itemsid,
                        principalTable: "Item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_itemsid",
                table: "CartItem",
                column: "itemsid");
        }
    }
}
