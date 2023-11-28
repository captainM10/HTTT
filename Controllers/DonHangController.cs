using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyPhamCheilinus.Controllers;
using MyPhamCheilinus.Infrastructure;
using MyPhamCheilinus.Models;


namespace MyPhamCpuheilinus.Controllers
{
    public class DonHangController : Controller
    {

        
        _2023MyPhamContext db = new _2023MyPhamContext();
        private readonly ILogger<DonHangController> _logger;

        public DonHangController(ILogger<DonHangController> logger)
        {
            _logger = logger;
        }

        public IActionResult DonHang()
        {
            var donHangList = db.DonHangs
           .Include(dh => dh.ChiTietDonHangs)
               .ThenInclude(ctdh => ctdh.MaSanPhamNavigation)
           .ToList();

            return View(donHangList);

            
        }
        [HttpPost]
        public IActionResult UpdateStatus(string donHangId, int trangThaiDonHang)
        {
            var donHang = db.DonHangs.Find(donHangId);
            if (donHang == null)
            {
                return NotFound();
            }

            donHang.TrangThaiDonHang = trangThaiDonHang;
            db.SaveChanges();

            return Ok();
        }
    }
}
