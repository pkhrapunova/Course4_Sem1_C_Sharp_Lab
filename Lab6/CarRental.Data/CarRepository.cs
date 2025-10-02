using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CarRental.Models;

namespace CarRental.Data
{
	public class CarRepository
	{
		private readonly string _connectionString;

		public CarRepository()
		{
			_connectionString = ConfigurationManager.ConnectionStrings["CarRentalDb"].ConnectionString;
		}

		public List<Car> GetAll()
		{
			var list = new List<Car>();
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
							PlateNumber = reader["PlateNumber"].ToString(),
							Brand = reader["Brand"].ToString(),
							Status = reader["Status"].ToString(),
							PricePerHour = (decimal)reader["PricePerHour"]
						});
					}
				}
			}
			return list;
		}
	}
}
