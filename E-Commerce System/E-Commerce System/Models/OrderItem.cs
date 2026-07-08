using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace E_Commerce_System.Models
{
    [Table("OrderItem")]
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int orderItemId { get; set; }// system generated

        [Required]
        [Range(1, 999)]
        public int quantity { get; set; } // user input

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal unitPrice { get; set; }

        [Required]
        [ForeignKey("Order")]
        public int orderId { get; set; }// foreign key 

        [Required]
        [ForeignKey("Product")]
        public int productId { get; set; }// foreign key

        //navigation
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }


    }
}
