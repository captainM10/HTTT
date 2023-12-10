using System;
using System.Collections.Generic;

namespace CanhGac.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string? UserName { get; set; }

    public string? TenDonVi { get; set; }

    public string? Pasword { get; set; }

    public string? Phone { get; set; }

    public int? RoleId { get; set; }

    public DateTime? LastLogin { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? Salt { get; set; }

    public virtual Role? Role { get; set; }
}
