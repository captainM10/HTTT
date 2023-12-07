using System;
using System.Collections.Generic;

namespace MyPhamCheilinus.Models1;

public partial class HocVien
{
    public string MaHocVien { get; set; } = null!;

    public string? TenHocVien { get; set; }

    public DateTime? NgaySinh { get; set; }

    public string? MaDaiDoi { get; set; }

    public string? GioiTinh { get; set; }

    public bool? Gac { get; set; }

    public int? SoLanGac { get; set; }

    public string? MaCapBac { get; set; }

    public string? MaChucVu { get; set; }

    public virtual CapBac? MaCapBacNavigation { get; set; }

    public virtual ChucVu? MaChucVuNavigation { get; set; }

    public virtual DonVi? MaDaiDoiNavigation { get; set; }

    public virtual ICollection<Pcgac> Pcgacs { get; set; } = new List<Pcgac>();
}
