using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Model.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        [MaxLength(256)]
        public string CustomerName { get; set; }
        [Required]
        [MaxLength(256)]
        public string CustomerAddress { get; set; }
        [Required]
        [MaxLength(256)]
        public string CustomerEmail { get; set; }
        [Required]
        [MaxLength(256)]
        public string CustomerMobile { get; set; }
   
        [MaxLength(256)]
        public string CustomerMessage { get; set; }
        public DateTime? CreatedDate { set; get; }

        [MaxLength(256)]
        public string CreatedBy { set; get; }
        public string PaymentMethod { set; get; }
        public string PaymentStatus { set; get; }
        public bool Status { set; get; }

        [StringLength(128)]
        [Column(TypeName ="nvarchar")]
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual ApplicationUser User { set; get; }
 
        public virtual IEnumerable<OrderDetail> OrderDetails { get; set; }

    }
}
