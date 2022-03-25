using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Web.Models
{
    public class ProductViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public int CategoryID { get; set; }
        public string Image { get; set; }
        public string MoreImages { get; set; }
        public Decimal Price { get; set; }
        public Decimal PromotionPrice { get; set; }
        public int? Warranty { set; get; }
        public string Description { set; get; }
        public string Content { set; get; }
        public bool? HomeFlag { set; get; }
        public bool? HotFlag { set; get; }
        public int? ViewCount { get; set; }
        public DateTime? CreatedDate { set; get; }
        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public string UpdatedBy { set; get; }

        public string MetaKeyword { set; get; }

        public string MetaDescription { set; get; }

        public bool Status { set; get; }
        public string Color { get; set; }
        public string Model { get; set; }
        public string whereProduct { get; set; }
        public string Tags { get; set; }
        public int Quantity { get; set; }
        public decimal OriginalPrice { get; set; }
        public virtual ProductCategoryViewModel ProductCategory { set; get; }
    }
}