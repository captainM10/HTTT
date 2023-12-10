using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPhamCheilinus.Models;
using System.Diagnostics;
using System.Web;
using X.PagedList;
using static MyPhamCheilinus.Controllers.HomeController;

namespace MyPhamCheilinus.Controllers
{


    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }

}

