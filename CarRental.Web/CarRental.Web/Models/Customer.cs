using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Web.Models
{
    [Table("Customers")]
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerID { get; set; }

        [Required]
        [Display(Name = "ФИО")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Серия паспорта")]
        public string Passport { get; set; }

        [Required]
        [Display(Name = "Телефон")]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Водительское удостоверение")]
        public string DrivingLicense { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        [Display(Name = "Администратор")]
        public bool IsAdmin { get; set; } = false; // по умолчанию обычный пользователь

    }
}