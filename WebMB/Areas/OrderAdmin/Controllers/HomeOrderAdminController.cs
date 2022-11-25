using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMB.Context;
using WebMB.Models;

namespace WebMB.Areas.OrderAdmin.Controllers
{
    public class HomeOrderAdminController : Controller
    {
        //order 0:chưa xử lí 1: xử lí 2: thanh cong 3:đơn bị hủy        requiment 0:yêu cầu check 1:đủ 2:ko đủ 3:yêu cầu xuát 4:hủy yêu cầu xuất kho 5:xuất thành công
        WebMobileEntities1 objWebMobileEntities = new WebMobileEntities1();
        // GET: OrderAdmin/HomerOrderAdmin
        public ActionResult Index()
        {

                var listOr = objWebMobileEntities.Orders.ToList();
                var sum = 0;
                foreach (var item in listOr)
                {
                    sum++;
                }
                ViewBag.sum1 = sum;
                var billchuaxuli = 0;
                foreach (var item in listOr)
                {
                    if (item.Status == 0)
                    {
                        billchuaxuli++;
                    }
                }
                ViewBag.sum2 = billchuaxuli;

                var billkhodangxuli = 0;
                foreach (var item in listOr)
                {
                    if (item.Status == 1)
                    {
                        billkhodangxuli++;
                    }
                }
                ViewBag.sum3 = billkhodangxuli;

                var billthanhcong = 0;
                foreach (var item in listOr)
                {
                    if (item.Status == 2)
                    {
                        billthanhcong++;
                    }
                }
                ViewBag.sum4 = billthanhcong;

                var billfail = 0;
                foreach (var item in listOr)
                {
                    if (item.Status == 2)
                    {
                        billfail++;
                    }
                }
                ViewBag.sum5 = billfail;

                return View();
            
        }
        public ActionResult ManagerProduct()
        {
            var listProduct = objWebMobileEntities.Products.ToList();
            return View(listProduct);
        }
        public ActionResult ManagerOrder()
        {
            var listOr = objWebMobileEntities.Orders.ToList();

            var model =
                        from a in objWebMobileEntities.Orders
                        join c in objWebMobileEntities.OrderDetails on a.ID equals c.OrderID
                        join b in objWebMobileEntities.Products on c.ProductID equals b.id
                        join d in objWebMobileEntities.Accounts on a.Username equals d.username


                        select new OrderView()
                        {
                            Order = a,
                            orderDetail = c,
                            Product = b,
                            account = d,
                        };
            ViewBag.OrderviewAdmin = model.ToList();

            return View(listOr);
        }
        public ActionResult CreateRequiforStock(int OrderID)
        {
            var listOr = objWebMobileEntities.Orders.ToList();
            foreach (var item in listOr)
            {
                if (item.ID == OrderID)
                {
                    //đang xử lí
                    item.Status = 1;
                    RequimentExport requimentExport = new RequimentExport();
                    requimentExport.OrderID = item.ID;
                    requimentExport.CreateDay = DateTime.Now;
                    //chưa xử lí check kho
                    requimentExport.StatusRequiment = 0;
                    objWebMobileEntities.RequimentExports.Add(requimentExport);
                    objWebMobileEntities.SaveChanges();
                    break;


                }

            }
            return RedirectToAction("ManagerOrder");
        }
        public ActionResult RequimentExport()
        {
            var listOr = objWebMobileEntities.RequimentExports.ToList();
            var model =
            from a in objWebMobileEntities.Orders
            join c in objWebMobileEntities.OrderDetails on a.ID equals c.OrderID
            join b in objWebMobileEntities.Products on c.ProductID equals b.id
            join d in objWebMobileEntities.Accounts on a.Username equals d.username


            select new OrderView()
            {
                Order = a,
                orderDetail = c,
                Product = b,
                account = d,
            };
            ViewBag.ReviewAdmin = model.ToList();

            return View(listOr);
        }
        public ActionResult CheckStock(int OrderID)
        {
            var listOr = objWebMobileEntities.RequimentExports.ToList();
            var model =
            from a in objWebMobileEntities.Orders
            join c in objWebMobileEntities.OrderDetails on a.ID equals c.OrderID
            join b in objWebMobileEntities.Products on c.ProductID equals b.id
            select new OrderView()
            {
                Order = a,
                orderDetail = c,
                Product = b,
            };
            var list = model.ToList();
            foreach (var item in list)
            {
                if (item.Order.ID == OrderID)
                {
                    if (item.Product.count - item.orderDetail.Quantity < 0)
                    {
                        foreach (var req in listOr)
                        {
                            if (item.Order.ID == req.OrderID)
                            {
                                // check du =1 ko du =2
                                req.StatusRequiment = 2;
                                objWebMobileEntities.SaveChanges();
                                return RedirectToAction("RequimentExport");
                            }
                        }

                    }
                }
            }
            foreach (var item in listOr)
            {
                if (item.OrderID == OrderID)
                {
                    //đủ trạng thái yêu cầu 1
                    item.StatusRequiment = 1;
                    objWebMobileEntities.SaveChanges();
                    break;
                }
            }




            return RedirectToAction("RequimentExport");

        }
        public ActionResult CreateExport(int OrderID)
        {

            var model =
            from a in objWebMobileEntities.Orders
            join c in objWebMobileEntities.OrderDetails on a.ID equals c.OrderID
            join b in objWebMobileEntities.Products on c.ProductID equals b.id
            join d in objWebMobileEntities.Accounts on a.Username equals d.username


            select new OrderView()
            {
                Order = a,
                orderDetail = c,
                Product = b,
                account = d,
            };
            var list = model.ToList();
            var listOr = objWebMobileEntities.RequimentExports.ToList();
            //set status of requiment
            foreach (var item2 in listOr)
            {
                if (item2.OrderID == OrderID)
                {
                    //set yêu cầu xuất là đã xuất hàng
                    item2.StatusRequiment = 5;
                    objWebMobileEntities.SaveChanges();
                    break;
                }

            }
            //minus in dattabase

            var listofDetailOrder = objWebMobileEntities.OrderDetails.ToList();
            var listofProduct = objWebMobileEntities.Products.ToList();
            foreach (var item in listofDetailOrder)
            {
                if (item.OrderID == OrderID)
                {
                    foreach (var item2 in listofProduct)
                    {
                        if (item.OrderID == item2.id)
                        {
                            item2.count = item2.count - item.Quantity;
                            objWebMobileEntities.SaveChanges();
                        }
                    }
                }
            }

            return RedirectToAction("RequimentExport");

        }
        public ActionResult AcceptOrd(int OrderID)
        {
            var listofOrder = objWebMobileEntities.Orders.ToList();
            foreach (var item2 in listofOrder)
            {
                if (item2.ID == OrderID)
                {
                    //set thanh công cho hoa đơn
                    item2.Status = 2;
                    objWebMobileEntities.SaveChanges();
                    break;

                }
            }
            var model =
            from a in objWebMobileEntities.Orders
            join c in objWebMobileEntities.OrderDetails on a.ID equals c.OrderID
            join b in objWebMobileEntities.Products on c.ProductID equals b.id
            join d in objWebMobileEntities.Accounts on a.Username equals d.username


            select new OrderView()
            {
                Order = a,
                orderDetail = c,
                Product = b,
                account = d,
            };
            var list = model.ToList();
            var listOr = objWebMobileEntities.RequimentExports.ToList();
            //set status of requiment
            //set yeu cau xuat kho trang thai 3
            foreach (var item2 in listOr)
            {
                if (item2.OrderID == OrderID)
                {
                    item2.StatusRequiment = 3;
                    objWebMobileEntities.SaveChanges();
                    break;
                }

            }
            return RedirectToAction("ManagerOrder");

        }
        public ActionResult CancelOrder(int OrderID)
        {
            var listofOrder = objWebMobileEntities.Orders.ToList();
            foreach (var item2 in listofOrder)
            {
                if (item2.ID == OrderID)
                {
                    //don bi hủy

                    item2.Status = 3;
                    objWebMobileEntities.SaveChanges();
                    break;

                }
                var listOr = objWebMobileEntities.RequimentExports.ToList();
                foreach (var item in listOr)
                {
                    if (item.OrderID == OrderID)
                    {
                        //set trang thai là không đủ hàng xuất
                        item.StatusRequiment = 4;
                        objWebMobileEntities.SaveChanges();
                        break;
                    }
                }
            }
            return RedirectToAction("ManagerOrder");

        }
    }
}