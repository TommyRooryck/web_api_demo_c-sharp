using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace demo.Migrations
{
    public partial class CreateCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Cartid",
                table: "Item",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "carts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carts", x => x.id);
                });

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

            migrationBuilder.DropTable(
                name: "carts");

            migrationBuilder.DropIndex(
                name: "IX_Item_Cartid",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Cartid",
                table: "Item");
        }
    }
}
