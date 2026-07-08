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
    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int orderId { get; set; }// system generated

        [Required]
        [ForeignKey("User")]
        public int userId { get; set; }// foreign key

        [Required]
        public DateTime orderDate { get; set; }// system generated

        [Required]
        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal totalAmount { get; set; }//calculated

        [Required]
        [MaxLength(30)]
        public string status { get; set; } = "Pending"; // default value

        [Required]
        [MaxLength(300)]
        public string shippingAddress { get; set; } // user input

        [Required]
        [MaxLength(50)]
        public string paymentMethod { get; set; } // user input

        //navigation 
        public virtual User User { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
    }
}
