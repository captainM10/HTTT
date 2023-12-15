using CanhGac.Respository;
using Microsoft.AspNetCore.Mvc;

namespace CanhGac.ViewComponents
{
    public class DaiDoiMenuViewComponent : ViewComponent
    {
        private readonly IDaiDoiRespository daidoi;
        public DaiDoiMenuViewComponent(IDaiDoiRespository daiDoiRespository)
        {
            daidoi = daiDoiRespository;
        }
        public IViewComponentResult Invoke()
        {
            var donvi = daidoi.GetAllDonVi().OrderBy(x => x.MaDonVi);
            return View(donvi);
        }
    }
}
