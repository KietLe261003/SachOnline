using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using PagedList;
using PagedList.Mvc;

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
        public ActionResult Index(int ?page)
        {
            const int PageSize = 6;
            int NowPage = page ?? 1;

            List<SACH> Danhsach = db.SACHes.ToList();
            ViewBag.s = Danhsach;
            int ItemTotal = Danhsach.Count();
            int SkipPage = (NowPage - 1) * PageSize;
            var pager = new Pager(ItemTotal, NowPage, PageSize);
            var ds1 = Danhsach.OrderBy(item => item.MaSach).Skip(SkipPage).Take(PageSize);
            ViewBag.Pager = pager;
            return View(ds1);
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
        public ActionResult Sachtheochude(int id,int ?page)
        {
            ViewBag.MaCd = id;
            const int PageSize= 3;
            int NowPage = page ?? 1;
            
            var ds = from s in db.SACHes where s.MaCD==id select s;
            int ItemTotal = ds.Count();
            int SkipPage = (NowPage - 1) * PageSize;
            var pager = new Pager(ItemTotal, NowPage, PageSize);
            var ds1 = ds.OrderBy(item => item.MaCD).Skip(SkipPage).Take(PageSize);
            ViewBag.Pager = pager;
            
            return View(ds1);
        }
        public ActionResult Sachtheonhaxuatban(int id,int ?page)
        {
            ViewBag.MaNXB = id;
            const int PageSize = 3;
            int NowPage = page ?? 1;
            var ds = from s in db.SACHes where s.MaNXB == id select s;
            int ItemTotal = ds.Count();
            int SkipPage = (NowPage - 1) * PageSize;
            var pager = new Pager(ItemTotal, NowPage, PageSize);
            var ds1 = ds.OrderBy(item => item.MaCD).Skip(SkipPage).Take(PageSize);
            ViewBag.Pager = pager;
            return View(ds1);
        }
        
        public ActionResult Chitietsach(int id)
        {
            SACH tmp = db.SACHes.FirstOrDefault(item => item.MaSach==id);
            return View(tmp);
        }
        public ActionResult LoginLogout()
        {
            return View();   
        }
    }
}