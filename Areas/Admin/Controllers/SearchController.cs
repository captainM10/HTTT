using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPhamCheilinus.Models;
using System.Runtime.CompilerServices;

namespace MyPhamCheilinus.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchController : Controller
    {
        private readonly _2023MyPhamContext _context;
        public SearchController(_2023MyPhamContext context)
        {
            _context = context;
        }
        [HttpPost]
        [HttpPost]
        public IActionResult FindProduct(string keyword, double? priceMin, double? priceMax, int? page)
        {
            List<SanPham> ls = new List<SanPham>();

            if (String.IsNullOrEmpty(keyword) && !priceMin.HasValue && !priceMax.HasValue)
            {
                // Nếu không có từ khóa hoặc giá tiền, hiển thị toàn bộ sản phẩm
                ls = _context.SanPhams
                    .AsNoTracking()
                    .Include(a => a.MaDanhMucNavigation)
                    .OrderByDescending(x => x.MaSanPham)
                    .Take(10)
                    .ToList();
            }
            else
            {
                // Nếu có từ khóa hoặc giá tiền, thực hiện tìm kiếm
                ls = _context.SanPhams
                    .AsNoTracking()
                    .Include(a => a.MaDanhMucNavigation)
                    .Where(x =>
                        (string.IsNullOrEmpty(keyword) || x.TenSanPham.Contains(keyword)) &&
                        (!priceMin.HasValue || x.Gia >= priceMin) &&
                        (!priceMax.HasValue || x.Gia <= priceMax)
                    )
                    .OrderByDescending(x => x.TenSanPham)
                    .Take(10)
                    .ToList();
            }
            ViewBag.CurrenPage = page;
            return PartialView("ListProductsSearchPartial", ls);
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
