using Microsoft.AspNetCore.Mvc;
using MyPhamCheilinus.Models1;

namespace MyPhamCheilinus.Areas.Admin.Controllers
{

    public class ThongTinGacController : Controller
    {
        private readonly CanhGacContext _context;
        public IActionResult Index()
        {
            var CanhGacContext = _context.DonVis;
            return View();
        }
    }
}
