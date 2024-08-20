using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcBookStore.Models;
using System.Web.Mvc;

namespace MvcBookStore.Controllers
{
    public class GioHangController : Controller
    {
        //lay gio hang
        public List<MatHangMua> LayGioHang()
        {
            List<MatHangMua> gioHang = Session["GioHang"] as List<MatHangMua>;
            //
            if (gioHang == null)
            {
                gioHang = new List<MatHangMua>();
                Session["GioHang"] = gioHang;
            }
            return gioHang;
        }

        //them san pham
        public ActionResult ThemSanPhamVaoGio(int MaSP)
        {
            List<MatHangMua> gioHang = LayGioHang();

            //
            MatHangMua sanPham = gioHang.FirstOrDefault(s => s.MaSach == MaSP);
            if (sanPham == null)
            {
                sanPham = new MatHangMua(MaSP);
                gioHang.Add(sanPham);
            }
            else
            {
                sanPham.SoLuong++; //SP TAng len 1
            }
            return RedirectToAction("Details", "BookStore", new { id = MaSP });
        }

        //tinh tong so luong mat hang dc mua
        private int TinhTongSL()
        {
            int tongSL = 0;
            List<MatHangMua> gioHang = LayGioHang();
            if (gioHang != null)
                tongSL = gioHang.Sum(sp => sp.SoLuong);
            return tongSL;
        }

        //tinh tong tien cac sp dc mua
        private double TinhTongTien()
        {
            double TongTien = 0;
            List<MatHangMua> gioHang = LayGioHang();
            if (gioHang != null)
                TongTien = gioHang.Sum(sp => sp.ThanhTien());
            return TongTien;
        }

        //hien thi thong tin trong gio hang
        public ActionResult HienThiGioHang()
        {
            List<MatHangMua> gioHang = LayGioHang();

            //
            if (gioHang == null || gioHang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            ViewBag.TongSl = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return View(gioHang);
        }

        //
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSl = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }

        //xoa sp khoi gio hang
        public ActionResult XoaMatHang(int MaSP)
        {
            List<MatHangMua> gioHang = LayGioHang();

            //lay sp trong gio hang
            var sanpham = gioHang.FirstOrDefault(s => s.MaSach == MaSP);
            if (sanpham != null)
            {
                gioHang.RemoveAll(s => s.MaSach == MaSP);
                return RedirectToAction("HienThiGioHang"); //quay ve trang gio hang
            }
            if (gioHang.Count == 0) //quay ve trang chu neu gio hang trong
                return RedirectToAction("Index", "BookStore");
            return RedirectToAction("HienThiGioHang");
        }

        //cap nhat gio hang
        public ActionResult CapNhatMatHang(int MaSP, int SoLuong)
        {
            List<MatHangMua> gioHang = LayGioHang();
            //lay sp trong gio hang
            var sanpham = gioHang.FirstOrDefault(s => s.MaSach == MaSP);
            if (sanpham != null)
            {
                sanpham.SoLuong = SoLuong;
            }
            return RedirectToAction("HienThiGioHang");
        }

        //ĐẶT HÀNG
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null) //chưa đăng nhập
                return RedirectToAction("DangNhap", "NguoiDung");
            List<MatHangMua> gioHang = LayGioHang();
            if (gioHang == null || gioHang.Count == 0)
                return RedirectToAction("Index", "BookStore");

            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return View(gioHang);
        }

        //ĐỒNG Ý ĐẶT HÀNG
        QLSACHEntities1 database = new QLSACHEntities1();
        //xac nhận đơn và lưu vao csdl
        public ActionResult DongYDatHang()
        {
            KHACHHANG khach = Session["TaiKhoan"] as KHACHHANG;
            List<MatHangMua> gioHang = LayGioHang();
            DONDATHANG DonHang = new DONDATHANG();
            DonHang.MaKH = khach.MaKH;
            DonHang.NgayDH = DateTime.Now;
            DonHang.Trigia = (decimal)TinhTongTien();
            DonHang.Dagiao = false;
            DonHang.Tennguoinhan = khach.HoTenKH;
            DonHang.Diachinhan = khach.DiaCHiKH;
            DonHang.Dienthoainhan = khach.DienthoaiKH;
            DonHang.HTThanhtian = false;
            DonHang.HTGiaohang = false;

            database.DONDATHANGs.Add(DonHang);
            database.SaveChanges();

            //them ct cho don hang
            foreach (var sanpham in gioHang)
            {
                CTDATHANG chitiet = new CTDATHANG();
                chitiet.SoDH = DonHang.SoDH;
                chitiet.MaSach = sanpham.MaSach;
                chitiet.Soluong = sanpham.SoLuong;
                chitiet.Dongia = (decimal)sanpham.DonGia;
                database.CTDATHANGs.Add(chitiet);
            }
            database.SaveChanges();

            //Xóa giỏ hàng
            Session["GioHang"] = null;
            return RedirectToAction("HoanThanhDonHang");
        }

        //Hoàn thành đơn hàng
        public ActionResult HoanThanhDonHang()
        {
            return View();
        }
    }
}