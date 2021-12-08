using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FationShop.Areas.Admin.Framework
{
    public class CustomerMetadata
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Email không được để trống!")]
        [EmailAddress(ErrorMessage = "Email không chính xác!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống!")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 kí tự.")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không trùng khớp!")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Họ tên không được để trống!")]
        public string DisplayName { get; set; }
        [Required(ErrorMessage = "Địa chỉ không được để trống!")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống!")]
        public string PhoneNumber { get; set; }
    }
}