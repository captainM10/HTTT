using System;
using System.Collections.Generic;

namespace MyPhamCheilinus.Models;

public partial class ChucVu
{
    public string MaChucVu { get; set; } = null!;

    public string TenChucVu { get; set; } = null!;

    public virtual ICollection<HocVien> HocViens { get; set; } = new List<HocVien>();
}
