using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CanhGac.Models;
using PagedList.Core;

namespace CanhGac.Controllers
{
    public class HocViensController : Controller
    {
        private readonly CanhGacContext _context;

        public HocViensController(CanhGacContext context)
        {
            _context = context;
        }

        // GET: HocViens
        public async Task<IActionResult> Index()
        {
            var canhGacContext = _context.HocViens.Include(h => h.MaCapBacNavigation).Include(h => h.MaChucVuNavigation).Include(h => h.MaDaiDoiNavigation);
            return View(await canhGacContext.ToListAsync());
        }

        public IActionResult HocVienTheoDaiDoi(int? page, string? MaHV = "", string? TenHV = "", string? ChucVu = "", string? CapBac = "", int? solangac = null, string? Gioitinh = "", bool? gac = true, string? madaidoi = "")
        {

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;

            IQueryable<HocVien> query = _context.HocViens.AsNoTracking()
                .Include(h => h.MaCapBacNavigation)
                .Include(h => h.MaChucVuNavigation)
                .Include(h => h.MaDaiDoiNavigation);

            if (!string.IsNullOrEmpty(madaidoi))
            {
                query = query.Where(x => x.MaDaiDoi == madaidoi);
            }
            if (!string.IsNullOrEmpty(MaHV))
            {
                query = query.Where(x => x.MaHocVien.Contains(MaHV));
            }
            if (!string.IsNullOrEmpty(TenHV))
            {
                query = query.Where(x => x.TenHocVien.Contains(TenHV));
            }
            if (!string.IsNullOrEmpty(ChucVu))
            {
                query = query.Where(x => x.MaChucVuNavigation.TenChucVu.Contains(ChucVu));
            }

            if (!string.IsNullOrEmpty(CapBac))
            {
                query = query.Where(x => x.MaCapBac == CapBac);
            }
            if (!string.IsNullOrEmpty(Gioitinh))
            {
                query = query.Where(x => x.GioiTinh.Contains(Gioitinh));
            }
            if (solangac != null)
            {
                query = query.Where(x => x.SoLanGac == solangac);
            }
            if ((bool)gac)
            {
                query = query.Where(x => x.Gac == gac);
            }
            var lsProducts = query.OrderByDescending(x => x.MaHocVien).ToList();

            PagedList<HocVien> models = new PagedList<HocVien>(lsProducts.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentMaDaiDoi = madaidoi;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.CurrentMaHV = MaHV;
            ViewBag.CurrentTenHV = TenHV;
            ViewBag.CurrentCapbac = CapBac;
            ViewBag.CurrentChucVu = ChucVu;
            ViewBag.CurrentGac = gac;
            ViewBag.CurrentSoLanGac = solangac;
            ViewBag.CurrentGioiTinh = Gioitinh;
            ViewData["GioiTinh"] = new SelectList(_context.HocViens, "GioiTinh", "GioiTinh", Gioitinh);
            ViewData["CapBac"] = new SelectList(_context.CapBacs, "MaCapBac", "TenCapBac", CapBac);
            // Thiếu dấu "." ở đây.
            ViewData["DonVi"] = new SelectList(_context.DonVis, "MaDonVi", "TenDonVi", madaidoi);

            return View(models);
        }

        public IActionResult Filtter(string? CapBac, string? TenHV, string? Gioitinh, string? madaidoi, string? MaHV)
        {
            var url = "/HocViens/HocVienTheoDaiDoi?";
            if (!string.IsNullOrEmpty(madaidoi))
            {
                url += $"MaDaiDoi={madaidoi}&";
            }
            if (MaHV != null)
            {
                url += $"MaHV={MaHV}&";
            }
            if (!string.IsNullOrEmpty(CapBac))
            {
                url += $"CapBac={CapBac}&";
            }

            if (!string.IsNullOrEmpty(TenHV))
            {
                url += $"TenHV={TenHV}&";
            }

            if (Gioitinh != null)
            {
                url += $"GioiTinh={Gioitinh}&";
            }

            // Loại bỏ dấu '&' cuối cùng nếu có
            if (url.EndsWith("&"))
            {
                url = url.Substring(0, url.Length - 1);
            }

            return Json(new { status = "success", redirectUrl = url });
        }

















        // GET: HocViens/Details/5
        public async Task<IActionResult> Details(string id, int? page, string? MaHV, string? TenHV, string? CapBac, string? Gioitinh, string? madaidoi)
        {
            if (id == null || _context.HocViens == null)
            {
                return NotFound();
            }

            var hocVien = await _context.HocViens
                .Include(h => h.MaCapBacNavigation)
                .Include(h => h.MaChucVuNavigation)
                .Include(h => h.MaDaiDoiNavigation)
                .FirstOrDefaultAsync(m => m.MaHocVien == id);
            if (hocVien == null)
            {
                return NotFound();
            }
            ViewBag.CurrentMaDaiDoi = madaidoi;
            ViewBag.CurrentPage = page;
            ViewBag.CurrentMaHV = MaHV;
            ViewBag.CurrentTenHV = TenHV;
            ViewBag.CurrentCapbac = CapBac;
            ViewBag.CurrentGioiTinh = Gioitinh;
            return View(hocVien);
        }

        // GET: HocViens/Create
        public IActionResult Create(int? page, string? MaHV, string? TenHV, string? CapBac, string? Gioitinh, string? madaidoi)
        {
            ViewData["MaCapBac"] = new SelectList(_context.CapBacs, "MaCapBac", "MaCapBac");
            ViewData["MaChucVu"] = new SelectList(_context.ChucVus, "MaChucVu", "MaChucVu");
            ViewData["MaDaiDoi"] = new SelectList(_context.DonVis, "MaDonVi", "MaDonVi");
            ViewBag.CurrentMaDaiDoi = madaidoi;
            ViewBag.CurrentPage = page;
            ViewBag.CurrentMaHV = MaHV;
            ViewBag.CurrentTenHV = TenHV;
            ViewBag.CurrentCapbac = CapBac;
            ViewBag.CurrentGioiTinh = Gioitinh;
            return View();
        }

        // POST: HocViens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? page, [Bind("MaHocVien,TenHocVien,NgaySinh,MaDaiDoi,GioiTinh,Gac,SoLanGac,MaCapBac,MaChucVu")] HocVien hocVien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hocVien);
                await _context.SaveChangesAsync();
                return RedirectToAction("HocVienTheoDaiDoi", new { page = page});
            }
            ViewData["CapBac"] = new SelectList(_context.CapBacs, "MaCapBac", "TenCapBac", hocVien.MaCapBac);
            ViewData["ChucVu"] = new SelectList(_context.ChucVus, "MaChucVu", "TenChucVu", hocVien.MaChucVu);
            ViewData["MaDaiDoi"] = new SelectList(_context.DonVis, "MaDonVi", "MaDonVi", hocVien.MaDaiDoi);

            return View(hocVien);
        }

        // GET: HocViens/Edit/5
        public async Task<IActionResult> Edit(string id, int? page, string? MaHV, string? TenHV, string? CapBac, string? Gioitinh, string? madaidoi)
        {
            if (id == null || _context.HocViens == null)
            {
                return NotFound();
            }

            var hocVien = await _context.HocViens.FindAsync(id);
            if (hocVien == null)
            {
                return NotFound();
            }
            ViewData["CapBac"] = new SelectList(_context.CapBacs, "MaCapBac", "TenCapBac", hocVien.MaCapBac);
            ViewData["ChucVu"] = new SelectList(_context.ChucVus, "MaChucVu", "TenChucVu", hocVien.MaChucVu);
            ViewData["MaDaiDoi"] = new SelectList(_context.DonVis, "MaDonVi", "MaDonVi", hocVien.MaDaiDoi);
            ViewBag.CurrentMaDaiDoi = madaidoi;
            ViewBag.CurrentPage = page;
            ViewBag.CurrentMaHV = MaHV;
            ViewBag.CurrentTenHV = TenHV;
            ViewBag.CurrentCapbac = CapBac;
  
            ViewBag.CurrentGioiTinh = Gioitinh;
            return View(hocVien);
        }

        // POST: HocViens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, int? page, string? MaHV, string? TenHV, string? CapBac, string? Gioitinh, string? madaidoi, [Bind("MaHocVien,TenHocVien,NgaySinh,MaDaiDoi,GioiTinh,Gac,SoLanGac,MaCapBac,MaChucVu")] HocVien hocVien)
        {
            if (id != hocVien.MaHocVien)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hocVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HocVienExists(hocVien.MaHocVien))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("HocVienTheoDaiDoi", new { page = page, MaHV = MaHV, TenHV = TenHV, Gioitinh = Gioitinh, CapBac = CapBac, madaidoi = madaidoi });
            }
            ViewData["MaCapBac"] = new SelectList(_context.CapBacs, "MaCapBac", "MaCapBac", hocVien.MaCapBac);
            ViewData["MaChucVu"] = new SelectList(_context.ChucVus, "MaChucVu", "MaChucVu", hocVien.MaChucVu);
            ViewData["MaDaiDoi"] = new SelectList(_context.DonVis, "MaDonVi", "MaDonVi", hocVien.MaDaiDoi);
   
            return View(hocVien);
        }

        // GET: HocViens/Delete/5
        public async Task<IActionResult> Delete(string id, int? page, string? MaHV, string? TenHV, string? CapBac, string? Gioitinh, string? madaidoi)
        {
            if (id == null || _context.HocViens == null)
            {
                return NotFound();
            }

            var hocVien = await _context.HocViens
                .Include(h => h.MaCapBacNavigation)
                .Include(h => h.MaChucVuNavigation)
                .Include(h => h.MaDaiDoiNavigation)
                .FirstOrDefaultAsync(m => m.MaHocVien == id);
            if (hocVien == null)
            {
                return NotFound();
            }
            ViewBag.CurrentMaDaiDoi = madaidoi;
            ViewBag.CurrentPage = page;
            ViewBag.CurrentMaHV = MaHV;
            ViewBag.CurrentTenHV = TenHV;
            ViewBag.CurrentCapbac = CapBac;
            ViewBag.CurrentGioiTinh = Gioitinh;
            return View(hocVien);
        }

        // POST: HocViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, int? page, string? MaHV, string? TenHV, string? CapBac, string? Gioitinh, string? madaidoi)
        {
            if (_context.HocViens == null)
            {
                return Problem("Entity set 'CanhGacContext.HocViens'  is null.");
            }
            var hocVien = await _context.HocViens.FindAsync(id);
            if (hocVien != null)
            {
                _context.HocViens.Remove(hocVien);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("HocVienTheoDaiDoi", new { page = page, MaHV = MaHV, TenHV = TenHV, Gioitinh = Gioitinh, CapBac = CapBac, madaidoi = madaidoi });
        }

        private bool HocVienExists(string id)
        {
          return (_context.HocViens?.Any(e => e.MaHocVien == id)).GetValueOrDefault();
        }
    }
}
