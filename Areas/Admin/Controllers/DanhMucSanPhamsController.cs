using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyPhamCheilinus.Models;
using PagedList.Core;

namespace MyPhamCheilinus.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]
    public class DanhMucSanPhamsController : Controller
    {
        private readonly _2023MyPhamContext _context;

        public INotyfService _notifyService { get; }

        public DanhMucSanPhamsController(_2023MyPhamContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: Admin/DanhMucSanPhams
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsCategorys = _context.DanhMucSanPhams
                .AsNoTracking()
                .OrderByDescending(x => x.MaDanhMuc);

            PagedList<DanhMucSanPham> models = new PagedList<DanhMucSanPham>(lsCategorys, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(models);
        }

        // GET: Admin/DanhMucSanPhams/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.DanhMucSanPhams == null)
            {
                return NotFound();
            }

            var danhMucSanPham = await _context.DanhMucSanPhams
                .Include(d => d.MaCtloaiNavigation)
                .Include(d => d.MaHangNavigation)
                .FirstOrDefaultAsync(m => m.MaDanhMuc == id);
            if (danhMucSanPham == null)
            {
                return NotFound();
            }

            return View(danhMucSanPham);
        }

        // GET: Admin/DanhMucSanPhams/Create
        public IActionResult Create()
        {
            ViewData["MaCtloai"] = new SelectList(_context.Ctloais, "MaCtloai", "MaCtloai");
            ViewData["MaHang"] = new SelectList(_context.Hangs, "MaHang", "MaHang");
            return View();
        }

        // POST: Admin/DanhMucSanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDanhMuc,TenDanhMuc,MoTa,MaCtloai,MaHang,HinhAnh,DanhGia,Gia,CachDung,ChiTiet")] DanhMucSanPham danhMucSanPham)
        {
            if (ModelState.IsValid)
            {
                _context.Add(danhMucSanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaCtloai"] = new SelectList(_context.Ctloais, "MaCtloai", "MaCtloai", danhMucSanPham.MaCtloai);
            ViewData["MaHang"] = new SelectList(_context.Hangs, "MaHang", "MaHang", danhMucSanPham.MaHang);
            return View(danhMucSanPham);
        }

        // GET: Admin/DanhMucSanPhams/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.DanhMucSanPhams == null)
            {
                return NotFound();
            }

            var danhMucSanPham = await _context.DanhMucSanPhams.FindAsync(id);
            if (danhMucSanPham == null)
            {
                return NotFound();
            }
            ViewData["MaCtloai"] = new SelectList(_context.Ctloais, "MaCtloai", "MaCtloai", danhMucSanPham.MaCtloai);
            ViewData["MaHang"] = new SelectList(_context.Hangs, "MaHang", "MaHang", danhMucSanPham.MaHang);
            return View(danhMucSanPham);
        }

        // POST: Admin/DanhMucSanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaDanhMuc,TenDanhMuc,MoTa,MaCtloai,MaHang,HinhAnh,DanhGia,Gia,CachDung,ChiTiet")] DanhMucSanPham danhMucSanPham)
        {
            if (id != danhMucSanPham.MaDanhMuc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(danhMucSanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhMucSanPhamExists(danhMucSanPham.MaDanhMuc))
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
            ViewData["MaCtloai"] = new SelectList(_context.Ctloais, "MaCtloai", "MaCtloai", danhMucSanPham.MaCtloai);
            ViewData["MaHang"] = new SelectList(_context.Hangs, "MaHang", "MaHang", danhMucSanPham.MaHang);
            return View(danhMucSanPham);
        }

        // GET: Admin/DanhMucSanPhams/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.DanhMucSanPhams == null)
            {
                return NotFound();
            }

            var danhMucSanPham = await _context.DanhMucSanPhams
                .Include(d => d.MaCtloaiNavigation)
                .Include(d => d.MaHangNavigation)
                .FirstOrDefaultAsync(m => m.MaDanhMuc == id);
            if (danhMucSanPham == null)
            {
                return NotFound();
            }

            return View(danhMucSanPham);
        }

        // POST: Admin/DanhMucSanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.DanhMucSanPhams == null)
            {
                return Problem("Entity set '_2023MyPhamContext.DanhMucSanPhams'  is null.");
            }
            var danhMucSanPham = await _context.DanhMucSanPhams.FindAsync(id);
            if (danhMucSanPham != null)
            {
                _context.DanhMucSanPhams.Remove(danhMucSanPham);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DanhMucSanPhamExists(string id)
        {
          return (_context.DanhMucSanPhams?.Any(e => e.MaDanhMuc == id)).GetValueOrDefault();
        }
    }
}
