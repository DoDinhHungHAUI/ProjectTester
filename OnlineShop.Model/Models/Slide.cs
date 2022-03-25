using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Model.Models
{
    [Table("Slides")]
    public class Slide
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }
        [MaxLength(256)]
        public string Desription { get; set; }
        [MaxLength(256)]
        public string Image { get; set; }
        [MaxLength(256)]
        public string Url { get; set; }
        public int DisplayOrder { get; set; }
        public bool Status { set; get; }
        public string Content { get; set; }


    }
}
