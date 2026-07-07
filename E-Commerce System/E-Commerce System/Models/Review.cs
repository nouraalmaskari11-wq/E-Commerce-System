using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_System.Models
{
    [Table("Reviews")]
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int reviewId { get; set; }// system generated

        [Required]
        [ForeignKey("User")]
        public int userId { get; set; } // foreign key

        [Required]
        [ForeignKey("Product")]
        public int productId { get; set; }  // from list // foreign key

        [Required]
        [Range(1,5)]
        public int rating { get; set; } // user input

        [MaxLength(1000)]
        public string? comment { get; set; } // user input

        [Required]
        public DateTime reviewDate { get; set; }// system generated

        //navigation
        public virtual User User { get; set; }
        public virtual Product Product { get; set; }

    }
}
