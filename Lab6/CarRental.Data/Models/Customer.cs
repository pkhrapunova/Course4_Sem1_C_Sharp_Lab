using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.Models
{
	public class Customer
	{
		public int CustomerID { get; set; }
		public string FullName { get; set; }
		public string Passport { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public string DrivingLicense { get; set; }
	}
}
