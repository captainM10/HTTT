using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyPhamCheilinus.Areas.Admin.Models;
using MyPhamCheilinus.Extension;
using MyPhamCheilinus.Helpper;
using MyPhamCheilinus.Models;

namespace MyPhamCheilinus.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]
    public class AccountsController : Controller
    {
        private readonly _2023MyPhamContext _context;
        public INotyfService _notifyService { get; }

        public AccountsController(_2023MyPhamContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: Admin/Accounts
        public async Task<IActionResult> Index()
        {

            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "Description");

            List<SelectListItem> lsTrangThai = new List<SelectListItem>();
            lsTrangThai.Add(new SelectListItem() { Text = "Active", Value = "1" });
            lsTrangThai.Add(new SelectListItem() { Text = "Block", Value = "0" });
            ViewData["lsTrangThai"] = lsTrangThai;


            var _2023MyPhamContext = _context.Accounts.Include(a => a.RoleNavigation);
            return View(await _2023MyPhamContext.ToListAsync());
        }

        // GET: Admin/Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.RoleNavigation)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Admin/Accounts/Create
        public IActionResult Create()
        {
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            return View();
        }

        // POST: Admin/Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,Phone,AccountEmail,AccountPassword,Sail,Active,FullName,RoleId,LastLogin,CreateDate")] Account account)
        {
            if (ModelState.IsValid)
            {
                string salt = Utilities.GetRandomKey();
                account.Sail = salt;
                //Tạo ngẫu nhiên mật khẩu
                account.AccountPassword = (account.Phone + salt.Trim()).ToMD5();
                account.CreateDate = DateTime.Now;

                _context.Add(account);
                await _context.SaveChangesAsync();
                _notifyService.Success("Tạo mới tài khoản quản trị thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "RoleName", account.RoleId);
            return View(account);
        }
        //ChangePassword
        public IActionResult ChangePassword()
        {
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var taikhoan = _context.Accounts.AsNoTracking().SingleOrDefault(x => x.AccountEmail == model.Email);
                if (taikhoan == null) return RedirectToAction("Login", "Accounts");

                var pass = (model.PasswordNow.Trim() + taikhoan.Sail.Trim()).ToMD5();
                if (pass == taikhoan.AccountPassword)
                {
                    string passnew = (model.Password.Trim() + taikhoan.Sail.Trim()).ToMD5();
                    taikhoan.AccountPassword = passnew;
                    taikhoan.LastLogin = DateTime.Now;
                    _context.Update(taikhoan);
                    _context.SaveChanges();
                    _notifyService.Success("Thay đổi mật khẩu thành công");
                    RedirectToAction("Login", "Accounts", new {Area = "Admin"});
                }
            }


            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            return View();
        }

        // GET: Admin/Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "RoleName", account.RoleId);
            return View(account);
        }

        // POST: Admin/Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,Phone,AccountEmail,AccountPassword,Sail,Active,FullName,RoleId,LastLogin,CreateDate")] Account account)
        {
            if (id != account.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();

                    // Kiểm tra xem tài khoản đang được chỉnh sửa có phải là tài khoản hiện tại không
                    if (id == int.Parse(HttpContext.User.FindFirst("AccountId")?.Value))
                    {
                        // Đăng xuất
                        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                        // Thêm lại các claim mới tương ứng với quyền truy cập mới của tài khoản
                        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, account.FullName),
        new Claim("AccountId", account.AccountId.ToString())
    };

                        if (account.RoleId == GetRoleIdForAdmin())
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                        }
                        if (account.RoleId == GetRoleIdForCustomer())
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "Customer"));
                        }
                        if (account.RoleId == GetRoleIdForEmployee())
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "Employee"));
                        }

                        // Đăng nhập lại
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "RoleName", account.RoleId);
            return View(account);
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
        // GET: Admin/Accounts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.RoleNavigation)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Admin/Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set '_2023MyPhamContext.Accounts'  is null.");
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
          return (_context.Accounts?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }

    }
}
