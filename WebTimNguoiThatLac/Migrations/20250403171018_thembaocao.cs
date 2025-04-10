using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebTimNguoiThatLac.Migrations
{
    /// <inheritdoc />
    public partial class thembaocao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaoCaoBaiViet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaBaiViet = table.Column<int>(type: "int", nullable: true),
                    MaNguoiBaoCao = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LyDo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChiTiet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayBaoCao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    check = table.Column<bool>(type: "bit", nullable: false),
                    DaDoc = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaoCaoBaiViet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaoCaoBaiViet_AspNetUsers_MaNguoiBaoCao",
                        column: x => x.MaNguoiBaoCao,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaoCaoBaiViet_TimNguoi_MaBaiViet",
                        column: x => x.MaBaiViet,
                        principalTable: "TimNguoi",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BaoCaoBinhLuan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaBinhLuan = table.Column<int>(type: "int", nullable: true),
                    MaNguoiBaoCao = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LyDo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayBaoCao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    check = table.Column<bool>(type: "bit", nullable: false),
                    DaDoc = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaoCaoBinhLuan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaoCaoBinhLuan_AspNetUsers_MaNguoiBaoCao",
                        column: x => x.MaNguoiBaoCao,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaoCaoBinhLuan_BinhLuan_MaBinhLuan",
                        column: x => x.MaBinhLuan,
                        principalTable: "BinhLuan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoBaiViet_MaBaiViet",
                table: "BaoCaoBaiViet",
                column: "MaBaiViet");

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoBaiViet_MaNguoiBaoCao",
                table: "BaoCaoBaiViet",
                column: "MaNguoiBaoCao");

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoBinhLuan_MaBinhLuan",
                table: "BaoCaoBinhLuan",
                column: "MaBinhLuan");

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoBinhLuan_MaNguoiBaoCao",
                table: "BaoCaoBinhLuan",
                column: "MaNguoiBaoCao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaoCaoBaiViet");

            migrationBuilder.DropTable(
                name: "BaoCaoBinhLuan");
        }
    }
}
