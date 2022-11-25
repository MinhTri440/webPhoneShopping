using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMB.Areas.StockAdmin.Controllers
{
    public class HomeStockAdminController : Controller
    {
        // GET: StockAdmin/HomeStockAdmin
        public ActionResult Index()
        {
            return View();
        }
    }
}