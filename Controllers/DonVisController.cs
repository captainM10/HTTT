using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyPhamCheilinus.Models;

namespace MyPhamCheilinus.Controllers
{
    public class DonVisController : Controller
    {
        private readonly CanhGacContext _context;

        public DonVisController(CanhGacContext context)
        {
            _context = context;
        }

        // GET: DonVis
        public async Task<IActionResult> Index()
        {
              return _context.DonVis != null ? 
                          View(await _context.DonVis.ToListAsync()) :
                          Problem("Entity set 'CanhGacContext.DonVis'  is null.");
        }

        // GET: DonVis/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.DonVis == null)
            {
                return NotFound();
            }

            var donVi = await _context.DonVis
                .FirstOrDefaultAsync(m => m.MaDonVi == id);
            if (donVi == null)
            {
                return NotFound();
            }

            return View(donVi);
        }

        // GET: DonVis/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DonVis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDonVi,TenDonVi,QuanSo,MauSac")] DonVi donVi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donVi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(donVi);
        }

        // GET: DonVis/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.DonVis == null)
            {
                return NotFound();
            }

            var donVi = await _context.DonVis.FindAsync(id);
            if (donVi == null)
            {
                return NotFound();
            }
            return View(donVi);
        }

        // POST: DonVis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaDonVi,TenDonVi,QuanSo,MauSac")] DonVi donVi)
        {
            if (id != donVi.MaDonVi)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donVi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonViExists(donVi.MaDonVi))
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
            return View(donVi);
        }

        // GET: DonVis/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.DonVis == null)
            {
                return NotFound();
            }

            var donVi = await _context.DonVis
                .FirstOrDefaultAsync(m => m.MaDonVi == id);
            if (donVi == null)
            {
                return NotFound();
            }

            return View(donVi);
        }

        // POST: DonVis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.DonVis == null)
            {
                return Problem("Entity set 'CanhGacContext.DonVis'  is null.");
            }
            var donVi = await _context.DonVis.FindAsync(id);
            if (donVi != null)
            {
                _context.DonVis.Remove(donVi);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonViExists(string id)
        {
          return (_context.DonVis?.Any(e => e.MaDonVi == id)).GetValueOrDefault();
        }
    }
}
