using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Web.Models.Models
{
	[Table("Customers")]
	public class Customer
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CustomerID { get; set; }

		[Required]
		[Display(Name ="ФИО")]
		public string FullName { get; set; }

		[Required]
		[Display(Name ="Серия паспорта")]    
		public string Passport { get; set; }

		[Required]
		[Display(Name ="Телефон")]
		[Phone]
		public string Phone { get; set; }

		[Required]
		[Display(Name ="Водительское удостоверение")]
		public string DrivingLicense { get; set; }

		public virtual ICollection<Order> Orders { get; set; }
	}
}