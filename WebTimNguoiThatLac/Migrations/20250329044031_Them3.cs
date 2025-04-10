using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebTimNguoiThatLac.Migrations
{
    /// <inheritdoc />
    public partial class Them3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThongBaos_AspNetUsers_MaNguoiDung",
                table: "ThongBaos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThongBaos",
                table: "ThongBaos");

            migrationBuilder.RenameTable(
                name: "ThongBaos",
                newName: "ThongBao");

            migrationBuilder.RenameIndex(
                name: "IX_ThongBaos_MaNguoiDung",
                table: "ThongBao",
                newName: "IX_ThongBao_MaNguoiDung");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThongBao",
                table: "ThongBao",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ThongBao_AspNetUsers_MaNguoiDung",
                table: "ThongBao",
                column: "MaNguoiDung",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThongBao_AspNetUsers_MaNguoiDung",
                table: "ThongBao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThongBao",
                table: "ThongBao");

            migrationBuilder.RenameTable(
                name: "ThongBao",
                newName: "ThongBaos");

            migrationBuilder.RenameIndex(
                name: "IX_ThongBao_MaNguoiDung",
                table: "ThongBaos",
                newName: "IX_ThongBaos_MaNguoiDung");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThongBaos",
                table: "ThongBaos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ThongBaos_AspNetUsers_MaNguoiDung",
                table: "ThongBaos",
                column: "MaNguoiDung",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
