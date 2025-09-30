using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CarRental.Models;

namespace CarRental.Data
{
	public class CustomerRepository : IRepository<Customer>
	{
		private readonly string _connectionString;

		public CustomerRepository()
		{
			_connectionString = ConfigurationManager.ConnectionStrings["CarRentalDb"].ConnectionString;
		}

		public IEnumerable<Customer> GetAll()
		{
			var customers = new List<Customer>();
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand("SELECT * FROM Customer", conn))
			{
				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						customers.Add(new Customer
						{
							CustomerID = (int)reader["CustomerID"],
							FullName = reader["FullName"].ToString(),
							Passport = reader["Passport"].ToString(),
							Address = reader["Address"].ToString(),
							Phone = reader["Phone"].ToString(),
							DrivingLicense = reader["DrivingLicense"].ToString()
						});
					}
				}
			}
			return customers;
		}

		public Customer GetById(int id)
		{
			Customer customer = null;
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand("SELECT * FROM Customer WHERE CustomerID = @id", conn))
			{
				cmd.Parameters.AddWithValue("@id", id);
				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						customer = new Customer
						{
							CustomerID = (int)reader["CustomerID"],
							FullName = reader["FullName"].ToString(),
							Passport = reader["Passport"].ToString(),
							Address = reader["Address"].ToString(),
							Phone = reader["Phone"].ToString(),
							DrivingLicense = reader["DrivingLicense"].ToString()
						};
					}
				}
			}
			return customer;
		}

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

		public void Delete(int id)
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
	}
}
