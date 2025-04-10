using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebTimNguoiThatLac.Migrations
{
    /// <inheritdoc />
    public partial class themlan3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NguoiThamGia_HopThoaiTinNhan_HopThoaiTinNhanId",
                table: "NguoiThamGia");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiThamGia_TinNhans_MaHopThoaiTinNhan",
                table: "NguoiThamGia");

            migrationBuilder.DropIndex(
                name: "IX_NguoiThamGia_HopThoaiTinNhanId",
                table: "NguoiThamGia");

            migrationBuilder.DropColumn(
                name: "HopThoaiTinNhanId",
                table: "NguoiThamGia");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "NguoiThamGia",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastMessageTime",
                table: "HopThoaiTinNhan",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiThamGia_HopThoaiTinNhan_MaHopThoaiTinNhan",
                table: "NguoiThamGia",
                column: "MaHopThoaiTinNhan",
                principalTable: "HopThoaiTinNhan",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NguoiThamGia_HopThoaiTinNhan_MaHopThoaiTinNhan",
                table: "NguoiThamGia");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "NguoiThamGia");

            migrationBuilder.DropColumn(
                name: "LastMessageTime",
                table: "HopThoaiTinNhan");

            migrationBuilder.AddColumn<int>(
                name: "HopThoaiTinNhanId",
                table: "NguoiThamGia",
                type: "int",
                nullable: true);

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
        }
    }
}
