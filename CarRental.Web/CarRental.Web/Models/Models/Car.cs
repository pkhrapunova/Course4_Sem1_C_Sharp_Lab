using System.Collections.Generic;

namespace CarRental.Web.Models.Models
{
	public class Car
	{
		public int CarID { get; set; }
		public string CarNumber { get; set; }
		public string Make { get; set; }
		public int Mileage { get; set; }
		public string Status { get; set; }
		public int Seats { get; set; }
		public decimal PricePerHour { get; set; }
		public virtual ICollection<Order> Orders { get; set; }
	}
}