using System;

namespace CarRental.Data.Models
{
	public class Order
	{
		public int OrderID { get; set; }             
		public string OrderNumber { get; set; }      
		public int CarID { get; set; }            
		public DateTime OrderDate { get; set; }     
		public TimeSpan OrderTime { get; set; }     
		public string EmployeeFullName { get; set; } 
		public int CustomerID { get; set; }         
		public DateTime ReturnDate { get; set; }     
		public int Hours { get; set; }

	}
}
