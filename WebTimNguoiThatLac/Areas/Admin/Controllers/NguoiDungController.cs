using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using WebTimNguoiThatLac.Areas.Admin.Models;
using WebTimNguoiThatLac.Data;
using WebTimNguoiThatLac.Models;
using X.PagedList.Extensions;

namespace WebTimNguoiThatLac.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]// chỉ cho phép admin
    public class NguoiDungController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> roleManager;
        private SignInManager<ApplicationUser> _signInManager;

        public  NguoiDungController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            this.db = db;
            this._userManager = userManager;
            this.roleManager = roleManager;
            _signInManager = signInManager;
        }
       
        public async Task<IActionResult> Index(string TimKiem = "", int Page = 1)
        {
            IEnumerable<ApplicationUser> ds = await db.Users.ToListAsync();
            ViewBag.TimKiem = TimKiem;
            int sodongtren1trang = 5;

            if (TimKiem.IsNullOrEmpty())
            {
                var dsTrang = ds.ToPagedList(Page, sodongtren1trang);
                return View(dsTrang);
            }
            else
            {
                TimKiem = WebTimNguoiThatLac.BoTro.Filter.ChuyenCoDauThanhKhongDau(TimKiem);
                List<ApplicationUser> dsTimKiem = new List<ApplicationUser>();
                foreach (ApplicationUser i in ds)
                {
                    string r1 = WebTimNguoiThatLac.BoTro.Filter.ChuyenCoDauThanhKhongDau(i.FullName);
                    if (r1.ToUpper().Contains(TimKiem.ToUpper()))
                    {
                        dsTimKiem.Add(i);
                        continue;
                    }
                    string r2 = WebTimNguoiThatLac.BoTro.Filter.ChuyenCoDauThanhKhongDau(i.UserName);
                    if (r2.ToUpper().Contains(TimKiem.ToUpper()))
                    {
                        dsTimKiem.Add(i);
                        continue;
                    }
                    if(i.PhoneNumber != null )
                    {
                        string r3 = i.PhoneNumber;
                        if (r3.ToUpper().Contains(TimKiem.ToUpper()))
                        {
                            dsTimKiem.Add(i);
                            continue;
                        }
                    }
                    
                }
                var dsTrang = dsTimKiem.ToPagedList(Page, sodongtren1trang);
                return View(dsTrang);
            }
        }

        public async Task<IActionResult> Create()
        {
            //return RedirectToAction("Register", "Account", new { area = "Identity" });
            return RedirectToPage("/Identity/Account/Manage/Email");
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
        public async Task<IActionResult> Update(string id)
        {
            ApplicationUser x = await db.Users.FirstOrDefaultAsync(i => i.Id == id);
            if (x == null)
            {
                return RedirectToAction("Index");
            }
            List<IdentityRole> ds = await db.Roles.ToListAsync();
            var dsLTT = new SelectList(ds, "Name", "Name");
            ViewBag.DanhSachRole = dsLTT;
            var roles = await _userManager.GetRolesAsync(x);
            ViewBag.RoleHienTai = roles.Any() ? roles[0] : "";
            return View(x);
        }
      
        [HttpPost]
        public async Task<IActionResult> Update(ApplicationUser t, string? LoaiTaiKhoan)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser x = await db.Users.FirstOrDefaultAsync(i => i.Id == t.Id);
                if (x == null)
                {
                    return RedirectToAction("Index");
                }

                if (LoaiTaiKhoan != null)
                {

                    var dsrole = await _userManager.GetRolesAsync(x);
                    await _userManager.RemoveFromRolesAsync(x, dsrole);

                    await _userManager.AddToRoleAsync(x, LoaiTaiKhoan);

                }

                x.FullName = t.FullName;
                x.Address = t.Address;
                x.NgaySinh = t.NgaySinh;
                x.CCCD = t.CCCD;
                x.PhoneNumber = t.PhoneNumber;
                x.Active = t.Active;

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }


            List<IdentityRole> ds = await db.Roles.ToListAsync();
            var dsLTT = new SelectList(ds, "Id", "Name");
            ViewBag.DanhSachRole = dsLTT;
            var roles = await _userManager.GetRolesAsync(t);
            ViewBag.RoleHienTai = roles.Any() ? roles[0] : "";
            return View(t);

        }


        [HttpPost]
        public async Task<IActionResult> HoatDong(string id)
        {

            try
            {
                ApplicationUser y = await db.Users.FirstOrDefaultAsync(i => i.Id == id);
                if (y == null)
                {
                    return Json(new { success = false, message = "Ko Có Id Cần chỉnh sửa" });
                }

                y.Active = !y.Active;

                await db.SaveChangesAsync();
                return Json(new { success = true, message = "Thành Công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                ApplicationUser y = await db.Users.FirstOrDefaultAsync(i => i.Id == id);
                if (y == null)
                {
                    return Json(new { success = false, message = "Ko Có Id Cần Xóa" });
                }


                List<TimNguoi> ds = await db.TimNguois
                                                        .Include(u => u.ApplicationUser)
                                                        .Include(u => u.AnhTimNguois)
                                                        .Where(i => i.IdNguoiDung == id)
                                                        .ToListAsync();
                foreach (TimNguoi i in ds)
                {
                    // Xóa ảnh tìm người
                    List<AnhTimNguoi> dsHA = await db.AnhTimNguois.Where(m => m.IdNguoiCanTim == i.Id).ToListAsync();
                    foreach (AnhTimNguoi j in dsHA)
                    {
                        DeleteImage(j.HinhAnh, "AnhNguoiCanTim");
                        db.AnhTimNguois.Remove(j);
                        //await db.SaveChangesAsync();
                    }

                    // Xóa Bình luôanj trong bài viết
                    List<BinhLuan> dsBL = await db.BinhLuans.Where(m => m.IdBaiViet == i.Id).ToListAsync();
                    foreach (BinhLuan z in dsBL)
                    {
                        if (z.HinhAnh != null)
                        {
                            DeleteImage(z.HinhAnh, "BinhLuan");
                        }
                        db.BinhLuans.Remove(z);
                        //await db.SaveChangesAsync();
                    }

                    db.TimNguois.Remove(i);
                    //await db.SaveChangesAsync();
                }

                // Xóa toàm bộ bình luận người dùng
                List<BinhLuan> BLUS = await db.BinhLuans
                                                     .Include(u => u.ApplicationUser)
                                                     .Where(i => i.UserId == y.Id)
                                                     .ToListAsync();

                foreach (BinhLuan j in BLUS)
                {
                    if (j.HinhAnh != null)
                    {
                        DeleteImage(j.HinhAnh, "BinhLuan");
                    }
                    db.BinhLuans.Remove(j);
                }



                var usrole = await _userManager.GetRolesAsync(y);

                if (usrole != null)
                {
                    await _userManager.RemoveFromRolesAsync(y, usrole);
                }

                db.Users.Remove(y);


                await db.SaveChangesAsync();

                return Json(new { success = true, message = "Thành Công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi từ nhà cung cấp: {remoteError}");
                return RedirectToAction(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // Nếu user chưa có tài khoản → tạo mới
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (email != null)
                {
                    var user = new ApplicationUser { UserName = email, Email = email };
                    var createResult = await _userManager.CreateAsync(user);
                    if (createResult.Succeeded)
                    {
                        await _userManager.AddLoginAsync(user, info);
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }

                // Nếu thất bại
                ViewBag.ErrorTitle = "Không thể đăng nhập bằng tài khoản ngoài.";
                return View("Error");
            }
        }

    }
}