using System;
using System.Collections.Generic;

namespace MyPhamCheilinus.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string? MaDonVi { get; set; }

    public string? Pasword { get; set; }

    public int? RoleId { get; set; }

    public DateTime? LastLogin { get; set; }

    public virtual Role? Role { get; set; }
}
