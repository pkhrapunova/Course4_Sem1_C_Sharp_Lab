using System.Collections.Generic;

namespace CarRental.Web.Models.Models

{
	public class Customer
	{
		public int CustomerID { get; set; }
		public string FullName { get; set; }
		public string Passport { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public string DrivingLicense { get; set; }

		public virtual ICollection<Order> Orders { get; set; }
	}
}