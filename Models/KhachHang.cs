using System;
using System.Collections.Generic;

namespace MyPhamCheilinus.Models;

public partial class KhachHang
{
    public int MaKhachHang { get; set; }

    public string? TenKhachHang { get; set; }

    public string? DiaChi { get; set; }

    public string? SoDienThoai { get; set; }

    public DateTime? NgaySinh { get; set; }

    public string? Avatar { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public int? DisTrict { get; set; }

    public int? Ward { get; set; }

    public int? LocationId { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? Password { get; set; }

    public string? Salt { get; set; }

    public DateTime? LastLogin { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
