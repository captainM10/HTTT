using CanhGac.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyPhamCheilinus.Controllers;
using MyPhamCheilinus.Infrastructure;
using MyPhamCheilinus.ModelViews;
using System;


namespace MyPhamCheilinus.Controllers
{
    public class PhanCongHocVienController : Controller
    {

        CanhGacContext db = new CanhGacContext();


        private readonly ILogger<PhanCongHocVienController> _logger;
        public PhanCongHocVienController(ILogger<PhanCongHocVienController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(DateTime ngay, int? page,  string? TenHV = "",  string? NhiemVu = "", string? VongGac = "")
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;

            var queryResult = (from pcg in db.Pcgacs
                               join hv in db.HocViens on pcg.MaHocVien equals hv.MaHocVien
                               join nv in db.NhiemVus on pcg.MaNhiemVu equals nv.MaNhiemVu
                               join cg in db.CaGacs on pcg.MaCaGac equals cg.MaCaGac
                               join vg in db.VongGacs on nv.MaVongGac equals vg.MaVongGac
                               where pcg.Ngay.Date == ngay.Date
                         && (string.IsNullOrEmpty(TenHV) || hv.TenHocVien.Contains(TenHV))
                         && (string.IsNullOrEmpty(NhiemVu) || nv.TenNhiemVu.Contains(NhiemVu))
                         && (string.IsNullOrEmpty(VongGac) || vg.TenVongGac.Contains(VongGac))            
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

            ViewBag.CurrentNgay = ngay;
            ViewBag.CurrentNV = NhiemVu;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.CurrentTenHV = TenHV;
            ViewBag.CurrentVG = VongGac;
            // Kiểm tra nếu queryResult rỗng, tức là không có dữ liệu cho ngày đầu vào
            if (queryResult.Count == 0)
            {
                var noDataViewModel = new PCGacViewModel
                {
                    Ngay = ngay
                };
                queryResult.Add(noDataViewModel);
            }

            return View(queryResult);
        }
        public IActionResult Filtter(DateTime ngay, int? page, string? TenHV = "", string? NhiemVu = "", string? VongGac = "")
        {
            var url = "/PhanCongHocVien/Index?";
            if (ngay != null)
            {
                url += $"ngay={ngay.ToString("yyyy-MM-dd")}&";
            }

            if (!string.IsNullOrEmpty(NhiemVu))
            {
                url += $"NhiemVu={NhiemVu}&";
            }

            if (!string.IsNullOrEmpty(TenHV))
            {
                url += $"TenHV={TenHV}&";
            }

            if (VongGac != null)
            {
                url += $"VongGac={VongGac}&";
            }

            // Loại bỏ dấu '&' cuối cùng nếu có
            if (url.EndsWith("&"))
            {
                url = url.Substring(0, url.Length - 1);
            }

            return Json(new { status = "success", redirectUrl = url });
        }


        [HttpPost]
        public IActionResult PhanCong(DateTime ngay)
        {
            try
            {
                var ngayParameter = new SqlParameter("@Ngay", ngay);

                // Gọi stored procedure PhanCongGac bằng ExecuteSqlRaw
                db.Database.ExecuteSqlRaw("EXEC PhanCongGac @Ngay", ngayParameter);

                return RedirectToAction("Index"); // Trả về kết quả thành công
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error"); // Trả về lỗi nếu xảy ra exception
            }
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
            catch(Exception ex)
            {
                return StatusCode(500, "Internal Server Error"); // Trả về lỗi nếu xảy ra exception
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