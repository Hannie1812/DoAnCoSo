using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebTimNguoiThatLac.Migrations
{
    /// <inheritdoc />
    public partial class themlan2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NguoiThamGia_HopThoaiTinNhan_MaHopThoaiTinNhan",
                table: "NguoiThamGia");

            migrationBuilder.DropForeignKey(
                name: "FK_TinNhan_AspNetUsers_MaNguoiGui",
                table: "TinNhan");

            migrationBuilder.DropForeignKey(
                name: "FK_TinNhan_HopThoaiTinNhan_MaHopThoaiTinNhan",
                table: "TinNhan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TinNhan",
                table: "TinNhan");

            migrationBuilder.RenameTable(
                name: "TinNhan",
                newName: "TinNhans");

            migrationBuilder.RenameIndex(
                name: "IX_TinNhan_MaNguoiGui",
                table: "TinNhans",
                newName: "IX_TinNhans_MaNguoiGui");

            migrationBuilder.RenameIndex(
                name: "IX_TinNhan_MaHopThoaiTinNhan",
                table: "TinNhans",
                newName: "IX_TinNhans_MaHopThoaiTinNhan");

            migrationBuilder.AddColumn<int>(
                name: "HopThoaiTinNhanId",
                table: "NguoiThamGia",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TinNhans",
                table: "TinNhans",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiThamGia_HopThoaiTinNhanId",
                table: "NguoiThamGia",
                column: "HopThoaiTinNhanId");

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiThamGia_HopThoaiTinNhan_HopThoaiTinNhanId",
                table: "NguoiThamGia",
                column: "HopThoaiTinNhanId",
                principalTable: "HopThoaiTinNhan",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiThamGia_TinNhans_MaHopThoaiTinNhan",
                table: "NguoiThamGia",
                column: "MaHopThoaiTinNhan",
                principalTable: "TinNhans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TinNhans_AspNetUsers_MaNguoiGui",
                table: "TinNhans",
                column: "MaNguoiGui",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TinNhans_HopThoaiTinNhan_MaHopThoaiTinNhan",
                table: "TinNhans",
                column: "MaHopThoaiTinNhan",
                principalTable: "HopThoaiTinNhan",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NguoiThamGia_HopThoaiTinNhan_HopThoaiTinNhanId",
                table: "NguoiThamGia");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiThamGia_TinNhans_MaHopThoaiTinNhan",
                table: "NguoiThamGia");

            migrationBuilder.DropForeignKey(
                name: "FK_TinNhans_AspNetUsers_MaNguoiGui",
                table: "TinNhans");

            migrationBuilder.DropForeignKey(
                name: "FK_TinNhans_HopThoaiTinNhan_MaHopThoaiTinNhan",
                table: "TinNhans");

            migrationBuilder.DropIndex(
                name: "IX_NguoiThamGia_HopThoaiTinNhanId",
                table: "NguoiThamGia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TinNhans",
                table: "TinNhans");

            migrationBuilder.DropColumn(
                name: "HopThoaiTinNhanId",
                table: "NguoiThamGia");

            migrationBuilder.RenameTable(
                name: "TinNhans",
                newName: "TinNhan");

            migrationBuilder.RenameIndex(
                name: "IX_TinNhans_MaNguoiGui",
                table: "TinNhan",
                newName: "IX_TinNhan_MaNguoiGui");

            migrationBuilder.RenameIndex(
                name: "IX_TinNhans_MaHopThoaiTinNhan",
                table: "TinNhan",
                newName: "IX_TinNhan_MaHopThoaiTinNhan");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TinNhan",
                table: "TinNhan",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiThamGia_HopThoaiTinNhan_MaHopThoaiTinNhan",
                table: "NguoiThamGia",
                column: "MaHopThoaiTinNhan",
                principalTable: "HopThoaiTinNhan",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TinNhan_AspNetUsers_MaNguoiGui",
                table: "TinNhan",
                column: "MaNguoiGui",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TinNhan_HopThoaiTinNhan_MaHopThoaiTinNhan",
                table: "TinNhan",
                column: "MaHopThoaiTinNhan",
                principalTable: "HopThoaiTinNhan",
                principalColumn: "Id");
        }
    }
}
