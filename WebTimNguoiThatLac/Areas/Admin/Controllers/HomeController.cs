using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebTimNguoiThatLac.Areas.Admin.Models;
using WebTimNguoiThatLac.Data;
using WebTimNguoiThatLac.Models;

namespace WebTimNguoiThatLac.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]// chỉ cho phép admin
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? year, int? month, int? day, string khuVuc, string trangThai, int? tuan)
        {
            // Lấy dữ liệu từ database
            var query = _context.TimNguois.AsQueryable();

            // Áp dụng các bộ lọc
            if (year.HasValue) query = query.Where(x => x.NgayDang.Year == year.Value);
            if (month.HasValue) query = query.Where(x => x.NgayDang.Month == month.Value);
            if (day.HasValue) query = query.Where(x => x.NgayDang.Day == day.Value);
            if (!string.IsNullOrEmpty(khuVuc)) query = query.Where(x => x.KhuVuc == khuVuc);
            if (!string.IsNullOrEmpty(trangThai)) query = query.Where(x => x.TrangThai == trangThai);

            var baiViets = query.ToList();

            // Xử lý dữ liệu thống kê
            var now = DateTime.Now;
            int soNgay = tuan.HasValue ? tuan.Value * 7 : 7;
            var ngayLabels = Enumerable.Range(0, soNgay)
                .Select(i => now.AddDays(-i).Date)
                .OrderBy(d => d)
                .Select(d => d.ToString("dd/MM"))
                .ToList();

            var baiVietTheoNgay = Enumerable.Range(0, soNgay)
                .Select(i => now.AddDays(-i).Date)
                .OrderBy(d => d)
                .Select(ngay => baiViets.Count(x => x.NgayDang.Date == ngay))
                .ToList();

            // Tạo dữ liệu thống kê với kiểu rõ ràng
            var khuVucStats = baiViets
                .GroupBy(x => x.KhuVuc)
                .Select(g => new { KhuVuc = g.Key ?? "Không xác định", Count = g.Count() })
                .OrderBy(x => x.KhuVuc)
                .ToList();

            var trangThaiStats = baiViets
                .GroupBy(x => x.TrangThai)
                .Select(g => new { TrangThai = g.Key ?? "Không xác định", Count = g.Count() })
                .OrderBy(x => x.TrangThai)
                .ToList();

            var gioiTinhStats = new
            {
                Labels = new List<string> { "Nam", "Nữ", "Khác" },
                Counts = new List<int>
                {
                    baiViets.Count(x => x.GioiTinh == 1),
                    baiViets.Count(x => x.GioiTinh == 2),
                    baiViets.Count(x => x.GioiTinh != 1 && x.GioiTinh != 2)
                }
            };

            // Lấy danh sách các giá trị distinct cho dropdown
            var allKhuVuc = _context.TimNguois.Select(x => x.KhuVuc).Distinct().ToList();
            var allTrangThai = _context.TimNguois.Select(x => x.TrangThai).Distinct().ToList();

            // Truyền dữ liệu sang view
            ViewBag.AllKhuVuc = allKhuVuc;
            ViewBag.AllTrangThai = allTrangThai;
            ViewBag.Year = year;
            ViewBag.Month = month;
            ViewBag.Day = day;
            ViewBag.KhuVuc = khuVuc;
            ViewBag.TrangThai = trangThai;
            ViewBag.Tuan = tuan;
            ViewBag.NgayLabels = ngayLabels;
            ViewBag.BaiVietTheoNgay = baiVietTheoNgay;
            ViewBag.KhuVucStats = khuVucStats;
            ViewBag.TrangThaiStats = trangThaiStats;
            ViewBag.GioiTinhStats = gioiTinhStats;

            // Xử lý AJAX request
            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new
                {
                    khuVucs = khuVucStats.Select(x => x.KhuVuc).ToList(),
                    soLuongsKhuVuc = khuVucStats.Select(x => x.Count).ToList(),
                    trangThais = trangThaiStats.Select(x => x.TrangThai).ToList(),
                    soLuongsTrangThai = trangThaiStats.Select(x => x.Count).ToList(),
                    soLuongsGioiTinh = gioiTinhStats.Counts,
                    baiVietTheoNgay = baiVietTheoNgay,
                    ngayLabels = ngayLabels,
                    label = GetTimeLabel(year, month, day, tuan)
                });
            }

            return View(baiViets);
        }

        private string GetTimeLabel(int? year, int? month, int? day, int? tuan)
        {
            if (day.HasValue && month.HasValue && year.HasValue)
                return $"{day.Value:D2}/{month.Value:D2}/{year.Value}";
            if (month.HasValue && year.HasValue)
                return $"Tháng {month.Value:D2}/{year.Value}";
            if (year.HasValue)
                return $"Năm {year.Value}";
            if (tuan.HasValue)
                return $"{tuan.Value} tuần gần nhất";
            return "7 ngày gần nhất";
        }
    }
}