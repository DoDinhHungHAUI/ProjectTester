using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Web.Models
{
    public class EditUserViewModel
    {

        [Required(ErrorMessage = "Bạn cần nhập tên.")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Bạn cần nhập tên đăng nhập.")]
        public string UserName { set; get; }
        [Required(ErrorMessage = "Bạn cần nhập mật khẩu.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string PassWordLast { set; get; }

        [Required(ErrorMessage = "Bạn cần nhập mật khẩu mới.")]
        [MinLength(6, ErrorMessage = "Mật khẩu mới phải có ít nhất 6 ký tự.")]
        public string PassWordNew { set; get; }
        [Required(ErrorMessage = "Bạn cần nhập mật Email")]
        [MinLength(6, ErrorMessage = "Địa chỉ email không đúng.")]
        public string Email { set; get; }
        [Required(ErrorMessage = "Bạn cần nhập địa chỉ")]
        public string Address { set; get; }
        [Required(ErrorMessage = "Bạn cần nhập số điện thoại.")]
        public string PhoneNumber { set; get; }
    }
}