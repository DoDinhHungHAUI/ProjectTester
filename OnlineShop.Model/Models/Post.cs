using OnlineShop.Model.Abstract;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace OnlineShop.Model.Models
{
    [Table("Posts")]
    public class Post : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(256)]
        
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        [Column(TypeName = "varchar")]
        public string Alias { get; set; }
        [Required]
        public int CategoryID { get; set; }
        [MaxLength(256)]
        public string Image { get; set; }
        //public XElement MoreImages { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public string Content { get; set; }
        public bool? HomeFlag { set; get; }
        public bool? HotFlag { set; get; }
        public int? ViewCount { get; set; }

        [ForeignKey("CategoryID")]
        public virtual PostCategories PostCategories { set; get; }

        public virtual IEnumerable<PostTag> PostTags { set; get; }

    }
}