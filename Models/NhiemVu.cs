﻿using System;
using System.Collections.Generic;

namespace CanhGac.Models;

public partial class NhiemVu
{
    public int MaNhiemVu { get; set; }

    public string? TenNhiemVu { get; set; }

    public virtual ICollection<Pcgac> Pcgacs { get; set; } = new List<Pcgac>();
}
