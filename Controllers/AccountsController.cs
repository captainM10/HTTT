using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPhamCheilinus.Extension;
using MyPhamCheilinus.Helpper;
using MyPhamCheilinus.Models;
using MyPhamCheilinus.ModelViews;
using System.Security.Claims;

namespace MyPhamCheilinus.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly _2023MyPhamContext _context;
        public INotyfService _notifyService { get; }

        public AccountsController(_2023MyPhamContext context, INotyfService notifyService)
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
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateEmail(string Email)
        {
            try
            {
                var khachhang = _context.Accounts.AsNoTracking().SingleOrDefault(x => x.AccountEmail.ToLower() == Email.ToLower());
                if (khachhang != null)
                    return Json(data: "Email : " + Email + " Đã được sử dụng");

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
            return _context.Roles.Where(r => r.RoleName == "Customer").Select(r => r.RoleId).FirstOrDefault();
        }
        private int GetRoleIdForAdmin()
        {
            // Thực hiện logic để lấy RoleId tương ứng với vai trò 'Customer' từ cơ sở dữ liệu
            // Ví dụ:
            return _context.Roles.Where(r => r.RoleName == "Admin").Select(r => r.RoleId).FirstOrDefault();
        }
        private int GetRoleIdForEmployee()
        {
            // Thực hiện logic để lấy RoleId tương ứng với vai trò 'Customer' từ cơ sở dữ liệu
            // Ví dụ:
            return _context.Roles.Where(r => r.RoleName == "Employee").Select(r => r.RoleId).FirstOrDefault();
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
                        FullName = taikhoan.TenKhachHang,
                        Phone = taikhoan.SoDienThoai.Trim().ToLower(),
                        AccountEmail = taikhoan.Email.Trim().ToLower(),
                        AccountPassword = (taikhoan.Password + salt.Trim()).ToMD5(),
                        Active = true,
                        Sail = salt,
                        RoleId = GetRoleIdForCustomer(),
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
                            new Claim(ClaimTypes.Name, khachhang.FullName),
                            new Claim("AccountId", khachhang.AccountId.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return RedirectToAction("Dashboard", "Accounts");
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("DangKyTaiKhoan", "Accounts");
                    }
                }
                else
                {
                    return View(taikhoan);
                }
            }
            catch
            {
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
                    bool isEmail = Utilities.IsValidEmail(customer.UserName);
                    if (!isEmail) return View(customer);

                    var khachhang = _context.Accounts.AsNoTracking().SingleOrDefault(x => x.AccountEmail.Trim() == customer.UserName);
                    if (khachhang == null) return RedirectToAction("DangKyTaiKhoan");

                    string pass = (customer.Password + khachhang.Sail.Trim()).ToMD5();
                    if (khachhang.AccountPassword != pass)
                    {
                        _notifyService.Success("Thông tin đăng nhập chưa chính xác.");
                        return View(customer);
                    }
                    if (khachhang.Active == false)
                    {
                        return RedirectToAction("ThongBao", "Accounts");
                    }

                    HttpContext.Session.SetString("AccountId", khachhang.AccountId.ToString());

                    var taikhoanID = HttpContext.Session.GetString("AccountId");

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, khachhang.FullName),
                        new Claim("AccountId", khachhang.AccountId.ToString()),
           

                    };
                    if (khachhang.RoleId == GetRoleIdForAdmin())
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    }
                    if (khachhang.RoleId == GetRoleIdForCustomer())
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Customer"));
                    }
                    if (khachhang.RoleId == GetRoleIdForEmployee())
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Employee"));
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
