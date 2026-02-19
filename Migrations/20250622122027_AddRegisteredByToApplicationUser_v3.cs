using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriEnergyConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddRegisteredByToApplicationUser_v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RegisteredById",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RegisteredById",
                table: "AspNetUsers",
                column: "RegisteredById");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_RegisteredById",
                table: "AspNetUsers",
                column: "RegisteredById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_RegisteredById",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RegisteredById",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RegisteredById",
                table: "AspNetUsers");
        }
    }
}
