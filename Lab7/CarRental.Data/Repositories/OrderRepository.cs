using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CarRental.Data.Models;

namespace CarRental.Data
{
	public class OrderRepository : IRepository<Order>
	{
		private readonly string _connectionString;

		public OrderRepository()
		{
			_connectionString = ConfigurationManager.ConnectionStrings["CarRentalDb"].ConnectionString;
		}

		// Получить все заказы
		public IEnumerable<Order> GetAll()
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
						list.Add(MapOrder(reader));
					}
				}
			}
			return list;
		}

		// Получить заказ по ID
		public Order GetById(int orderId)
		{
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand("sp_GetOrderById", conn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@OrderID", orderId);
				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						return MapOrder(reader);
					}
				}
			}
			return null;
		}

		// Добавление заказа
		public void Insert(Order order)
		{
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand("sp_InsertOrder", conn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@CarID", order.CarID);
				cmd.Parameters.AddWithValue("@CustomerID", order.CustomerID);
				cmd.Parameters.AddWithValue("@EmployeeFullName", order.EmployeeFullName);
				cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
				cmd.Parameters.AddWithValue("@OrderTime", order.OrderTime);
				cmd.Parameters.AddWithValue("@ReturnDate", order.ReturnDate);
				cmd.Parameters.AddWithValue("@Hours", order.Hours);
				conn.Open();
				cmd.ExecuteNonQuery();
			}
		}

		// Обновление заказа
		public void Update(Order order)
		{
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand("sp_UpdateOrder", conn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@OrderID", order.OrderID);
				cmd.Parameters.AddWithValue("@CarID", order.CarID);
				cmd.Parameters.AddWithValue("@CustomerID", order.CustomerID);
				cmd.Parameters.AddWithValue("@EmployeeFullName", order.EmployeeFullName);
				cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
				cmd.Parameters.AddWithValue("@OrderTime", order.OrderTime);
				cmd.Parameters.AddWithValue("@ReturnDate", order.ReturnDate);
				cmd.Parameters.AddWithValue("@Hours", order.Hours);
				conn.Open();
				cmd.ExecuteNonQuery();
			}
		}

		// Удаление заказа
		public void Delete(int orderId)
		{
			try
			{
				using (var conn = new SqlConnection(_connectionString))
				using (var cmd = new SqlCommand("sp_DeleteOrder", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@OrderID", orderId);
					conn.Open();
					cmd.ExecuteNonQuery();
				}
			}
			catch (SqlException ex) when (ex.Number == 547)
			{
				throw new Exception("Нельзя удалить этот заказ, так как он связан с другими данными.");
			}
			catch (Exception ex)
			{
				throw new Exception($"Ошибка при удалении заказа: {ex.Message}", ex);
			}
		}

		// Вспомогательный метод для маппинга
		private Order MapOrder(SqlDataReader reader)
		{
			return new Order
			{
				OrderID = (int)reader["OrderID"],
				CarID = (int)reader["CarID"],
				CustomerID = (int)reader["CustomerID"],
				EmployeeFullName = reader["EmployeeFullName"].ToString(),
				OrderDate = (DateTime)reader["OrderDate"],
				OrderTime = (TimeSpan)reader["OrderTime"],
				ReturnDate = (DateTime)reader["ReturnDate"],
				Hours = (int)reader["Hours"]
			};
		}
	}
}
