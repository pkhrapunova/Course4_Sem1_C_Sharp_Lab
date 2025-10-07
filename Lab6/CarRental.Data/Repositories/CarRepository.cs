using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using CarRental.Data.Models;

namespace CarRental.Data
{
	public class CarRepository : IRepository<Car>
	{
		private readonly string _connectionString;

		public CarRepository()
		{
			_connectionString = ConfigurationManager.ConnectionStrings["CarRentalDb"].ConnectionString;
		}

		// Получить все автомобили
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
							list.Add(MapCar(reader));
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
		public List<PopularCar> GetPopularCars()
		{
			var popularCars = new List<PopularCar>();

			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand("sp_GetPopularCars", conn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				conn.Open();

				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						popularCars.Add(new PopularCar
						{
							CarID = (int)reader["CarID"],
							CarNumber = reader["CarNumber"].ToString(),
							Make = reader["Make"].ToString(),
							Status = reader["Status"].ToString(),
							PricePerHour = (decimal)reader["PricePerHour"],
							OrderCount = (int)reader["OrderCount"],
							TotalRentalHours = (int)reader["TotalRentalHours"],
							AverageRentalHours = Convert.ToDouble(reader["AverageRentalHours"])
						});
					}
				}
			}

			return popularCars;
		}


		// Получить автомобиль по ID
		public Car GetById(int carId)
		{
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand("sp_GetCarById", conn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@CarID", carId);
				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						return MapCar(reader);
					}
				}
			}
			return null;
		}

		// Добавление автомобиля
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

		// Обновление автомобиля
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

		// Удаление автомобиля
		public void Delete(int carId)
		{
			try
			{
				using (var conn = new SqlConnection(_connectionString))
				using (var cmd = new SqlCommand("sp_DeleteCar", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@CarID", carId);
					conn.Open();
					cmd.ExecuteNonQuery();
				}
			}
			catch (SqlException ex) when (ex.Number == 547)
			{
				throw new Exception("Нельзя удалить этот автомобиль, так как он связан с заказами.");
			}
			catch (Exception ex)
			{
				throw new Exception($"Ошибка при удалении автомобиля: {ex.Message}", ex);
			}
		}

		// Получить все доступные автомобили
		public IEnumerable<Car> GetAvailableCars()
		{
			var list = new List<Car>();
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand("sp_GetAvailableCars", conn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						list.Add(MapCar(reader));
					}
				}
			}
			return list;
		}

		// Обновление статуса автомобиля
		public void UpdateCarStatus(int carId, string status)
		{
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand("sp_UpdateCarStatus", conn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@CarID", carId);
				cmd.Parameters.AddWithValue("@Status", status);
				conn.Open();
				cmd.ExecuteNonQuery();
			}
		}
		public List<CarCurrentMonth> GetCarsCurrentMonth()
		{
			var cars = new List<CarCurrentMonth>();

			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand("sp_GetCarsCurrentMonth", conn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				conn.Open();

				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						cars.Add(new CarCurrentMonth
						{
							CarID = (int)reader["CarID"],
							CarNumber = reader["CarNumber"].ToString(),
							Make = reader["Make"].ToString(),
							TotalHoursThisMonth = (int)reader["TotalHoursThisMonth"]
						});
					}
				}
			}

			return cars;
		}


		// Вспомогательный метод для маппинга Car из SqlDataReader
		private Car MapCar(SqlDataReader reader)
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
