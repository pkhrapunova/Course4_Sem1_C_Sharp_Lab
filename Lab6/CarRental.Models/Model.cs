using System;

namespace CarRental.Models
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

	public class Car
	{
		public int CarID { get; set; }
		public string PlateNumber { get; set; }
		public string Brand { get; set; }
		public int Mileage { get; set; }
		public string Status { get; set; }
		public int Seats { get; set; }
		public decimal PricePerHour { get; set; }
	}


	public class Order
	{
		public int OrderID { get; set; }
		public int CarID { get; set; }
		public int CustomerID { get; set; }
		public string EmployeeFullName { get; set; }
		public DateTime OrderDate { get; set; }
		public TimeSpan OrderTime { get; set; }
		public DateTime ReturnDate { get; set; }
		public int Hours { get; set; }
	}
}
