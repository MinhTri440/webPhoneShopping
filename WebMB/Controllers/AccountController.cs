    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using WebMB.Context;

namespace WebMB.Controllers
{
    public class AccountController : Controller
    {
        WebMobileEntities1 objWebMobileEntities = new WebMobileEntities1();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Account _user)
        {
            if (ModelState.IsValid)
            {

                var check = objWebMobileEntities.Accounts.FirstOrDefault(s => s.username == _user.username);
                if (check == null)
                {
                    objWebMobileEntities.Configuration.ValidateOnSaveEnabled = false;
                    objWebMobileEntities.Accounts.Add(_user);
                    objWebMobileEntities.SaveChanges();
                    ViewBag.SuccessMessage = "Register success";
                    return RedirectToAction("Register");
                }

                else
                {
                    ViewBag.Error = "username already exists";
                    return View();
                }
                }
                return View();

            }
           
        

        public ActionResult Login()
        {
        return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(String username, string password)
        {
            if (ModelState.IsValid)
            {
                var check = objWebMobileEntities.Accounts.FirstOrDefault(s => s.username == username && s.password == password);
                if (check != null)
                {
                    
                    //add session
 //                   if (check.type.Equals("4"))
 //                   {
                        Session["UserName"] = check.username;
                        return RedirectToAction("Index", "Home");
                    //                   }
                    //                   else if (check.type.ToString().Equals("3"))
                    //                 {
                    //                   Session["OrderAdmin"] = check.username;
                    //                 return RedirectToAction("~/OrderAdmin/HomeOrderAdmin/");
                    //           }
                    //         else if (check.type.ToString().Equals("2"))
                    //       {
                    //          Session["StockAdmin"] = check.username;
                    //        return RedirectToAction("~/StockAdmin/HomeStockAdmin/");
                    //  }
                    //else
                    //{
                    //  Session["Admin"] = check.username;
                    //return RedirectToAction("~/Admin/HomeAdmin/");
                }
                //}

                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }


            }
            return View("Login");
        }
        public ActionResult LogOut()
        {
            Session.Clear();//remove session
            return RedirectToAction("Index");
        }
    }
}