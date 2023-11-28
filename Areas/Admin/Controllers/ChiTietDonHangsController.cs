using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyPhamCheilinus.Models;
using PagedList.Core;

namespace MyPhamCheilinus.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChiTietDonHangsController : Controller
    {
        private readonly _2023MyPhamContext _context;
        public INotyfService _notifyService { get; }

        public ChiTietDonHangsController(_2023MyPhamContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: Admin/ChiTietDonHangs

        public IActionResult Index(int? page, string? MaKH = "")
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;

            IQueryable<ChiTietDonHang> query = _context.ChiTietDonHangs
                .AsNoTracking()
                .Include(c => c.MaDonHangNavigation).Include(c => c.MaSanPhamNavigation).Include(c => c.MaDonHangNavigation.MaKhachHangNavigation);



            var lsDonHangs = query.OrderByDescending(x => x.MaDonHangNavigation.NgayDatHang);

            PagedList<ChiTietDonHang> models = new PagedList<ChiTietDonHang>(lsDonHangs, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;

            //ViewData["KhachHang"] = new SelectList(_context.DanhMucSanPhams, "MaKhachHang", "TenKhachHang", MaKH);

            return View(models);
        }
        // GET: Admin/ChiTietDonHangs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.ChiTietDonHangs == null)
            {
                return NotFound();
            }

            var chiTietDonHang = await _context.ChiTietDonHangs
                .Include(c => c.MaDonHangNavigation)
                .Include(c => c.MaSanPhamNavigation)
                 .Include(c => c.MaDonHangNavigation.MaKhachHangNavigation)
                .FirstOrDefaultAsync(m => m.MaDonHang == id);
            if (chiTietDonHang == null)
            {
                return NotFound();
            }
            var chitietdonhang= _context.ChiTietDonHangs.Include(c => c.MaDonHangNavigation)
                .Include(c => c.MaSanPhamNavigation)
                .Include(c => c.MaDonHangNavigation.MaKhachHangNavigation)
                .AsNoTracking().Where(x=>x.MaDonHang == chiTietDonHang.MaDonHang)
                .OrderBy(x=>x.MaDonHang).ToList();
            ViewBag.ChiTiet = chitietdonhang;
            //string fulladdress = $"{chitietdonhang.Ma}"
            return View(chiTietDonHang);
        }

        public IActionResult ChiTietDonHang()
        {
            var donHangList = _context.ChiTietDonHangs.Include(c => c.MaDonHangNavigation)
                .Include(c => c.MaSanPhamNavigation)
                 .Include(c => c.MaDonHangNavigation.MaKhachHangNavigation)
           .ToList();

            return View(donHangList);
        }
        [HttpPost]
        public IActionResult UpdateStatus(string donHangId, int trangThaiDonHang)
        {
            var donHang = _context.ChiTietDonHangs.Find(donHangId);
            if (donHang == null)
            {
                return NotFound();
            }

            donHang.MaDonHangNavigation.TrangThaiDonHang = trangThaiDonHang;
            _context.SaveChanges();

            return Ok();
        }

        // POST: Admin/ChiTietDonHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDonHang,MaSanPham,SoLuong,GiaBan")] ChiTietDonHang chiTietDonHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chiTietDonHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaDonHang"] = new SelectList(_context.DonHangs, "MaDonHang", "MaDonHang", chiTietDonHang.MaDonHang);
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "MaSanPham", chiTietDonHang.MaSanPham);
            return View(chiTietDonHang);
        }

        public async Task<IActionResult> ChangeStatus(string id, string? id1)
        {
            if (id == null || _context.ChiTietDonHangs == null)
            {
                return NotFound();
            }

            var chiTietDonHang = await _context.ChiTietDonHangs
                 .Include(c => c.MaDonHangNavigation)
                 .Include(c => c.MaSanPhamNavigation)
                 .Include(c => c.MaDonHangNavigation.MaKhachHangNavigation)
                 .FirstOrDefaultAsync(m => m.MaDonHang == id);

            if (chiTietDonHang == null)
            {
                return NotFound();
            }

            List<SelectListItem> trangThaiList = new List<SelectListItem>
    {
        new SelectListItem { Text = "Chờ xử lý", Value = "1" },
        new SelectListItem { Text = "Đang xử lý", Value = "2" },
        new SelectListItem { Text = "Hoàn thành", Value = "3" },
        new SelectListItem { Text = "Đã hủy", Value = "4" }
        // Thêm các trạng thái khác nếu cần
    };

            ViewData["TrangThai"] = new SelectList(trangThaiList, "Value", "Text", chiTietDonHang.MaDonHangNavigation.TrangThaiDonHang.ToString());
            ViewData["MaDonHang"] = new SelectList(_context.DonHangs, "MaDonHang", "MaDonHang", chiTietDonHang.MaDonHang);
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "MaSanPham", chiTietDonHang.MaSanPham);
            ViewData["MaDonHangNavigation"] = new SelectList(_context.DonHangs, "MaDonHangNavigation", "MaDonHangNavigation", chiTietDonHang.MaDonHangNavigation);
            ViewData["MaSanPhamNavigation"] = new SelectList(_context.SanPhams, "MaSanPhamNavigation", "MaSanPhamNavigation", chiTietDonHang.MaSanPhamNavigation);
            ViewBag.Currentid = id;
            ViewBag.Currentid1 = id1;
 

            return View(chiTietDonHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(string id, string? id1, [Bind("MaDonHang,MaSanPham,SoLuong,GiaBan")] ChiTietDonHang chiTietDonHang)
        {
            if (id != chiTietDonHang.MaDonHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy chi tiết đơn hàng và include MaDonHangNavigation và MaSanPhamNavigation
                    var ctDonHang = await _context.ChiTietDonHangs
                        .Include(c => c.MaDonHangNavigation)
                        .Include(c => c.MaSanPhamNavigation)
                        .FirstOrDefaultAsync(m => m.MaDonHang == id);

                    if (ctDonHang != null)
                    {
                        if (int.TryParse(ModelState["MaDonHangNavigation.TrangThaiDonHang"].AttemptedValue, out int trangThai))
                        {
                            // Cập nhật trạng thái đơn hàng
                            ctDonHang.MaDonHangNavigation.TrangThaiDonHang = trangThai;
                        }

                        _context.Update(ctDonHang);
                        await _context.SaveChangesAsync();
                        _notifyService.Success("Cập nhật trạng thái thành công");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietDonHangExists(chiTietDonHang.MaDonHang))
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

            // Trong trường hợp có lỗi, bạn có thể in các lỗi ModelState để kiểm tra
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }
            }

            // Nếu có lỗi, hiển thị trang cập nhật với thông báo lỗi
            List<SelectListItem> trangThaiList = new List<SelectListItem>
            {
               new SelectListItem { Text = "Chờ xử lý", Value = "1" },
             new SelectListItem { Text = "Đang xử lý", Value = "2" },
        new SelectListItem { Text = "Hoàn thành", Value = "3" },
        new SelectListItem { Text = "Đã hủy", Value = "4" }
        // Thêm các trạng thái khác nếu cần
    };

            ViewData["TrangThai"] = new SelectList(trangThaiList, "Value", "Text", ModelState["MaDonHangNavigation.TrangThaiDonHang"].AttemptedValue);
            ViewData["MaDonHang"] = new SelectList(_context.DonHangs, "MaDonHang", "MaDonHang", chiTietDonHang.MaDonHang);
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "MaSanPham", chiTietDonHang.MaSanPham);
            ViewData["MaDonHangNavigation"] = new SelectList(_context.DonHangs, "MaDonHangNavigation", "MaDonHangNavigation", chiTietDonHang.MaDonHangNavigation);
            ViewData["MaSanPhamNavigation"] = new SelectList(_context.SanPhams, "MaSanPhamNavigation", "MaSanPhamNavigation", chiTietDonHang.MaSanPhamNavigation);
            ViewBag.Currentid = id;
            ViewBag.Currentid1 = id1;
       
            return View(chiTietDonHang);
        }

        // GET: Admin/ChiTietDonHangs/Edit/5
        public async Task<IActionResult> Edit(string id, string id1)
        {
            if (id == null || _context.ChiTietDonHangs == null)
            {
                return NotFound();
            }

            var chiTietDonHang = await _context.ChiTietDonHangs.FindAsync(id, id1);

            if (chiTietDonHang == null)
            {
                return NotFound();
            }
            ViewData["MaDonHang"] = new SelectList(_context.DonHangs, "MaDonHang", "MaDonHang", chiTietDonHang.MaDonHang);
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "MaSanPham", chiTietDonHang.MaSanPham);
            return View(chiTietDonHang);
        }

        // POST: Admin/ChiTietDonHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string id1,[Bind("MaDonHang,MaSanPham,SoLuong,GiaBan")] ChiTietDonHang chiTietDonHang)
        {
            if (id != chiTietDonHang.MaDonHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chiTietDonHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietDonHangExists(chiTietDonHang.MaDonHang))
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
            ViewData["MaDonHang"] = new SelectList(_context.DonHangs, "MaDonHang", "MaDonHang", chiTietDonHang.MaDonHang);
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "MaSanPham", chiTietDonHang.MaSanPham);
            return View(chiTietDonHang);
        }

        // GET: Admin/ChiTietDonHangs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.ChiTietDonHangs == null)
            {
                return NotFound();
            }

            var chiTietDonHang = await _context.ChiTietDonHangs
                .Include(c => c.MaDonHangNavigation)
                .Include(c => c.MaSanPhamNavigation)
                .FirstOrDefaultAsync(m => m.MaDonHang == id);
            if (chiTietDonHang == null)
            {
                return NotFound();
            }

            return View(chiTietDonHang);
        }

        // POST: Admin/ChiTietDonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.ChiTietDonHangs == null)
            {
                return Problem("Entity set '_2023MyPhamContext.ChiTietDonHangs'  is null.");
            }
            var chiTietDonHang = await _context.ChiTietDonHangs.FindAsync(id);
            if (chiTietDonHang != null)
            {
                _context.ChiTietDonHangs.Remove(chiTietDonHang);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChiTietDonHangExists(string id)
        {
          return (_context.ChiTietDonHangs?.Any(e => e.MaDonHang == id)).GetValueOrDefault();
        }
    }
}
