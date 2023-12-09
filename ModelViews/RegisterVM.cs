using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyPhamCheilinus.ModelViews
{
    public class RegisterVM
    {
        [Key]
        public int MaDonVi { get; set; }
        //Tên đơn vi
        [Display(Name = "Tên đơn vị")]
        [Required(ErrorMessage = "Vui lòng nhập tên đơn vị")]
        public string TenDonVi { get; set; }
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        //Tên đăng nhập
        [MaxLength(150)]
        [Remote(action: "ValidateTaiKhoan", controller: "Accounts")]
        public string TenDangNhap { get; set; }
        //Số điện thoại
        [MaxLength(11)]
        [Required(ErrorMessage = "Vui lòng nhập Số điện thoại")]
        [Display(Name = "Điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [Remote(action: "ValidatePhone", controller: "Accounts")]
        public string SoDienThoai { get; set; }
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
        public string Password { get; set; }
        [MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Vui lòng nhập mật khẩu giống nhau")]
        public string ConfirmPassword { get; set; }
    }
}
