using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using MvcBookStore.Models;

namespace MvcBookStore.Controllers
{
    public class BookStoreController : Controller
    {
        // Use DbContext to manage database
        QLSACHEntities1 database = new QLSACHEntities1();
        private List<SACH> LaySachMoi(int soluong)
        {
            // Sắp xếp sách theo ngày cập nhật giảm dần, lấy đúng số lượng sách cần
            // Chuyển qua dạng danh sách kết quả đạt được
            return database.SACHes.OrderByDescending(sach =>
            sach.Ngaycapnhat).Take(soluong).ToList();
        }
        // GET: BookStore
        public ActionResult Index(int? page)
        {
            //tao bien cho biet so sach moi trang
            int pageSize = 5;
            //Tao bien so trang
            int pageNum = (page ?? 1);

            // Giả sử cần lấy 5 quyển sách mới cập nhật
            var dsSachMoi = LaySachMoi(15);
            return View(dsSachMoi.ToPagedList(pageNum, pageSize));
        }

        //Chu De
        public ActionResult LayChuDe()
        {
            var dsChuDe = database.CHUDEs.ToList();
            return PartialView(dsChuDe);
        }

        //Nha xuat ban
        public ActionResult LayNhaXuatBan()
        {
            var dsNhaXB = database.NHAXUATBANs.ToList();
            return PartialView(dsNhaXB);
        }

        //SP theo chu de
        public ActionResult SPTheoChuDe(int id)
        {
            var dsSachTheoChuDe = database.SACHes.Where(sach => sach.MaCD == id).ToList();
            return View("Index", dsSachTheoChuDe);
        }

        //SP theo NXB
        public ActionResult SPTheoNXB(int id)
        {
            var dsNhaXB = database.SACHes.Where(sach => sach.MaNXB == id).ToList();
            return View("Index", dsNhaXB);
        }

        //Details
        public ActionResult Details(int id)
        {
            var sach = database.SACHes.FirstOrDefault(s => s.MaSach == id);
            return View(sach);
        }
    }

}