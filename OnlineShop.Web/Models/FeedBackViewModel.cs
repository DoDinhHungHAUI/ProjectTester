using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Web.Models
{
    public class FeedBackViewModel
    {

        public int ID { set; get; }
        [MaxLength(250, ErrorMessage = "Tên không được quá 250 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string Name { set; get; }
        [MaxLength(250, ErrorMessage = "Email không được quá 250 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email không hợp lệ")]
        public string Email { set; get; }
        [MaxLength(500, ErrorMessage = "Email không được quá 500 ký tự")]
        public string Message { set; get; }
        public DateTime CreatedDate { get; set; }

        public bool Status { get; set; }
        public ContactDetailViewModel ContactDetail { set; get; }

    }
}