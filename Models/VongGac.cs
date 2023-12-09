using System;
using System.Collections.Generic;

namespace CanhGac.Models;

public partial class VongGac
{
    public int MaVongGac { get; set; }

    public string? TenVongGac { get; set; }

    public string? ViTri { get; set; }

    public virtual ICollection<Pcgac> Pcgacs { get; set; } = new List<Pcgac>();
}
