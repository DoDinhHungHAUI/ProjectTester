using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Model.Models
{
    [Table("OrderDetails")]
    public class OrderDetail
    {
        [Key]
        [Column(Order =1)]
        public int OrderID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { set; get; }

        [ForeignKey("ProductID")]
        public virtual Product Products { set; get; }

        [ForeignKey("OrderID")]
        public virtual  Order Orders { get; set; }

    }
}
