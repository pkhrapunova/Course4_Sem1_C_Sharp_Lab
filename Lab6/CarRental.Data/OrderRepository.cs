using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CarRental.Models;

namespace CarRental.Data
{
	public class OrderRepository
	{
		private readonly string _connectionString;

		public OrderRepository()
		{
			_connectionString = ConfigurationManager.ConnectionStrings["CarRentalDb"].ConnectionString;
		}

		public List<Order> GetAll()
		{
			var list = new List<Order>();
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand("sp_GetAllOrders", conn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						list.Add(new Order
						{
							OrderID = (int)reader["OrderID"],
							CustomerID = (int)reader["CustomerID"],
							CarID = (int)reader["CarID"],
							OrderDate = (DateTime)reader["OrderDate"],
							Hours = (int)reader["Hours"]
						});
					}
				}
			}
			return list;
		}
	}
}
