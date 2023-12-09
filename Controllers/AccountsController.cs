using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPhamCheilinus.Helpper;
using CanhGac.Models;
using MyPhamCheilinus.ModelViews;
using System.Security.Claims;
using MyPhamCheilinus.Extension;

namespace MyPhamCheilinus.Controllers
{
    public class AccountsController : Controller
    {
        private readonly CanhGacContext _context;
        public INotyfService _notifyService { get; }

        public AccountsController(CanhGacContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidatePhone(string SoDienThoai)
        {
            try
            {
                var khachhang = _context.Accounts.AsNoTracking().SingleOrDefault(x => x.Phone.ToLower() == SoDienThoai.ToLower());
                if (khachhang != null)
                    return Json(data: "Số điện thoại : " + SoDienThoai + " Đã được sử dụng");

                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }
        public IActionResult ValidateTaiKhoan(string userName)
        {
            try
            {
                var khachhang = _context.Accounts.AsNoTracking().SingleOrDefault(x => x.UserName.ToLower() == userName.ToLower());
                if (khachhang != null)
                    return Json(data: "Tên đăng nhập : " + userName + " Đã được sử dụng");

                return Json(data: true);
            }
            catch
            {

                return Json(data: true);
            }
        }
        [Route("tai-khoan-cua-toi.html", Name = "Dashboard")]
   
        public IActionResult Dashboard()
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID != null)
            {
                var khachhang = _context.Accounts.AsNoTracking().SingleOrDefault(x => x.AccountId == Convert.ToInt32(taikhoanID));
                if (khachhang != null)
                {
                    return View();
                }
            }
            return RedirectToAction("Login");
        }
        private int GetRoleIdForCustomer()
        {
            // Thực hiện logic để lấy RoleId tương ứng với vai trò 'Customer' từ cơ sở dữ liệu
            // Ví dụ:
            return _context.Roles.Where(r => r.RoleName == "Đại đội").Select(r => r.RoleId).FirstOrDefault();
        }
        private int GetRoleIdForAdmin()
        {
            // Thực hiện logic để lấy RoleId tương ứng với vai trò 'Customer' từ cơ sở dữ liệu
            // Ví dụ:
            return _context.Roles.Where(r => r.RoleName == "Tiểu đoàn").Select(r => r.RoleId).FirstOrDefault();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "Dangky")]
       
        public IActionResult DangKyTaiKhoan()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "Dangky")]
        public async Task<IActionResult> DangKyTaiKhoan(RegisterVM taikhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string salt = Utilities.GetRandomKey();
                    Account khachhang = new Account
                    {
                        //FullName = taikhoan.TenKhachHang,
                        //Phone = taikhoan.SoDienThoai.Trim().ToLower(),
                        //AccountEmail = taikhoan.Email.Trim().ToLower(),
                        //AccountPassword = (taikhoan.Password + salt.Trim()).ToMD5(),
                        //Active = true,
                        //Sail = salt,
                        //RoleId = GetRoleIdForCustomer(),
                        //CreateDate = DateTime.Now
                        TenDonVi = taikhoan.TenDonVi,
                        UserName = taikhoan.TenDangNhap.Trim().ToLower(),
                        Pasword = (taikhoan.Password + salt.Trim()).ToMD5(),
                        Phone = taikhoan.SoDienThoai.Trim().ToLower(),
                        RoleId = GetRoleIdForCustomer(),
                        Salt = salt,
                        CreateDate = DateTime.Now

                    };
                    try
                    {
                        _context.Add(khachhang);
                        await _context.SaveChangesAsync();
                        HttpContext.Session.SetString("AccountId", khachhang.AccountId.ToString());
                        var taikhoanID = HttpContext.Session.GetString("AccountId");

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, khachhang.TenDonVi),
                            new Claim("AccountId", khachhang.AccountId.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return RedirectToAction("Dashboard", "Accounts");
                    }
                    catch (Exception ex)
                    {
                        // In ra lỗi để debug
                        Console.WriteLine(ex.Message);
                        return RedirectToAction("DangKyTaiKhoan", "Accounts");
                    }
                }
                else
                {
                    return View(taikhoan);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(taikhoan);
            }
        }
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "Dangnhap")]
        public IActionResult Login(string? returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID != null)
            {
                return RedirectToAction("Dashboard", "Accounts");
            }

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "Dangnhap")]
        public async Task<IActionResult> Login(LoginViewModel customer, string? returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var canBo = _context.Accounts.AsNoTracking().SingleOrDefault(x => x.UserName.Trim() == customer.UserName);
                    if (canBo == null) return RedirectToAction("DangKyTaiKhoan");

                    string pass = (customer.Password + canBo.Salt.Trim()).ToMD5();
                    if (canBo.Pasword != pass)
                    {
                        _notifyService.Success("Thông tin đăng nhập chưa chính xác.");
                        return View(customer);
                    }
                  
                    HttpContext.Session.SetString("AccountId", canBo.AccountId.ToString());

                    var taikhoanID = HttpContext.Session.GetString("AccountId");

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, canBo.TenDonVi),
                        new Claim("AccountId", canBo.AccountId.ToString()),


                    };
                    if (canBo.RoleId == GetRoleIdForAdmin())
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Tiểu đoàn"));
                    }
                    if (canBo.RoleId == GetRoleIdForCustomer())
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Đại đội"));
                    }

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    //_notifyService.Success("Đăng nhập thành công!");
                    return RedirectToAction("Dashboard", "Accounts");
                }
            }
            catch
            {
                return RedirectToAction("DangKyTaiKhoan", "Accounts");
            }
            return View(customer);
        }

        public IActionResult ChangePassword()
        {
            return PartialView("ChangePassword");
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                var taikhoanID = HttpContext.Session.GetString("AccountId");
                if (taikhoanID == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                if (ModelState.IsValid)
                {
                    var taikhoan = _context.Accounts.Find(Convert.ToInt32(taikhoanID));
                    if (taikhoan == null) return RedirectToAction("Login", "Accounts");
                    var pass = (model.PasswordNow.Trim() + taikhoan.Salt.Trim()).ToMD5();
                    if (pass == taikhoan.Pasword)
                    {
                        string passnew = (model.Password.Trim() + taikhoan.Salt.Trim()).ToMD5();
                        taikhoan.Pasword = passnew;
                        _context.Update(taikhoan);
                        _context.SaveChanges();
                        _notifyService.Success("Thay đổi mật khẩu thành công!");
                        return RedirectToAction("ChangePassword", "Accounts");
                    }
                    else
                    {
                        _notifyService.Error("Sai mật khẩu");
                        return RedirectToAction("ChangePassword", "Accounts");
                    }
                }
            }
            catch
            {
                _notifyService.Success("Thay đổi không mật khẩu thành công");
                return RedirectToAction("ChangePassword", "Accounts");
            }
            _notifyService.Error("Mật khẩu mới không giống nhau");
            return RedirectToAction("ChangePassword", "Accounts");

        }
        [HttpGet]
        [Route("dang-xuat.html", Name = "Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("AccountId");
            return RedirectToAction("Index", "Home");
        }
    }
}
