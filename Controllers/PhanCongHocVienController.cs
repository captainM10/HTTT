using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyPhamCheilinus.Controllers;
using MyPhamCheilinus.Infrastructure;
using MyPhamCheilinus.Models;
using MyPhamCheilinus.ModelViews;


namespace MyPhamCheilinus.Controllers
{
    public class PhanCongHocVienController : Controller
    {

        CanhGacContext db = new CanhGacContext();
       

        private readonly ILogger <PhanCongHocVienController> _logger;
        public PhanCongHocVienController(ILogger<PhanCongHocVienController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(DateTime ngay)
        {
            var queryResult = (from pcg in db.Pcgacs
                               join hv in db.HocViens on pcg.MaHocVien equals hv.MaHocVien
                               join nv in db.NhiemVus on pcg.MaNhiemVu equals nv.MaNhiemVu
                               join cg in db.CaGacs on pcg.MaCaGac equals cg.MaCaGac
                               join vg in db.VongGacs on nv.MaVongGac equals vg.MaVongGac
                               where pcg.Ngay.Date == ngay.Date
                               orderby cg.MaCaGac
                               select new PCGacViewModel
                               {
                                   MaHocVien = hv.MaHocVien,
                                   TenHocVien = hv.TenHocVien,
                                   GioiTinh = hv.GioiTinh,
                                   TenNhiemVu = nv.TenNhiemVu,
                                   TenVongGac = vg.TenVongGac,
                                   Ngay = ngay,
                                   TuGio = TimeSpan.Parse(cg.TuGio.ToString()),
                                   DenGio = TimeSpan.Parse(cg.DenGio.ToString())
                               }).ToList();

            return View(queryResult);
        }

        [HttpPost]
        public IActionResult ThayTheHocVien(string maHocVienCu, DateTime ngay)
        {
            try
            {
                // Gọi proc ThayTheHocVien
                db.Database.ExecuteSqlRaw("EXEC ThayTheHocVien @MaHocVienCu, @Ngay",
                    new SqlParameter("@MaHocVienCu", maHocVienCu),
                    new SqlParameter("@Ngay", ngay));

                // Load lại trang, có thể chuyển hướng hoặc render lại view tùy thuộc vào yêu cầu của bạn
                return RedirectToAction("Index"); // Chuyển hướng đến action Index
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và ghi log nếu cần
                return StatusCode(500, "Internal Server Error");
            }
        }



       










        //// GET: PhanCongHocVien
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: PhanCongHocVien/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: PhanCongHocVien/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: PhanCongHocVien/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: PhanCongHocVien/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: PhanCongHocVien/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: PhanCongHocVien/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: PhanCongHocVien/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
