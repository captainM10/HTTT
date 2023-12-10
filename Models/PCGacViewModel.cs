using System;
using System.Collections.Generic;





namespace CanhGac.Models
{
    public class PCGacViewModel
    {
        public string MaHocVien { get; set; }
        public string TenHocVien { get; set; }
        public string GioiTinh { get; set; }
        public string TenNhiemVu { get; set; }
        public string TenVongGac { get; set; }
        public DateTime Ngay { get; set; }
        public TimeSpan TuGio { get; set; }
        public TimeSpan DenGio { get; set; }
    }
}
