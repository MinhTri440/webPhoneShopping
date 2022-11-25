using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMB.Context;

namespace WebMB.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        WebMobileEntities1 objWebMobileEntities = new WebMobileEntities1();
        public ActionResult Index()
        {
            var listProduct = objWebMobileEntities.Products.ToList();
            return View(listProduct);
        }
        public ActionResult Detail()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Index");
        }
    }
}