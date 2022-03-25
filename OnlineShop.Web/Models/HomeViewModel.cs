using OnlineShop.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Web.Models
{
    public class HomeViewModel
    {
        public IEnumerable<SlideViewModel> slideView { get; set; }
        public IEnumerable<ProductViewModel> LastestLaptop { set; get; }

        public IEnumerable<ProductViewModel> lastestPhone { get; set; }
        public IEnumerable<ProductViewModel> TopSaleProducts { set; get; }

        public string Title { set; get; }
        public string MetaKeyword { set; get; }
        public string MetaDescription { set; get; }



    }
}