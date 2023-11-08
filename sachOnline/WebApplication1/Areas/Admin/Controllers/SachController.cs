﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Areas.Admin.Controllers
{
    
    public class SachController : Controller
    {
        // GET: Admin/Sach
        QLBanSachEntities db = new QLBanSachEntities();
        public ActionResult Index(int? page)
        {
            const int PageSize = 3;
            int NowPage = page ?? 1;
            List<SACH> s = db.SACHes.ToList();
            int ItemTotal = s.Count();
            int SkipPage = (NowPage - 1) * PageSize;
            var pager = new Pager(ItemTotal, NowPage, PageSize);
            List<SACH> s1 = s.OrderBy(item => item.MaSach).Skip(SkipPage).Take(PageSize).ToList();
            ViewBag.p = pager;

            return View(s1);
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaCd = new SelectList(db.CHUDEs.ToList().OrderBy(item=> item.TenChuDe),"MaCD","TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(item => item.TenNXB),"MaNXB", "TenNXB");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SACH s, FormCollection f,HttpPostedFileBase fFileUpload)
        {
            //Đưa dữ liệu vào DropDown
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n =>n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n =>n.TenNXB), "MaNXB", "TenNXB");
            if (fFileUpload == null)
            {
                //Nội dung thông báo yêu cầu chọn ảnh bìa
                ViewBag.ThongBao = "Hãy chọn ảnh bìa.";
                //Lưu thông tin để khi load lại trang do yêu cầu chọn ảnh bìa sẽ hiển thịcác thông tin này lên trang
                ViewBag.TenSach = f["sTenSach"];
                ViewBag.MoTa = f["sMoTa"];
                ViewBag.SoLuong = int.Parse(f["iSoLuong"]);
                ViewBag.GiaBan = decimal.Parse(f["mGiaBan"]);
                ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", int.Parse(f["MaCD"]));
                ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", int.Parse(f["MaNXB"]));
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Lấy tên file (Khai báo thư viện: System.IO)
                    var sFileName = Path.GetFileName(fFileUpload.FileName); //Lấy đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/Images/Sach"), sFileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    s.TenSach = f["sTenSach"];
                    s.MoTa = f["sMoTa"].Replace("<p>", "").Replace("</p>", "\n");
                    s.HinhMinhHoa = sFileName;
                    s.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]); 
                    s.SoLuongBan = int.Parse(f["iSoLuong"]);
                    s.DonGia = decimal.Parse(f["mGiaBan"]); 
                    s.MaCD =int.Parse(f["MaCD"]); 
                    s.MaNXB = int.Parse(f["MaNXB"]);
                    db.SACHes.Add(s);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View();
            }
        }
        public ActionResult Details(int id)
        {
            var Sach = db.SACHes.FirstOrDefault(item => item.MaSach == id);
            if(Sach==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(Sach);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var Sach = db.SACHes.FirstOrDefault(item => item.MaSach == id);
            if (Sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(Sach);
        }
        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var Sach = db.SACHes.FirstOrDefault(item => item.MaSach == id);
            if (Sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var CTDH = db.CTDATHANGs.Where(item => item.MaSach == id);
            if(CTDH.Count()>0)
            {
                ViewBag.ThongBao = "Sách này đã này đã có trong bảng chi tiết đặt hàng <br>"+
                    "Nếu muốn xóa thì hãy xóa hết trong chi tiết đặt hàng";
                return View(Sach);
            }
            var vietsach = db.VIETSACHes.Where(item => item.MaSach == id).ToList();
            if(vietsach!=null)
            {
                foreach(var item in vietsach)
                {
                    db.VIETSACHes.Remove(item);
                }
                db.SaveChanges();
            }
            db.SACHes.Remove(Sach);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var Sach = db.SACHes.FirstOrDefault(item => item.MaSach == id);
            if (Sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", Sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", Sach.MaNXB);
            return View(Sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            int id = int.Parse(f["iMaSach"]);
            var Sach = db.SACHes.FirstOrDefault(item => item.MaSach ==id);
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD","TenChuDe", Sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB","TenNXB", Sach.MaNXB);

            if (ModelState.IsValid)
            {
                if(fFileUpload!=null)
                {
                    //Lấy tên file (Khai báo thư viện: System.IO)
                    var sFileName = Path.GetFileName(fFileUpload.FileName); //Lấy đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/Images/Sach"), sFileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    Sach.HinhMinhHoa = sFileName;
                }    
                Sach.TenSach = f["sTenSach"];
                Sach.MoTa = f["sMoTa"].Replace("<p>", "").Replace("</p>", "\n");
                Sach.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);
                Sach.SoLuongBan = int.Parse(f["iSoLuong"]);
                Sach.DonGia = decimal.Parse(f["mGiaBan"]);
                Sach.MaCD = int.Parse(f["MaCD"]);
                Sach.MaNXB = int.Parse(f["MaNXB"]);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Sach);
        }
    }
}