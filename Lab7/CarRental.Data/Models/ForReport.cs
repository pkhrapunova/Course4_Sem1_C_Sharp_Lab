using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.Models
{
	public class PopularCar
	{
		public int CarID { get; set; }
		public string CarNumber { get; set; }
		public string Make { get; set; }
		public string Status { get; set; }
		public decimal PricePerHour { get; set; }
		public int OrderCount { get; set; }
		public int TotalRentalHours { get; set; }
		public double AverageRentalHours { get; set; }
	}


	public class CustomerTotalReport
	{
		public int CustomerID { get; set; }
		public string FullName { get; set; }
		public string Phone { get; set; }
		public int TotalOrders { get; set; }
		public int TotalHours { get; set; }
		public decimal TotalSpent { get; set; }
		public DateTime FirstOrderDate { get; set; }
		public DateTime LastOrderDate { get; set; }
	}

	public class CarDisplayModel
	{
		public int CarID { get; set; }
		public string CarNumber { get; set; }
		public string Make { get; set; }
		public int Mileage { get; set; }
		public string Status { get; set; }
		public int Seats { get; set; }
		public decimal PricePerHour { get; set; }
	}

	public class CustomerDisplayModel
	{
		public int CustomerID { get; set; }
		public string FullName { get; set; }
		public string Passport { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public string DrivingLicense { get; set; }
	}
	public class CarCurrentMonth
	{
		public int CarID { get; set; }
		public string CarNumber { get; set; }
		public string Make { get; set; }
		public int TotalHoursThisMonth { get; set; }
	}


}

