using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class SachOnlineController : Controller
    {
        QLBanSachEntities db = new QLBanSachEntities();
        private List<SACH> Laysachmoi(int count)
        {
            return db.SACHes.OrderByDescending(item => item.NgayCapNhat).Take(count).ToList();
        }
        // GET: SachOnline
        public ActionResult Index()
        {
            var Danhsach = Laysachmoi(6);
            return View(Danhsach);
        }
        public ActionResult ChuDePartial()
        {
            var listcd = db.CHUDEs.ToList();
            var listnxb = db.NHAXUATBANs.ToList();

            ViewBag.nxb = listnxb;
            return PartialView(listcd);
        }
        private List<SACH> LaySachBanNhieu(int count=6)
        {
            return db.SACHes.OrderByDescending(item => item.SoLuongBan).Take(count).ToList();
        }
        public ActionResult SachBanNhieu()
        {
            var ds = LaySachBanNhieu(6);
            return PartialView(ds);
        }
        public ActionResult NavPartial()
        {
            return PartialView();
        }
        public ActionResult SliderPartial()
        {
            return PartialView();
        }
        public ActionResult FooterPartial()
        {
            return PartialView();
        }
        public ActionResult Sachtheochude(int id)
        {
            var ds = from s in db.SACHes where s.MaCD==id select s;
            return View(ds);
        }
        public ActionResult Sachtheonhaxuatban(int id)
        {
            var ds = from s in db.SACHes where s.MaNXB == id select s;
            return View(ds);
        }
        
        public ActionResult Chitietsach(int id)
        {
            SACH tmp = db.SACHes.FirstOrDefault(item => item.MaSach==id);
            return View(tmp);
        }
    }
}