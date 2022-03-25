using OnlineShop.Model.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace OnlineShop.Model.Models
{
    [Table("Products")]
    public class Product : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        public string Alias { get; set; }
        [Required]
        public int CategoryID { get; set; }
        [MaxLength(256)]
        public string Image { get; set; }
        [Column(TypeName = "xml")]
        public string MoreImages { get; set; }

        public Decimal Price { get; set; }
        public Decimal? PromotionPrice { get; set; }

        public int? Warranty { set; get; }
        [MaxLength(256)]
        public string Description { set; get; }
        public string Content { set; get; }
        public bool? HomeFlag { set; get; }
        public bool? HotFlag { set; get; }
        public int? ViewCount { get; set; }
        public decimal OriginalPrice { set; get; }
        public int? Quantity { get; set; }
        public string Tags { set; get; }

        public string Color { get; set; }
        public string Model { get; set; }
        public string whereProduct { get; set; }


        [ForeignKey("CategoryID")]
        public virtual ProductCategory ProductCategory { set; get; }

        public virtual IEnumerable<ProductTag> ProductTags { get; set; }


        //public virtual IEquatable<Tag> Tags { get; set; }




    }
}