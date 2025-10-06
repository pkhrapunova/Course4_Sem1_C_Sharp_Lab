using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CarRental.Data.Models;

namespace CarRental.Data
{
	public class CustomerRepository : IRepository<Customer>
	{
		private readonly string _connectionString;

		public CustomerRepository()
		{
			_connectionString = ConfigurationManager.ConnectionStrings["CarRentalDb"].ConnectionString;
		}

		// Получить всех клиентов
		public IEnumerable<Customer> GetAll()
		{
			var customers = new List<Customer>();
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand("sp_GetAllCustomers", conn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						customers.Add(MapCustomer(reader));
					}
				}
			}
			return customers;
		}

		// Получить клиента по ID
		public Customer GetById(int id)
		{
			Customer customer = null;
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand("sp_GetCustomerById", conn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@CustomerID", id);
				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						customer = MapCustomer(reader);
					}
				}
			}
			return customer;
		}

		// Добавление клиента
		public void Insert(Customer entity)
		{
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand("sp_InsertCustomer", conn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@FullName", entity.FullName);
				cmd.Parameters.AddWithValue("@Passport", entity.Passport);
				cmd.Parameters.AddWithValue("@Address", entity.Address);
				cmd.Parameters.AddWithValue("@Phone", entity.Phone);
				cmd.Parameters.AddWithValue("@DrivingLicense", entity.DrivingLicense);
				conn.Open();
				cmd.ExecuteNonQuery();
			}
		}

		// Обновление клиента
		public void Update(Customer entity)
		{
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand("sp_UpdateCustomer", conn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@CustomerID", entity.CustomerID);
				cmd.Parameters.AddWithValue("@FullName", entity.FullName);
				cmd.Parameters.AddWithValue("@Passport", entity.Passport);
				cmd.Parameters.AddWithValue("@Address", entity.Address);
				cmd.Parameters.AddWithValue("@Phone", entity.Phone);
				cmd.Parameters.AddWithValue("@DrivingLicense", entity.DrivingLicense);
				conn.Open();
				cmd.ExecuteNonQuery();
			}
		}

		// Удаление клиента
		public void Delete(int id)
		{
			try
			{
				using (var conn = new SqlConnection(_connectionString))
				using (var cmd = new SqlCommand("sp_DeleteCustomer", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@CustomerID", id);
					conn.Open();
					cmd.ExecuteNonQuery();
				}
			}
			catch (SqlException ex) when (ex.Number == 547)
			{
				throw new Exception("Нельзя удалить этого клиента, так как он связан с заказами.");
			}
			catch (Exception ex)
			{
				throw new Exception($"Ошибка при удалении клиента: {ex.Message}", ex);
			}
		}

		// Вспомогательный метод для маппинга клиента из SqlDataReader
		private Customer MapCustomer(SqlDataReader reader)
		{
			return new Customer
			{
				CustomerID = (int)reader["CustomerID"],
				FullName = reader["FullName"].ToString(),
				Passport = reader["Passport"].ToString(),
				Address = reader["Address"].ToString(),
				Phone = reader["Phone"].ToString(),
				DrivingLicense = reader["DrivingLicense"].ToString()
			};
		}
		public List<CustomerTotalReport> GetCustomerTotals()
		{
			var report = new List<CustomerTotalReport>();

			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand(@"
        SELECT 
            cu.CustomerID,
            cu.FullName,
            cu.Phone,
            COUNT(o.OrderID) AS TotalOrders,
            ISNULL(SUM(o.Hours),0) AS TotalHours,
            ISNULL(SUM(o.Hours * c.PricePerHour),0) AS TotalSpent,
            MIN(o.OrderDate) AS FirstOrderDate,
            MAX(o.OrderDate) AS LastOrderDate
        FROM Customer cu
        LEFT JOIN [Order] o ON cu.CustomerID = o.CustomerID
        LEFT JOIN Car c ON o.CarID = c.CarID
        GROUP BY cu.CustomerID, cu.FullName, cu.Phone
    ", conn))
			{
				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						report.Add(new CustomerTotalReport
						{
							CustomerID = (int)reader["CustomerID"],
							FullName = reader["FullName"].ToString(),
							Phone = reader["Phone"].ToString(),
							TotalOrders = (int)reader["TotalOrders"],
							TotalHours = (int)reader["TotalHours"],
							TotalSpent = (decimal)reader["TotalSpent"],
							FirstOrderDate = reader["FirstOrderDate"] != DBNull.Value
											 ? (DateTime)reader["FirstOrderDate"]
											 : DateTime.MinValue,
							LastOrderDate = reader["LastOrderDate"] != DBNull.Value
											? (DateTime)reader["LastOrderDate"]
											: DateTime.MinValue
						});
					}
				}
			}

			return report;
		}

	}
}
