using System;
using System.Collections.Generic;

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
		public string CarNumber { get; set; }
		public string Make { get; set; }
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

	public class OrderViewModel
	{
		public int OrderID { get; set; }
		public string CustomerName { get; set; }
		public string CarNumber { get; set; }
		public string EmployeeFullName { get; set; }
		public DateTime OrderDate { get; set; }
		public TimeSpan OrderTime { get; set; }
		public DateTime ReturnDate { get; set; }
		public int Hours { get; set; }
		public decimal TotalPrice { get; set; }
	}

	// Модели для статистики
	public class CustomerStatistics
	{
		public int CustomerID { get; set; }
		public string CustomerName { get; set; }
		public int TotalOrders { get; set; }
		public int TotalHours { get; set; }
		public decimal TotalSpent { get; set; }
		public decimal AverageOrderPrice { get; set; }
		public decimal MaxOrderPrice { get; set; }
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

	// Модели для запросов
	public class CustomerOrderInfo
	{
		public int CustomerID { get; set; }
		public string FullName { get; set; }
		public string Phone { get; set; }
		public string CarNumber { get; set; }
		public string Make { get; set; }
		public DateTime OrderDate { get; set; }
		public TimeSpan OrderTime { get; set; }
		public int Hours { get; set; }
		public decimal TotalPrice { get; set; }
	}

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

	public class CustomerOrderDetail
	{
		public int OrderID { get; set; }
		public string CustomerName { get; set; }
		public DateTime OrderDate { get; set; }
		public TimeSpan OrderTime { get; set; }
		public DateTime ReturnDate { get; set; }
		public int Hours { get; set; }
		public string EmployeeFullName { get; set; }
		public string CarNumber { get; set; }
		public string Make { get; set; }
		public decimal PricePerHour { get; set; }
		public decimal TotalPrice { get; set; }
	}

	public class CarRentalHours
	{
		public int CarID { get; set; }
		public string CarNumber { get; set; }
		public string Make { get; set; }
		public string Status { get; set; }
		public decimal PricePerHour { get; set; }
		public int RentalHoursThisMonth { get; set; }
		public int OrderCountThisMonth { get; set; }
		public decimal RevenueThisMonth { get; set; }
	}
}