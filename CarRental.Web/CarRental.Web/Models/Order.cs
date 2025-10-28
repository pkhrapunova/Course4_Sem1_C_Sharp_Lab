using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Web.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }

        [Required]
        [ForeignKey("Car")]
        public int CarID { get; set; }
        public DateTime OrderDate { get; set; }
        public int Hours { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Car Car { get; set; }
        public Order()
        {
        }
    }
}