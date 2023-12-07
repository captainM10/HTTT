using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyPhamCheilinus.Models1;

namespace MyPhamCheilinus.Controllers
{
    public class ThongTinGacsController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly CanhGacContext _context;

        public ThongTinGacsController(CanhGacContext context)
        {
            _context = context;
        }

        // GET: ThongTinGacs
        public async Task<IActionResult> Index()
        {
            var canhGacContext = _context.DonVis;
            return View(await canhGacContext.ToListAsync());
        }


        public IActionResult GetEvents_D1()
        {
            try
            {
                var events = _context.ThongTinGacs
                    .Where(p => _context.ThongTinGacs.Select(d => d.MaDonVi).Contains(p.MaDonVi))
                    .ToList();

                var eventList = events.Select(e => new
                {
                    id = e.Ngay.ToString("yyyy-MM-dd"),
                    title = e.MaDonVi,
                    start = e.Ngay.ToString("yyyy-MM-dd"),
                    allDay = true,
                    color = (e.MaDonVi == "C158") ? "#FF8A65" : (e.MaDonVi == "C157") ? "#00695C" : (e.MaDonVi == "C156") ? "#82B1FF" : (e.MaDonVi == "C155") ? "#7B1FA2" : null,
                    hoi = e.Hoi,
                    dap = e.Dap
                });

                return Json(eventList);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }


        private IActionResult JsonResult(IEnumerable<object> eventList, JsonRequestBehavior allowGet)
        {
            throw new NotImplementedException();
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult EditEvent(string MaDonVi, DateTime Ngay, string Hoi, string Dap)
        {
            try
            {
                var existingEvent = _context.ThongTinGacs.FirstOrDefault(e => e.Ngay.Date == Ngay.Date);

                if (existingEvent == null)
                {
                    return Json(new { status = "error", message = "Không tìm thấy ngày gác." });
                }

                if (Ngay.Date < DateTime.Today)
                {
                    return Json(new { status = "error", message = "Không thể sửa đổi ngày gác cho ngày đã qua." });
                }

                existingEvent.MaDonVi = MaDonVi;
                existingEvent.Ngay = Ngay;
                existingEvent.Hoi = Hoi;
                existingEvent.Dap = Dap;

                _context.SaveChanges();

                return Json(new { status = "success", message = "Đã sửa ngày gác thành công." });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }

        private Microsoft.AspNetCore.Mvc.JsonResult JsonResult(object value)
        {
            throw new NotImplementedException();
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult DeleteEvent(DateTime id)
        {
            try
            {
                var existingEvent = _context.ThongTinGacs.FirstOrDefault(e => e.Ngay.Date == id.Date);

                if (existingEvent == null)
                {
                    return Json(new { status = "error", message = "Không tìm thấy ngày gác." });
                }

                if (existingEvent.Ngay.Date < DateTime.Today)
                {
                    return Json(new { status = "error", message = "Không thể xóa ngày gác cho ngày đã qua." });
                }

                _context.ThongTinGacs.Remove(existingEvent);
                _context.SaveChanges();

                return Json(new { status = "success", message = "Đã xóa ngày gác thành công." });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }


        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult AddEvent(ThongTinGac model)
        {
            try
            {
                using (var _contextContext = new CanhGacContext())
                {
                    if (model == null)
                    {
                        return Json(new { status = "error", message = "Dữ liệu không hợp lệ." });
                    }

                    if (_contextContext.ThongTinGacs.Any(e => e.Ngay == model.Ngay))
                    {
                        return Json(new { status = "error", message = "MaPCDaiDoi đã tồn tại. Vui lòng nhập lại." });
                    }

                    if (model.Ngay.Date < DateTime.Today)
                    {
                        return Json(new { status = "error", message = "Không thể thêm ngày gác cho ngày đã qua." });
                    }

                    var newEvent = new ThongTinGac
                    {
                        MaDonVi = model.MaDonVi,
                        Ngay = model.Ngay,
                        Hoi = model.Hoi,
                        Dap = model.Dap
                    };

                    _contextContext.ThongTinGacs.Add(newEvent);
                    _contextContext.SaveChanges();

                    return Json(new { status = "success", message = "Ngày gác đã được thêm thành công." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Đã xảy ra lỗi khi thêm ngày gác." });
            }
        }



        public IActionResult CheckEvents_D1(string date)
        {
            try
            {
                using (var _contextContext = new CanhGacContext())
                {
                    DateTime selectedDate = DateTime.Parse(date);

                    if (selectedDate.Date < DateTime.Today)
                    {
                        return Json(new { status = "error", message = "Không thể kiểm tra ngày gác cho ngày đã qua." });
                    }

                    bool hasEvents = _contextContext.ThongTinGacs.Any(e => e.Ngay == selectedDate && (e.MaDonVi == "C155" || e.MaDonVi == "C156" || e.MaDonVi == "C157" || e.MaDonVi == "C154"));

                    return Json(new { hasEvents = hasEvents });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IActionResult GetDonViForDate(DateTime? date)
        {
            try
            {
                using (var dbContext = new CanhGacContext())
                {
                    // Kiểm tra nếu giá trị date là null
                    if (!date.HasValue)
                    {
                        return Json(new { status = "error", message = "Giá trị ngày truyền vào không hợp lệ." });
                    }

                    // Lấy giá trị đơn vị cho ngày trước đó từ cơ sở dữ liệu
                    var donVi = dbContext.ThongTinGacs
                        .Where(d => d.Ngay < date)
                        .OrderByDescending(d => d.Ngay)
                        .Select(d => d.MaDonVi)
                        .FirstOrDefault();

                    if (donVi != null)
                    {
                        return Json(new { status = "success", donVi = donVi });
                    }
                    else
                    {
                        // Trả về donVi = "c158" nếu không có giá trị đơn vị tương ứng
                        return Json(new { status = "success", donVi = "c158" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }

    }
}