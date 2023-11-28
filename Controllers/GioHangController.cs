using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyPhamCheilinus.Controllers;
using MyPhamCheilinus.Infrastructure;
using MyPhamCheilinus.Models;
using System.Data;


namespace MyPhamCpuheilinus.Controllers
{
    public class GioHangController : Controller
    {

        public GioHang? GioHang { get; set; }
        _2023MyPhamContext db = new _2023MyPhamContext();
        private readonly ILogger<GioHangController> _logger;

        public GioHangController(ILogger<GioHangController> logger)
        {
            _logger = logger;
        }
        [Authorize(Roles = "Admin,Employee,Customer")]
        public IActionResult AddGioHang(string maSanPham)
        {
            SanPham? sanpham = db.SanPhams.FirstOrDefault(p => p.MaSanPham == maSanPham);
            if (sanpham != null)
            {
                GioHang = HttpContext.Session.GetJson<GioHang>("giohang") ?? new GioHang();
                GioHang.AddItem(sanpham, 1);
                HttpContext.Session.SetJson("giohang", GioHang);
                TempData["ThongBao"] = "Sản phẩm đã được thêm vào giỏ hàng";
            }
            return View("GioHang", GioHang);
        }
        public IActionResult Remove_1_FromGioHang(string maSanPham)
        {
            SanPham? sanpham = db.SanPhams.FirstOrDefault(p => p.MaSanPham == maSanPham);
            if (sanpham != null)
            {
                GioHang = HttpContext.Session.GetJson<GioHang>("giohang") ?? new GioHang();
                GioHang.AddItem(sanpham, -1);
                HttpContext.Session.SetJson("giohang", GioHang);
            }
            return View("GioHang", GioHang);
        }

        public IActionResult RemoveFromGioHang(string maSanPham)
        {
            SanPham? sanpham = db.SanPhams.FirstOrDefault(p => p.MaSanPham == maSanPham);
            if (sanpham != null)
            {
                GioHang = HttpContext.Session.GetJson<GioHang>("giohang");
                GioHang.RemoveLine(sanpham);
                HttpContext.Session.SetJson("giohang", GioHang);
            }
            return View("GioHang", GioHang);
        }

        public IActionResult CheckOut()
        {
            var gioHang = HttpContext.Session.GetJson<GioHang>("giohang") ?? new GioHang();
            return View(gioHang);
        }
        public IActionResult ViewGioHang()
        {
            var gioHang = HttpContext.Session.GetJson<GioHang>("giohang") ?? new GioHang();
            return View(gioHang);
        }
        private string GenerateUniqueCustomerCode()
        {
            Random random = new Random();
            int randomNumber = random.Next(10000, 99999); // Sinh ra số ngẫu nhiên từ 10,000 đến 99,999
            string customerCode = "KH" + randomNumber.ToString();
            return customerCode;
        }
        private string GenerateUniqueOrderCode()
        {
            Random random = new Random();
            int randomNumber = random.Next(10000, 99999); // Sinh ra số ngẫu nhiên từ 10,000 đến 99,999
            string customerCode = "DH" + randomNumber.ToString();
            return customerCode;
        }

        public ActionResult ThanhToan(string maKhachHang, string hoTen, string soDienThoai, string diaChi, string email)
        {
            var khachhang = new KhachHang
            {
                //MaKhachHang = GenerateUniqueCustomerCode(),
                TenKhachHang = hoTen,
                DiaChi = diaChi,
                SoDienThoai = soDienThoai,
                Email = email
            };
            db.KhachHangs.Add(khachhang);
            db.SaveChanges();

            var gioHang = HttpContext.Session.GetJson<GioHang>("giohang");
            var donHang = new DonHang
            {
                MaDonHang = GenerateUniqueOrderCode(),
                NgayDatHang = DateTime.Now,
                TongTien = gioHang.ComputeTotalValues(),
                TrangThaiDonHang = 1,
                MaKhachHang = khachhang.MaKhachHang,
                ThanhToan = true,
                VanChuyen = 1,
                PhiVanChuyen = 10000
            };
            db.DonHangs.Add(donHang);
            db.SaveChanges();
            foreach (var line in gioHang.Lines)
            {
                var chiTietDonHang = new ChiTietDonHang
                {
                    MaDonHang = donHang.MaDonHang,
                    MaSanPham = line.SanPham.MaSanPham,
                    SoLuong = line.SoLuong,
                    GiaBan = line.SanPham.Gia
                };
                db.ChiTietDonHangs.Add(chiTietDonHang);
                RemoveFromGioHang(line.SanPham.MaSanPham);
            }

            db.SaveChanges();

            foreach (var line in gioHang.Lines)
            {
                ;
                RemoveFromGioHang(line.SanPham.MaSanPham);
            }
     ;

            gioHang.Clear();


            return View("Success");
        }

    }
}
