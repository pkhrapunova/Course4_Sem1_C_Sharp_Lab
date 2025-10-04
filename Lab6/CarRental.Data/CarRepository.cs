using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CarRental.Models;

namespace CarRental.Data
{
	public class CarRepository : IRepository<Car>
	{
		private readonly string _connectionString;

		public CarRepository()
		{
			_connectionString = ConfigurationManager.ConnectionStrings["CarRentalDb"].ConnectionString;
		}

		public IEnumerable<Car> GetAll()
		{
			var list = new List<Car>();
			try
			{
				using (var conn = new SqlConnection(_connectionString))
				using (var cmd = new SqlCommand("sp_GetAllCars", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					conn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							list.Add(new Car
							{
								CarID = (int)reader["CarID"],
								CarNumber = reader["CarNumber"].ToString(),
								Make = reader["Make"].ToString(),
								Mileage = (int)reader["Mileage"],
								Status = reader["Status"].ToString(),
								Seats = (int)reader["Seats"],
								PricePerHour = (decimal)reader["PricePerHour"]
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Ошибка при загрузке автомобилей: {ex.Message}", ex);
			}
			return list;
		}

		public Car GetById(int carId)
		{
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand("SELECT * FROM Car WHERE CarID = @CarID", conn))
			{
				cmd.Parameters.AddWithValue("@CarID", carId);
				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						return new Car
						{
							CarID = (int)reader["CarID"],
							CarNumber = reader["CarNumber"].ToString(),
							Make = reader["Make"].ToString(),
							Mileage = (int)reader["Mileage"],
							Status = reader["Status"].ToString(),
							Seats = (int)reader["Seats"],
							PricePerHour = (decimal)reader["PricePerHour"]
						};
					}
				}
			}
			return null;
		}

		public void Insert(Car car)
		{
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand("sp_InsertCar", conn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@CarNumber", car.CarNumber);
				cmd.Parameters.AddWithValue("@Make", car.Make);
				cmd.Parameters.AddWithValue("@Mileage", car.Mileage);
				cmd.Parameters.AddWithValue("@Status", car.Status);
				cmd.Parameters.AddWithValue("@Seats", car.Seats);
				cmd.Parameters.AddWithValue("@PricePerHour", car.PricePerHour);
				conn.Open();
				cmd.ExecuteNonQuery();
			}
		}

		public void Update(Car car)
		{
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand("sp_UpdateCar", conn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@CarID", car.CarID);
				cmd.Parameters.AddWithValue("@CarNumber", car.CarNumber);
				cmd.Parameters.AddWithValue("@Make", car.Make);
				cmd.Parameters.AddWithValue("@Mileage", car.Mileage);
				cmd.Parameters.AddWithValue("@Status", car.Status);
				cmd.Parameters.AddWithValue("@Seats", car.Seats);
				cmd.Parameters.AddWithValue("@PricePerHour", car.PricePerHour);
				conn.Open();
				cmd.ExecuteNonQuery();
			}
		}

		public void Delete(int id)
		{
			try
			{
				using (var conn = new SqlConnection(_connectionString))
				using (var cmd = new SqlCommand("sp_DeleteCar", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@CarID", id);
					conn.Open();
					cmd.ExecuteNonQuery();
				}
			}
			catch (SqlException ex) when (ex.Number == 547)
			{
				throw new Exception("Нельзя удалить этот элемент, так как он связан с другими данными в системе.");
			}
			catch (Exception ex)
			{
				throw new Exception($"Ошибка при удалении: {ex.Message}", ex);
			}
		}

		// Новый метод для получения доступных автомобилей
		public IEnumerable<Car> GetAvailableCars()
		{
			var list = new List<Car>();
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand("SELECT * FROM Car WHERE Status = 'Свободна'", conn))
			{
				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						list.Add(new Car
						{
							CarID = (int)reader["CarID"],
							CarNumber = reader["CarNumber"].ToString(),
							Make = reader["Make"].ToString(),
							Mileage = (int)reader["Mileage"],
							Status = reader["Status"].ToString(),
							Seats = (int)reader["Seats"],
							PricePerHour = (decimal)reader["PricePerHour"]
						});
					}
				}
			}
			return list;
		}

		// Метод для обновления статуса автомобиля
		public void UpdateCarStatus(int carId, string status)
		{
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand("UPDATE Car SET Status = @Status WHERE CarID = @CarID", conn))
			{
				cmd.Parameters.AddWithValue("@Status", status);
				cmd.Parameters.AddWithValue("@CarID", carId);
				conn.Open();
				cmd.ExecuteNonQuery();
			}
		}
	}
}