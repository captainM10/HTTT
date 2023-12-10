using System;
using System.Collections.Generic;

namespace MyPhamCheilinus.Models;

public partial class Pcgac
{
    public DateTime Ngay { get; set; }

    public string MaHocVien { get; set; } = null!;

    public int MaNhiemVu { get; set; }

    public int MaCaGac { get; set; }

    public virtual CaGac MaCaGacNavigation { get; set; } = null!;

    public virtual HocVien MaHocVienNavigation { get; set; } = null!;

    public virtual NhiemVu MaNhiemVuNavigation { get; set; } = null!;

    public virtual ThongTinGac NgayNavigation { get; set; } = null!;
}
