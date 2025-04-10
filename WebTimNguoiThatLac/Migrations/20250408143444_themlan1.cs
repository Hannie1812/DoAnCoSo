using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebTimNguoiThatLac.Migrations
{
    /// <inheritdoc />
    public partial class themlan1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HopThoaiTinNhan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TieuDeChat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsGroup = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HopThoaiTinNhan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NguoiThamGia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHopThoaiTinNhan = table.Column<int>(type: "int", nullable: true),
                    MaNguoiThamGia = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NgayThamGia = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiThamGia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NguoiThamGia_AspNetUsers_MaNguoiThamGia",
                        column: x => x.MaNguoiThamGia,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NguoiThamGia_HopThoaiTinNhan_MaHopThoaiTinNhan",
                        column: x => x.MaHopThoaiTinNhan,
                        principalTable: "HopThoaiTinNhan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TinNhan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHopThoaiTinNhan = table.Column<int>(type: "int", nullable: true),
                    MaNguoiGui = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayGui = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinNhan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TinNhan_AspNetUsers_MaNguoiGui",
                        column: x => x.MaNguoiGui,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TinNhan_HopThoaiTinNhan_MaHopThoaiTinNhan",
                        column: x => x.MaHopThoaiTinNhan,
                        principalTable: "HopThoaiTinNhan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NguoiThamGia_MaHopThoaiTinNhan",
                table: "NguoiThamGia",
                column: "MaHopThoaiTinNhan");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiThamGia_MaNguoiThamGia",
                table: "NguoiThamGia",
                column: "MaNguoiThamGia");

            migrationBuilder.CreateIndex(
                name: "IX_TinNhan_MaHopThoaiTinNhan",
                table: "TinNhan",
                column: "MaHopThoaiTinNhan");

            migrationBuilder.CreateIndex(
                name: "IX_TinNhan_MaNguoiGui",
                table: "TinNhan",
                column: "MaNguoiGui");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NguoiThamGia");

            migrationBuilder.DropTable(
                name: "TinNhan");

            migrationBuilder.DropTable(
                name: "HopThoaiTinNhan");
        }
    }
}
