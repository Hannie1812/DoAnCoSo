﻿using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Claims;
using WebTimNguoiThatLac.BoTro;
using WebTimNguoiThatLac.Data;
using WebTimNguoiThatLac.Models;
using WebTimNguoiThatLac.Services;
using X.PagedList.Extensions;

namespace WebTimNguoiThatLac.Controllers
{
    public class TimNguoiController : Controller
    {
        private ApplicationDbContext db;
        
        private UserManager<ApplicationUser> userManager;

        private readonly EmailService _emailService;

        private readonly ILogger<TimNguoiController> _logger;

        private static readonly IEnumerable<string> TinhThanhIEnumerable = new List<string>
    {
        "Hà Nội",
        "Hồ Chí Minh",
        "Đà Nẵng",
        "Hải Phòng",
        "Cần Thơ",
        "An Giang",
        "Bà Rịa - Vũng Tàu",
        "Bắc Giang",
        "Bắc Kạn",
        "Bạc Liêu",
        "Bắc Ninh",
        "Bến Tre",
        "Bình Định",
        "Bình Dương",
        "Bình Phước",
        "Bình Thuận",
        "Cà Mau",
        "Cao Bằng",
        "Đắk Lắk",
        "Đắk Nông",
        "Điện Biên",
        "Đồng Nai",
        "Đồng Tháp",
        "Gia Lai",
        "Hà Giang",
        "Hà Nam",
        "Hà Tĩnh",
        "Hải Dương",
        "Hậu Giang",
        "Hòa Bình",
        "Hưng Yên",
        "Khánh Hòa",
        "Kiên Giang",
        "Kon Tum",
        "Lai Châu",
        "Lâm Đồng",
        "Lạng Sơn",
        "Lào Cai",
        "Long An",
        "Nam Định",
        "Nghệ An",
        "Ninh Bình",
        "Ninh Thuận",
        "Phú Thọ",
        "Quảng Bình",
        "Quảng Nam",
        "Quảng Ngãi",
        "Quảng Ninh",
        "Quảng Trị",
        "Sóc Trăng",
        "Sơn La",
        "Tây Ninh",
        "Thái Bình",
        "Thái Nguyên",
        "Thanh Hóa",
        "Thừa Thiên Huế",
        "Tiền Giang",
        "Trà Vinh",
        "Tuyên Quang",
        "Vĩnh Long",
        "Vĩnh Phúc",
        "Yên Bái",
        "Phú Yên"
    };

        public  TimNguoiController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, EmailService emailService, ILogger<TimNguoiController> logger)
        {
            this.db = db;
            this.userManager = userManager;
            _emailService = emailService;
            _logger = logger;
        }



        public IActionResult Index(string ten, string khuVuc, string dacDiem, int page = 1)
        {
            int pageSize = 6; // Số bài viết mỗi trang

            var query = db.TimNguois
                .Include(u => u.ApplicationUser)
                .Include(u => u.AnhTimNguois)
                .Where(i => i.active == true);

            // Áp dụng bộ lọc tên
            if (!string.IsNullOrEmpty(ten))
            {
                query = query.Where(x => x.HoTen.Contains(ten) || x.TieuDe.Contains(ten));
            }

            // Áp dụng bộ lọc khu vực
            if (!string.IsNullOrEmpty(khuVuc))
            {
                query = query.Where(x => x.KhuVuc.Contains(khuVuc));
            }

            // Áp dụng bộ lọc đặc điểm nhận dạng
            if (!string.IsNullOrEmpty(dacDiem))
            {
                query = query.Where(x => x.DaciemNhanDang.Contains(dacDiem));
            }

            // Lưu các giá trị filter vào ViewBag
            ViewBag.TenFilter = ten;
            ViewBag.KhuVucFilter = khuVuc;
            ViewBag.DacDiemFilter = dacDiem;
            ViewBag.TinhThanhList = new SelectList(TinhThanhIEnumerable);

            // Sắp xếp và phân trang
            var pagedList = query.OrderByDescending(x => x.Id)
                                .ToPagedList(page, pageSize);

            return View(pagedList);
        }
        public async Task<IActionResult> ThemNguoiCanTim()
        {
            ViewBag.DanhSachTinhThanh = TinhThanhIEnumerable;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemNguoiCanTim(TimNguoi timNguoi, List<IFormFile> DSHinhAnhCapNhat)
        {
            if (ModelState.IsValid)
            {
                if(DSHinhAnhCapNhat == null)
                {
                    ModelState.AddModelError("Lỗi", "Chưa Có Hình Ảnh");
                    ViewBag.DanhSachTinhThanh = TinhThanhIEnumerable;
                    return View(timNguoi);
                }
                timNguoi.active = false;
                db.Add(timNguoi);
                await db.SaveChangesAsync();
                int d = 0;
                foreach(IFormFile i in DSHinhAnhCapNhat)
                {
                    AnhTimNguoi x = new AnhTimNguoi();
                    x.IdNguoiCanTim = timNguoi.Id;
                    if(d==0)
                    {
                        x.TrangThai = 1;
                        d++;
                    }
                    else
                    {
                        x.TrangThai = 0;
                    }
                    x.HinhAnh = await SaveImage(i, "AnhNguoiCanTim");
                    db.AnhTimNguois.Add(x);
                    await db.SaveChangesAsync();
                }
                return RedirectToAction("ThongTinCaNhan", "NguoiDung");
            }

            ViewBag.DanhSachTinhThanh = TinhThanhIEnumerable;
            return View(timNguoi);
        }

        public async Task<string> SaveImage(IFormFile ImageURL, string subFolder)
        {
            if (ImageURL == null || ImageURL.Length == 0)
            {
                throw new ArgumentException("File không hợp lệ!");
            }

            // Đường dẫn thư mục lưu ảnh trong wwwroot/uploads/tin-tuc
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", subFolder);

            // Tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Tạo tên file duy nhất để tránh trùng lặp
            string fileExtension = Path.GetExtension(ImageURL.FileName);
            string fileName = Path.GetFileNameWithoutExtension(ImageURL.FileName);
            string uniqueFileName = fileName + "_" + Guid.NewGuid().ToString("N") + fileExtension;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Lưu file vào thư mục
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await ImageURL.CopyToAsync(fileStream);
            }

            // Trả về đường dẫn tương đối để hiển thị ảnh trên web
            return $"/uploads/{subFolder}/{uniqueFileName}";
        }


        public void DeleteImage(string ImageURL, string subFolder)
        {
            if (string.IsNullOrEmpty(ImageURL))
            {
                throw new ArgumentException("Đường dẫn ảnh không hợp lệ!");
            }

            // Lấy đường dẫn tuyệt đối của ảnh trong thư mục wwwroot/uploads/
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", subFolder);
            string filePath = Path.Combine(uploadsFolder, Path.GetFileName(ImageURL));

            // Kiểm tra nếu file tồn tại thì xóa
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }


        public async Task<IActionResult> ChiTietBaiTimNguoi(int id)
        {
            if(User.Identity.IsAuthenticated == false)
            {
                return Redirect("/Identity/Account/login");
            }
            ApplicationUser x = await userManager.GetUserAsync(User);
            if(x != null)
            {
                
                TimNguoi y = db.TimNguois.Include(u => u.AnhTimNguois).FirstOrDefault(i => i.Id == id);
                ApplicationUser us = await userManager.FindByIdAsync(y.IdNguoiDung);
                ViewBag.NguoiTim = us;
                ViewBag.DSHinhAnh = await db.AnhTimNguois
                                                        .Where(i => i.IdNguoiCanTim == y.Id)
                                                        .ToListAsync();
                List<BinhLuan> DSBinhLuan = db.BinhLuans
                                                        .Include(u => u.ApplicationUser)
                                                        .Where(i => i.IdBaiViet ==  id && i.Active == true)
                                                        .OrderByDescending(z => z.NgayBinhLuan)
                                                        .ToList();
                ViewBag.DSBinhLuan = DSBinhLuan;
                return View(y);
            }
            return RedirectToAction("ThongTinCaNhan", "NguoiDung");
        }

        [HttpPost]
        public async Task<IActionResult> ThemBinhLuan(int IdBaiViet,string UserId,string NoiDung, IFormFile? HinhAnh)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy ID của người dùng hiện tại
                                                                             // Lấy thông tin bài viết
                var baiViet = await db.TimNguois
                    .Include(x => x.ApplicationUser)
                    .FirstOrDefaultAsync(x => x.Id == IdBaiViet);

                if(baiViet == null || baiViet.ApplicationUser.Email == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                // Xử lý upload hình ảnh
                if (ModelState.IsValid)
                {

                    BinhLuan x = new BinhLuan();
                    if(HinhAnh != null)
                    {
                        x.HinhAnh = await SaveImage(HinhAnh, "BinhLuan");
                    }
                    x.NoiDung = NoiDung;
                    x.UserId = userId;
                    x.IdBaiViet = IdBaiViet;
                    x.DaDoc = false;

                    db.BinhLuans.Add(x);

                   

                    await db.SaveChangesAsync();

                    try
                    {
                        await _emailService.SendEmailAsync(baiViet.ApplicationUser.Email, ThongTinEmail.TieuDeBinhLuanMoi, ThongTinEmail.NoiDungBinhLuanMoi(x.NgayBinhLuan, baiViet.HoTen, x.ApplicationUser?.FullName, x.NoiDung));
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Error = $"Lỗi khi gửi email: {ex.Message}";
                        return Json(new { success = false, message = "Lỗi: Email đã được gửi Thất Bại! " + ex.Message });
                    }

                    return RedirectToAction("ChiTietBaiTimNguoi", new { id = IdBaiViet });
                }
                return RedirectToAction("ChiTietBaiTimNguoi", new { id = IdBaiViet }); // Quay lại trang chi tiết

               
            }
            ModelState.AddModelError("Lỗi", "Chưa Đăng Nhập");
            return RedirectToAction("ChiTietBaiTimNguoi", new { id = IdBaiViet }); // Quay lại trang chi tiết
        }

        public async Task<IActionResult> CapNhatBaiViet(int id)
        {
            if(User.Identity.IsAuthenticated)
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                TimNguoi x = db.TimNguois
                                    .Include(u => u.ApplicationUser)
                                    .Include(u => u.AnhTimNguois)
                                    .Include(u => u.BinhLuans)
                                    .FirstOrDefault(i => i.Id ==  id);
                if(x.IdNguoiDung == userid)
                {
                    ViewBag.DanhSachTinhThanh = TinhThanhIEnumerable;
                    ViewBag.DanhSachHinhAnh = db.AnhTimNguois.Where(i => i.IdNguoiCanTim == id).ToList();
                    return View(x);
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            ViewBag.DanhSachTinhThanh = TinhThanhIEnumerable;
            ViewBag.DanhSachHinhAnh = db.AnhTimNguois.Where(i => i.IdNguoiCanTim == id).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CapNhatBaiViet(TimNguoi x, List<IFormFile>? DSHinhAnhCapNhat)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            TimNguoi y = db.TimNguois
                                .Include(u => u.ApplicationUser)
                                .Include(u => u.AnhTimNguois)
                                .Include(u => u.BinhLuans)
                                .FirstOrDefault(i => i.Id == x.Id);

            if (y == null || y.IdNguoiDung != userid)
            {
                return RedirectToAction("Login", "Account");
            }

            // Luôn thiết lập ViewBag trước khi trả về View
            ViewBag.DanhSachTinhThanh = TinhThanhIEnumerable;
            ViewBag.DanhSachHinhAnh = db.AnhTimNguois.Where(i => i.IdNguoiCanTim == x.Id).ToList();

            if (!ModelState.IsValid)
            {
                return View(x);
            }

            // Cập nhật thông tin cơ bản (luôn thực hiện dù có ảnh hay không)
            y.MoTa = x.MoTa;
            y.TieuDe = x.TieuDe;
            y.DaciemNhanDang = x.DaciemNhanDang;
            y.GioiTinh = x.GioiTinh;
            y.TrangThai = x.TrangThai;
            y.KhuVuc = x.KhuVuc;

            // Chỉ xử lý ảnh nếu có ảnh mới được chọn
            if (DSHinhAnhCapNhat != null && DSHinhAnhCapNhat.Count > 0)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var dsHinhAnh = db.AnhTimNguois.Where(i => i.IdNguoiCanTim == x.Id).ToList();

                        if (dsHinhAnh != null && dsHinhAnh.Count > 0)
                        {
                            foreach (var i in dsHinhAnh)
                            {
                                DeleteImage(i.HinhAnh, "AnhNguoiCanTim");
                                db.AnhTimNguois.Remove(i);
                            }
                            await db.SaveChangesAsync();
                        }

                        int d = 0;
                        foreach (var i in DSHinhAnhCapNhat)
                        {
                            var z = new AnhTimNguoi
                            {
                                IdNguoiCanTim = y.Id,
                                TrangThai = (d == 0) ? 1 : 0,
                                HinhAnh = await SaveImage(i, "AnhNguoiCanTim")
                            };
                            db.AnhTimNguois.Add(z);
                            d++;
                        }

                        await db.SaveChangesAsync();
                        return RedirectToAction("ChiTietBaiTimNguoi", new { id = x.Id }); // Quay lại trang chi tiết
                    }
                    catch
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật hình ảnh");
                        return View(x);
                    }
                }
            }

            // Lưu các thay đổi khác (luôn thực hiện)
            await db.SaveChangesAsync();
            return RedirectToAction("ChiTietBaiTimNguoi", new { id = x.Id });
        }

        [HttpGet]
        public async Task<IActionResult> DemBinhLuanChuaDoc()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { count = 0 });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var count = await db.BinhLuans
                .Include(b => b.TimNguoi)
                .CountAsync(b => b.TimNguoi.IdNguoiDung == userId && !b.DaDoc);

            return Json(new { count });
        }

        // Lấy danh sách bình luận chưa đọc
        [HttpGet]
        public async Task<IActionResult> LayBinhLuanChuaDoc()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return PartialView("_DanhSachThongBao", new List<BinhLuan>());
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var comments = await db.BinhLuans
                .Include(b => b.ApplicationUser)
                .Include(b => b.TimNguoi)
                .Where(b => b.TimNguoi.IdNguoiDung == userId && !b.DaDoc)
                .OrderByDescending(b => b.NgayBinhLuan)
                .Take(10) // Giới hạn 10 thông báo mới nhất
                .ToListAsync();

            return PartialView("_DanhSachThongBao", comments);
        }

        // Đánh dấu đã đọc
        [HttpPost]
        public async Task<IActionResult> DanhDauDaDoc(int id)
        {
            var binhLuan = await db.BinhLuans.FindAsync(id);
            if (binhLuan != null)
            {
                binhLuan.DaDoc = true;
                await db.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BaoCaoBinhLuan(int MaBinhLuan, string LyDo)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập để thực hiện báo cáo" });
            }

            try
            {
                var binhLuan = await db.BinhLuans
                    .Include(b => b.TimNguoi)
                    .FirstOrDefaultAsync(b => b.Id == MaBinhLuan);

                if (binhLuan == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy bình luận" });
                }

                var currentUser = await userManager.GetUserAsync(User);

                var baoCao = new BaoCaoBinhLuan
                {
                    MaBinhLuan = MaBinhLuan,
                    MaNguoiBaoCao = currentUser.Id,
                    LyDo = LyDo,
                    NgayBaoCao = DateTime.Now
                };

                db.BaoCaoBinhLuans.Add(baoCao);
                await db.SaveChangesAsync();

                // Gửi thông báo cho admin/quản trị viên
                await _emailService.SendEmailAsync(
                    "dinhcongminh4424@gmail.com",
                    "Báo cáo bình luận mới",
                    $"Bình luận #{MaBinhLuan} trong bài viết '{binhLuan.TimNguoi.HoTen}' đã được báo cáo bởi {currentUser.Email}.\nLý do: {LyDo}"
                );

                return Json(new { success = true, message = "Báo cáo của bạn đã được gửi thành công" });
            }
            catch (Exception ex)
            {
               
                return Json(new { success = false, message = "Đã xảy ra lỗi khi gửi báo cáo" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BaoCaoBaiViet(int MaBaiViet, string LyDo,string ChiTiet, IFormFile? HinhAnhBaoCaoBaiViet)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập để thực hiện báo cáo" });
            }

            try
            {
                var currentUser = await userManager.GetUserAsync(User);
                var baiViet = await db.TimNguois.FirstOrDefaultAsync(i => i.Id == MaBaiViet);

                if (baiViet == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy bài viết" });
                }
                BaoCaoBaiViet x = new BaoCaoBaiViet();
                x.MaBaiViet = MaBaiViet;
                x.LyDo = LyDo;
                x.MaNguoiBaoCao = currentUser.Id;
                x.NgayBaoCao = DateTime.Now;
                x.ChiTiet = ChiTiet;
                if(HinhAnhBaoCaoBaiViet != null && HinhAnhBaoCaoBaiViet.Length > 0)
                {
                    x.HinhAnh = await SaveImage(HinhAnhBaoCaoBaiViet, "AnhMinhTrungBaoCaoBaiViet");
                }


                db.BaoCaoBaiViets.Add(x);
                await db.SaveChangesAsync();

                // Gửi thông báo cho admin/quản trị viên
                await _emailService.SendEmailAsync(
                    "dinhcongminh4424@gmail.com",
                    "Báo cáo bài viết mới",
                    $"Bài viết #{x.MaBaiViet} '{baiViet.HoTen}' đã được báo cáo bởi {currentUser.Email}.\nLý do: {x.LyDo}\nChi tiết: {x.ChiTiet}"
                );

                return Json(new { success = true, message = "Báo cáo của bạn đã được gửi thành công" });
            }
            catch (Exception ex)
            {
              
                return Json(new { success = false, message = "Đã xảy ra lỗi khi gửi báo cáo" });
            }
        }

      
    }

}
