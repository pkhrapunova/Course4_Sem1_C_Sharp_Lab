using System.Collections.Generic;

namespace CarRental.Data.Models
{
	public class Service
	{
		public int ServiceID { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public virtual ICollection<CarService> CarServices { get; set; }

	}
}
