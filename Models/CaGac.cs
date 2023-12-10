using System;
using System.Collections.Generic;

namespace MyPhamCheilinus.Models;

public partial class CaGac
{
    public int MaCaGac { get; set; }

    public TimeSpan? TuGio { get; set; }

    public TimeSpan? DenGio { get; set; }

    public virtual ICollection<Pcgac> Pcgacs { get; set; } = new List<Pcgac>();
}
