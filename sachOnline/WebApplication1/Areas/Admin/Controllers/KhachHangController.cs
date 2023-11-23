using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.App_Start;
using WebApplication1.Models;
namespace WebApplication1.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: Admin/KhachHang
        QLBanSachEntities db = new QLBanSachEntities();
        [AdminAuthorize()]
        public ActionResult Index(int? page)
        {
            const int PageSize = 10;
            int NowPage = page ?? 1;
            List<KHACHHANG> s = db.KHACHHANGs.ToList();
            int ItemTotal = s.Count();
            int SkipPage = (NowPage - 1) * PageSize;
            var pager = new Pager(ItemTotal, NowPage, PageSize);
            List<KHACHHANG> s1 = s.OrderBy(item => item.MaKH).Skip(SkipPage).Take(PageSize).ToList();
            ViewBag.p = pager;
            return View(s1);
        }
        [AdminAuthorize()]
        public ActionResult Create()
        {
            return Redirect("~/User/DangKy");
        }
        public ActionResult Details(int id)
        {
            KHACHHANG kh = db.KHACHHANGs.FirstOrDefault(item=> item.MaKH==id);
            return View(kh);
        }
        public ActionResult Delete(int id)
        {
            KHACHHANG kh = db.KHACHHANGs.FirstOrDefault(item => item.MaKH == id);
            return View(kh);
        }
        [HttpPost]
        public ActionResult Delete(FormCollection f)
        {
            int id = int.Parse(f["iMaKh"]);
            KHACHHANG kh = db.KHACHHANGs.FirstOrDefault(item => item.MaKH == id);
            db.KHACHHANGs.Remove(kh);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            KHACHHANG kh = db.KHACHHANGs.FirstOrDefault(item => item.MaKH == id);
            return View(kh);
        }
        [HttpPost]
        public ActionResult Edit(KHACHHANG kh)
        {
            KHACHHANG kh1 = db.KHACHHANGs.FirstOrDefault(item => item.MaKH==kh.MaKH);
            kh1.HoTenKH = kh.HoTenKH;
            kh1.DiaChiKH = kh.DiaChiKH;
            kh1.DienThoaiKH = kh.DienThoaiKH;
            kh1.TenDN = kh.TenDN;
            kh1.MatKhau = kh.MatKhau;
            kh1.NgaySinh = kh.NgaySinh;
            kh1.GioiTinh = kh.GioiTinh;
            kh1.Email = kh.Email;
            kh1.DaDuyet = kh.DaDuyet;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}