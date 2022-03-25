using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Web.Models
{
    public class SlideViewModel
    {
        public int ID { set; get; }
        public string Name { get; set; }
        public string Desription { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public int DisplayOrder { get; set; }
        public bool Status { set; get; }
        public string Content { get; set; }

    }
}