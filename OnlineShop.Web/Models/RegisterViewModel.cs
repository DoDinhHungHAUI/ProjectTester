using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Web.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage =  "Bạn cần nhập tên.")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập tên đăng nhập.")]
        public string UserName { set; get; }
        [Required(ErrorMessage = "Bạn cần nhập mật khẩu.")]
        [MinLength(6 , ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string PassWord { set; get; }
       
        [Compare("PassWord" , ErrorMessage = "Mật khẩu phải trùng với mật khẩu đã nhập.")]
        public string ConfirmPassWord { set; get; }
        [Required(ErrorMessage = "Bạn cần nhập mật Email")]
        [MinLength(6, ErrorMessage = "Địa chỉ email không đúng.")]
        public string Email { set; get; }
        [Required(ErrorMessage = "Bạn cần nhập địa chỉ")]
        public string Address { set; get; }
        [Required(ErrorMessage = "Bạn cần nhập số điện thoại.")]
        public string PhoneNumber { set; get; }

    }
}