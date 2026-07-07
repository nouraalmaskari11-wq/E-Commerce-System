using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_System.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userId {  get; set; }// system generated

        [Required]
        [MaxLength(50)]
        [Index(IsUnique = true)]
        public string username { get; set; } // user input

        [Required]
        [MaxLength(150)]
        [Index(IsUnique = true)]
        [EmailAddress]
        public string email { get; set; } // user input

        [Required]
        [MaxLength(256)]
        public string passwordHash { get; set; } // user input

        [Required]
        [MaxLength(100)]
        public string fullName { get; set; } // user input

        [MaxLength(20)]
        public string? phoneNumber { get; set; } // user input (optional)

        [MaxLength(300)]
        public string? address { get; set; } // user input (optional)

        [Required]
        public DateTime registrationDate { get; set; }// system generated

       
        public bool isActive { get; set; } = true;// default value


        // navigation properties
        public virtual List<Order> Orders { get; set; }
        public virtual List<Review> Reviews { get; set; }
        

    }
}
