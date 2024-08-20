using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcBookStore.Models
{
    public class MatHangMua
    {
        QLSACHEntities1 db = new QLSACHEntities1();
        public int MaSach { get; set; }
        public string TenSach { get; set; }
        public string AnhBia { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }

        //
        public double ThanhTien()
        {
            return SoLuong * DonGia;
        }

        public MatHangMua(int MaSach)
        {
            this.MaSach = MaSach;
            //
            var sach = db.SACHes.Single(s => s.MaSach == this.MaSach);
            this.TenSach = sach.TenSach;
            this.AnhBia = sach.Hinhminhhoa;
            this.DonGia = double.Parse(sach.Dongia.ToString());
            this.SoLuong = 1;
        }
    }
}