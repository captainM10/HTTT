using System;
using System.Collections.Generic;

namespace MyPhamCheilinus.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string? Phone { get; set; }

    public string? AccountEmail { get; set; }

    public string? AccountPassword { get; set; }

    public string? Sail { get; set; }

    public bool Active { get; set; }

    public string? FullName { get; set; }

    public int? RoleId { get; set; }

    public DateTime? LastLogin { get; set; }

    public DateTime? CreateDate { get; set; }

    public int? MaKhachHang { get; set; }

    public virtual KhachHang? MaKhachHangNavigation { get; set; }

    public virtual Role? RoleNavigation { get; set; }
}
