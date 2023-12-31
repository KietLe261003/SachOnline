﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.App_Start;
namespace WebApplication1.Areas.Admin.Controllers
{

    public class HomeController : Controller
    {

        QLBanSachEntities db = new QLBanSachEntities();
        // GET: Admin/Home
        [AdminAuthorize()]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            var User = f["Username"];
            var Password = f["Password"];
            ADMIN ad = db.ADMINs.FirstOrDefault(item => item.TenDNAdmin==User && item.MatKhauAdmin==Password);
            if(ad!=null)
            {
                Session["Admin"] = ad;
                return RedirectToAction("Index");
            }    
            else
            {
                ViewBag.ThongBao = "Tên Đăng nhập hoặc mật khẩu không đúng";
            }


            return View();
        }
    }
}