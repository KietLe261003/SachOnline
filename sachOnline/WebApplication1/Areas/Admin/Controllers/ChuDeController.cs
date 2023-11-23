using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.App_Start;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class ChuDeController : Controller
    {
        // GET: Admin/ChuDe
        QLBanSachEntities db = new QLBanSachEntities();
        [AdminAuthorize()]
        public ActionResult Index(int ?page)
        {
            const int PageSize = 10;
            int NowPage = page ?? 1;
            List<CHUDE> s = db.CHUDEs.ToList();
            int ItemTotal = s.Count();
            int SkipPage = (NowPage - 1) * PageSize;
            var pager = new Pager(ItemTotal, NowPage, PageSize);
            List<CHUDE> s1 = s.OrderBy(item => item.MaCD).Skip(SkipPage).Take(PageSize).ToList();
            ViewBag.p = pager;
            return View(s1);
        }

        //Thêm chủ đề start
        [AdminAuthorize()]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection f)
        {
            CHUDE cd = new CHUDE();
            cd.TenChuDe =f["sNameCd"];
            db.CHUDEs.Add(cd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //Thêm chủ đề end


        //Sửa start
        public ActionResult Edit(int id)
        {
            CHUDE cd = db.CHUDEs.FirstOrDefault(item => item.MaCD == id);
            return View(cd);
        }
        [HttpPost]
        public ActionResult Edit(FormCollection f)
        {
            int id =int.Parse(f["iMaCD"]);
            CHUDE cd = db.CHUDEs.FirstOrDefault(item => item.MaCD == id);
            var NameCd = f["sNameCd"];
            if(NameCd==null)
            {
                ViewBag.ThongBao = "Tên chủ đề không được để trống";
                return View(cd);
            }
            cd.TenChuDe = NameCd;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //Sửa end

        public ActionResult Delete(int id)
        {
            CHUDE cd = db.CHUDEs.FirstOrDefault(item => item.MaCD == id);
            return View(cd);
        }
        [HttpPost]
        public ActionResult Delete(FormCollection f)
        {
            int id = int.Parse(f["iMaCD"]);
            CHUDE cd = db.CHUDEs.FirstOrDefault(item => item.MaCD == id);
            db.CHUDEs.Remove(cd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            CHUDE cd = db.CHUDEs.FirstOrDefault(item => item.MaCD == id);
            return View(cd);
        }
    }
}