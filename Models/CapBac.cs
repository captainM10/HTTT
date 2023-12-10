using System;
using System.Collections.Generic;

namespace MyPhamCheilinus.Models;

public partial class CapBac
{
    public string MaCapBac { get; set; } = null!;

    public string TenCapBac { get; set; } = null!;

    public virtual ICollection<HocVien> HocViens { get; set; } = new List<HocVien>();
}
