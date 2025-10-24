namespace CarRental.Web.Models.Models
{
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
