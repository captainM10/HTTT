using System;
using System.Collections.Generic;

namespace MyPhamCheilinus.Models;

public partial class VongGac
{
    public int MaVongGac { get; set; }

    public string? TenVongGac { get; set; }

    public string? ViTri { get; set; }

    public virtual ICollection<NhiemVu> NhiemVus { get; set; } = new List<NhiemVu>();
}
