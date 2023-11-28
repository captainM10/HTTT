using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using MyPhamCheilinus.Helpper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyPhamCheilinus.Models;
using PagedList.Core;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace MyPhamCheilinus.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoaisController : Controller
    {
        private readonly _2023MyPhamContext _context;
        public INotyfService _notifyService { get; }

        public LoaisController(_2023MyPhamContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: Admin/Loais
        public IActionResult Index(int? page, string search = "")
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;

            IQueryable<Loai> query = _context.Loais
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.TenLoai.Contains(search));
            }

            var lsProducts = query.OrderByDescending(x => x.MaLoai).ToList();
            PagedList<Loai> models = new PagedList<Loai>(lsProducts.AsQueryable(), pageNumber, pageSize);
     
            ViewBag.CurrentPage = pageNumber;
            ViewBag.CurrentSearch = search;


            return View(models);
        }
        public IActionResult Filtter(string search)
        {
            var url = "/Admin/Loais?";

            if (!string.IsNullOrEmpty(search))
            {
                url += $"search={search}&";
            }

            // Loại bỏ dấu '&' cuối cùng nếu có
            if (url.EndsWith("&"))
            {
                url = url.Substring(0, url.Length - 1);
            }

            return Json(new { status = "success", redirectUrl = url });
        }
        // GET: Admin/Loais/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Loais == null)
            {
                return NotFound();
            }

            var loai = await _context.Loais
                .FirstOrDefaultAsync(m => m.MaLoai == id);
            if (loai == null)
            {
                return NotFound();
            }

            return View(loai);
        }

        // GET: Admin/Loais/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Loais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLoai,TenLoai")] Loai loai)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loai);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loai);
        }

        // GET: Admin/Loais/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Loais == null)
            {
                return NotFound();
            }

            var loai = await _context.Loais.FindAsync(id);
            if (loai == null)
            {
                return NotFound();
            }
            return View(loai);
        }

        // POST: Admin/Loais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaLoai,TenLoai")] Loai loai)
        {
            if (id != loai.MaLoai)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loai);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiExists(loai.MaLoai))
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
            return View(loai);
        }

        // GET: Admin/Loais/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Loais == null)
            {
                return NotFound();
            }

            var loai = await _context.Loais
                .FirstOrDefaultAsync(m => m.MaLoai == id);
            if (loai == null)
            {
                return NotFound();
            }

            return View(loai);
        }

        // POST: Admin/Loais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Loais == null)
            {
                return Problem("Entity set '_2023MyPhamContext.Loais'  is null.");
            }
            var loai = await _context.Loais.FindAsync(id);
            if (loai != null)
            {
                _context.Loais.Remove(loai);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoaiExists(string id)
        {
          return (_context.Loais?.Any(e => e.MaLoai == id)).GetValueOrDefault();
        }
    }
}
