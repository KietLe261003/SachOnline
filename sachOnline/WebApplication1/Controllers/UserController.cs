using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        QLBanSachEntities db = new QLBanSachEntities();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KHACHHANG kh)
        {
            var sHoTen = collection["HoTenKH"];
            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["Matkhau"];
            var sMatKhauNhapLai = collection["MatKhauNL"];
            var sDiaChi = collection["DiaChiKH"];
            var sEmail = collection["Email"];
            var sDienThoai = collection["DienThoaiKH"];
            var dNgaySinh = String.Format("{0:MM/dd/yyyy}",collection["NgaySinh"]);

            if(String.IsNullOrEmpty(sHoTen))
            {
                ViewData["err1"] = "Họ tên không được rỗng";
            }    
            else if(String.IsNullOrEmpty(sTenDN))
            {
                ViewData["err2"] = "Tên đăng nhập không được rỗng";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["err3"] = "Mật khẩu không được để trống";
            }
            else if (String.IsNullOrEmpty(sMatKhauNhapLai))
            {
                ViewData["err4"] = "Phải nhập lại mật khẩu";
            }
            else if(sMatKhau!=sMatKhauNhapLai)
            {
                ViewData["err4"] = "Mật khẩu không trùng khớp ";
            }
            else if (String.IsNullOrEmpty(sEmail))
            {
                ViewData["err5"] = "Email không được rỗng";
            }
            else if(db.KHACHHANGs.FirstOrDefault(item => item.TenDN==sTenDN)!=null)
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
            }    
            else if(db.KHACHHANGs.FirstOrDefault(item => item.Email==sEmail)!=null)
            {
                ViewBag.ThongBao = "Email đã được sử dụng";
            }    
            else
            {
                kh.HoTenKH = sHoTen;
                kh.TenDN = sTenDN;
                kh.MatKhau = sMatKhau;
                kh.Email = sEmail;
                kh.DiaChiKH = sDiaChi;
                kh.DienThoaiKH = sDienThoai;
                kh.NgaySinh = DateTime.Parse(dNgaySinh);

                db.KHACHHANGs.Add(kh);
                db.SaveChanges();
                return RedirectToAction("DangNhap");
            }    
            return View();
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(string TenDN, string Password)
        {
            if(String.IsNullOrEmpty(TenDN))
            {
                ViewData["err1"] = "Tên đăng nhập không được để trống";
            }    
            else if(String.IsNullOrEmpty(Password))
            {
                ViewData["err1"] = "Mật khẩu không được để trống";
            }    
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(item => item.TenDN==TenDN && item.MatKhau==Password);
                if(kh!=null)
                {
                    ViewBag.thongbao = "Đăng nhập thành công";
                    Session["TaiKhoan"] = kh;
                    return RedirectToAction("Index","SachOnline");
                }   
                else
                {
                    ViewBag.thongbao = "Sai tên tk hoặc mật khẩu";
                }   
            }    
            return RedirectToAction("Index","Home");
        }
    }
}