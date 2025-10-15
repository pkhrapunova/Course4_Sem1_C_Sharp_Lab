using System;
using System.Collections.Generic;

namespace CarRental.Data.Models
{
	public class Order
	{
		public int OrderID { get; set; }
		public int CustomerID { get; set; }
		public int CarID { get; set; }
		public DateTime OrderDate { get; set; }
		public DateTime? ReturnDate { get; set; }
		public int Hours { get; set; }
		public string EmployeeFullName { get; set; }

		public virtual Customer Customer { get; set; }
		public virtual Car Car { get; set; }
		public Order()
		{
		}
	}
}