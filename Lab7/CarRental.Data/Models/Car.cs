using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.Models
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
	}
}
