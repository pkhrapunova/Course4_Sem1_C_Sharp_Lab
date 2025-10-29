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

        [ForeignKey("Customer")]
        public int CustomerID { get; set; }

        [ForeignKey("Car")]
        public int CarID { get; set; }

        [Required(ErrorMessage = "Выберите дату и время бронирования")]
        public DateTime OrderDate { get; set; }

        [Required]
        [Range(1, 24, ErrorMessage = "Введите корректное количество часов")]
        public int Hours { get; set; }

        public virtual Car? Car { get; set; }
        public virtual Customer? Customer { get; set; }

    }
}