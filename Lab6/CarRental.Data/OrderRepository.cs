using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CarRental.Models;

namespace CarRental.Data
{
	public class OrderRepository : IRepository<Order>
	{
		private readonly string _connectionString;

		public OrderRepository()
		{
			_connectionString = ConfigurationManager.ConnectionStrings["CarRentalDb"].ConnectionString;
		}

		// ИСПРАВЛЕНИЕ: Возвращаем IEnumerable<Order> вместо List<Order>
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
						list.Add(new Order
						{
							OrderID = (int)reader["OrderID"],
							CustomerID = (int)reader["CustomerID"],
							CarID = (int)reader["CarID"],
							EmployeeFullName = reader["EmployeeFullName"].ToString(),
							OrderDate = (DateTime)reader["OrderDate"],
							OrderTime = (TimeSpan)reader["OrderTime"],
							ReturnDate = (DateTime)reader["ReturnDate"],
							Hours = (int)reader["Hours"]
						});
					}
				}
			}
			return list;
		}

		// НОВЫЙ МЕТОД: Получение заказов с информацией о клиентах и автомобилях
		public List<OrderViewModel> GetAllWithDetails()
		{
			var list = new List<OrderViewModel>();
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand(@"
                SELECT o.*, c.FullName as CustomerName, car.CarNumber, car.PricePerHour,
                       (car.PricePerHour * o.Hours) as TotalPrice
                FROM [Order] o
                INNER JOIN Customer c ON o.CustomerID = c.CustomerID
                INNER JOIN Car car ON o.CarID = car.CarID
                ORDER BY o.OrderDate DESC, o.OrderTime DESC", conn))
			{
				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						list.Add(new OrderViewModel
						{
							OrderID = (int)reader["OrderID"],
							CustomerName = reader["CustomerName"].ToString(),
							CarNumber = reader["CarNumber"].ToString(),
							EmployeeFullName = reader["EmployeeFullName"].ToString(),
							OrderDate = (DateTime)reader["OrderDate"],
							OrderTime = (TimeSpan)reader["OrderTime"],
							ReturnDate = (DateTime)reader["ReturnDate"],
							Hours = (int)reader["Hours"],
							TotalPrice = (decimal)reader["TotalPrice"]
						});
					}
				}
			}
			return list;
		}

		public Order GetById(int orderId)
		{
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand("SELECT * FROM [Order] WHERE OrderID = @OrderID", conn))
			{
				cmd.Parameters.AddWithValue("@OrderID", orderId);
				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						return new Order
						{
							OrderID = (int)reader["OrderID"],
							CustomerID = (int)reader["CustomerID"],
							CarID = (int)reader["CarID"],
							EmployeeFullName = reader["EmployeeFullName"].ToString(),
							OrderDate = (DateTime)reader["OrderDate"],
							OrderTime = (TimeSpan)reader["OrderTime"],
							ReturnDate = (DateTime)reader["ReturnDate"],
							Hours = (int)reader["Hours"]
						};
					}
				}
			}
			return null;
		}

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
				throw new Exception("Нельзя удалить этот элемент, так как он связан с другими данными в системе.");
			}
			catch (Exception ex)
			{
				throw new Exception($"Ошибка при удалении: {ex.Message}", ex);
			}
		}

		// НОВЫЙ МЕТОД: Создание заказа с транзакцией и проверкой доступности
		public void CreateOrderWithTransaction(Order order)
		{
			using (var conn = new SqlConnection(_connectionString))
			{
				conn.Open();
				using (var transaction = conn.BeginTransaction())
				{
					try
					{
						// Проверяем доступность автомобиля
						using (var checkCmd = new SqlCommand(
							"SELECT Status FROM Car WHERE CarID = @CarID", conn, transaction))
						{
							checkCmd.Parameters.AddWithValue("@CarID", order.CarID);
							var status = checkCmd.ExecuteScalar()?.ToString();

							if (status != "Свободна")
							{
								throw new Exception("Автомобиль недоступен для аренды");
							}
						}

						// Создаем заказ
						using (var insertCmd = new SqlCommand("sp_InsertOrder", conn, transaction))
						{
							insertCmd.CommandType = CommandType.StoredProcedure;
							insertCmd.Parameters.AddWithValue("@CarID", order.CarID);
							insertCmd.Parameters.AddWithValue("@CustomerID", order.CustomerID);
							insertCmd.Parameters.AddWithValue("@EmployeeFullName", order.EmployeeFullName);
							insertCmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
							insertCmd.Parameters.AddWithValue("@OrderTime", order.OrderTime);
							insertCmd.Parameters.AddWithValue("@ReturnDate", order.ReturnDate);
							insertCmd.Parameters.AddWithValue("@Hours", order.Hours);
							insertCmd.ExecuteNonQuery();
						}

						// Обновляем статус автомобиля
						using (var updateCmd = new SqlCommand(
							"UPDATE Car SET Status = 'Арендована' WHERE CarID = @CarID", conn, transaction))
						{
							updateCmd.Parameters.AddWithValue("@CarID", order.CarID);
							updateCmd.ExecuteNonQuery();
						}

						transaction.Commit();
					}
					catch
					{
						transaction.Rollback();
						throw;
					}
				}
			}
		}


		public List<CustomerOrderInfo> GetCustomersByOrderDate(DateTime orderDate)
		{
			var list = new List<CustomerOrderInfo>();
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand(@"
                SELECT DISTINCT 
                    c.CustomerID,
                    c.FullName,
                    c.Phone,
                    car.CarNumber,
                    car.Make,
                    o.OrderDate,
                    o.OrderTime,
                    o.Hours,
                    (car.PricePerHour * o.Hours) as TotalPrice
                FROM Customer c
                INNER JOIN [Order] o ON c.CustomerID = o.CustomerID
                INNER JOIN Car car ON o.CarID = car.CarID
                WHERE o.OrderDate = @OrderDate
                ORDER BY o.OrderTime", conn))
			{
				cmd.Parameters.AddWithValue("@OrderDate", orderDate.Date);
				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						list.Add(new CustomerOrderInfo
						{
							CustomerID = (int)reader["CustomerID"],
							FullName = reader["FullName"].ToString(),
							Phone = reader["Phone"].ToString(),
							CarNumber = reader["CarNumber"].ToString(),
							Make = reader["Make"].ToString(),
							OrderDate = (DateTime)reader["OrderDate"],
							OrderTime = (TimeSpan)reader["OrderTime"],
							Hours = (int)reader["Hours"],
							TotalPrice = (decimal)reader["TotalPrice"]
						});
					}
				}
			}
			return list;
		}

		// ЗАПРОС 2: Вывести список самых популярных машин (заказанных более 2 раз)
		public List<PopularCar> GetPopularCars(int minOrders = 2)
		{
			var list = new List<PopularCar>();
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand(@"
                SELECT 
                    car.CarID,
                    car.CarNumber,
                    car.Make,
                    car.Status,
                    car.PricePerHour,
                    COUNT(o.OrderID) as OrderCount,
                    SUM(o.Hours) as TotalRentalHours,
                    AVG(o.Hours) as AverageRentalHours
                FROM Car car
                INNER JOIN [Order] o ON car.CarID = o.CarID
                GROUP BY car.CarID, car.CarNumber, car.Make, car.Status, car.PricePerHour
                HAVING COUNT(o.OrderID) > @MinOrders
                ORDER BY OrderCount DESC, TotalRentalHours DESC", conn))
			{
				cmd.Parameters.AddWithValue("@MinOrders", minOrders);
				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						list.Add(new PopularCar
						{
							CarID = (int)reader["CarID"],
							CarNumber = reader["CarNumber"].ToString(),
							Make = reader["Make"].ToString(),
							Status = reader["Status"].ToString(),
							PricePerHour = (decimal)reader["PricePerHour"],
							OrderCount = (int)reader["OrderCount"],
							TotalRentalHours = reader["TotalRentalHours"] != DBNull.Value ? (int)reader["TotalRentalHours"] : 0,
							AverageRentalHours = reader["AverageRentalHours"] != DBNull.Value ? (double)reader["AverageRentalHours"] : 0
						});
					}
				}
			}
			return list;
		}

		// ЗАПРОС 3: Вывести список заказов заданного заказчика
		public List<CustomerOrderDetail> GetOrdersByCustomer(int customerId)
		{
			var list = new List<CustomerOrderDetail>();
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand(@"
                SELECT 
                    o.OrderID,
                    o.OrderDate,
                    o.OrderTime,
                    o.ReturnDate,
                    o.Hours,
                    o.EmployeeFullName,
                    car.CarNumber,
                    car.Make,
                    car.PricePerHour,
                    (car.PricePerHour * o.Hours) as TotalPrice,
                    c.FullName as CustomerName
                FROM [Order] o
                INNER JOIN Car car ON o.CarID = car.CarID
                INNER JOIN Customer c ON o.CustomerID = c.CustomerID
                WHERE o.CustomerID = @CustomerID
                ORDER BY o.OrderDate DESC, o.OrderTime DESC", conn))
			{
				cmd.Parameters.AddWithValue("@CustomerID", customerId);
				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						list.Add(new CustomerOrderDetail
						{
							OrderID = (int)reader["OrderID"],
							OrderDate = (DateTime)reader["OrderDate"],
							OrderTime = (TimeSpan)reader["OrderTime"],
							ReturnDate = (DateTime)reader["ReturnDate"],
							Hours = (int)reader["Hours"],
							EmployeeFullName = reader["EmployeeFullName"].ToString(),
							CarNumber = reader["CarNumber"].ToString(),
							Make = reader["Make"].ToString(),
							PricePerHour = (decimal)reader["PricePerHour"],
							TotalPrice = (decimal)reader["TotalPrice"],
							CustomerName = reader["CustomerName"].ToString()
						});
					}
				}
			}
			return list;
		}

		// ЗАПРОС 4: Вывести список машин с указанием количества часов проката за текущий месяц
		public List<CarRentalHours> GetCarsRentalHoursCurrentMonth()
		{
			var list = new List<CarRentalHours>();
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand(@"
                SELECT 
                    car.CarID,
                    car.CarNumber,
                    car.Make,
                    car.Status,
                    car.PricePerHour,
                    ISNULL(SUM(o.Hours), 0) as RentalHoursThisMonth,
                    COUNT(o.OrderID) as OrderCountThisMonth,
                    ISNULL(SUM(car.PricePerHour * o.Hours), 0) as RevenueThisMonth
                FROM Car car
                LEFT JOIN [Order] o ON car.CarID = o.CarID 
                    AND YEAR(o.OrderDate) = YEAR(GETDATE()) 
                    AND MONTH(o.OrderDate) = MONTH(GETDATE())
                GROUP BY car.CarID, car.CarNumber, car.Make, car.Status, car.PricePerHour
                ORDER BY RentalHoursThisMonth DESC, RevenueThisMonth DESC", conn))
			{
				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						list.Add(new CarRentalHours
						{
							CarID = (int)reader["CarID"],
							CarNumber = reader["CarNumber"].ToString(),
							Make = reader["Make"].ToString(),
							Status = reader["Status"].ToString(),
							PricePerHour = (decimal)reader["PricePerHour"],
							RentalHoursThisMonth = (int)reader["RentalHoursThisMonth"],
							OrderCountThisMonth = (int)reader["OrderCountThisMonth"],
							RevenueThisMonth = (decimal)reader["RevenueThisMonth"]
						});
					}
				}
			}
			return list;
		}

		// ЗАПРОС 5: Вывести список ФИО заказчиков с указанием дат заказа и общей стоимостью проката
		// (группирующий отчет с итогами)
		public List<CustomerTotalReport> GetCustomerTotalReport()
		{
			var list = new List<CustomerTotalReport>();
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand(@"
                SELECT 
                    c.CustomerID,
                    c.FullName,
                    c.Phone,
                    COUNT(o.OrderID) as TotalOrders,
                    SUM(o.Hours) as TotalHours,
                    SUM(car.PricePerHour * o.Hours) as TotalSpent,
                    MIN(o.OrderDate) as FirstOrderDate,
                    MAX(o.OrderDate) as LastOrderDate
                FROM Customer c
                LEFT JOIN [Order] o ON c.CustomerID = o.CustomerID
                LEFT JOIN Car car ON o.CarID = car.CarID
                GROUP BY c.CustomerID, c.FullName, c.Phone
                HAVING COUNT(o.OrderID) > 0
                ORDER BY TotalSpent DESC", conn))
			{
				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						list.Add(new CustomerTotalReport
						{
							CustomerID = (int)reader["CustomerID"],
							FullName = reader["FullName"].ToString(),
							Phone = reader["Phone"].ToString(),
							TotalOrders = reader["TotalOrders"] != DBNull.Value ? (int)reader["TotalOrders"] : 0,
							TotalHours = reader["TotalHours"] != DBNull.Value ? (int)reader["TotalHours"] : 0,
							TotalSpent = reader["TotalSpent"] != DBNull.Value ? (decimal)reader["TotalSpent"] : 0,
							FirstOrderDate = reader["FirstOrderDate"] != DBNull.Value ? (DateTime)reader["FirstOrderDate"] : DateTime.MinValue,
							LastOrderDate = reader["LastOrderDate"] != DBNull.Value ? (DateTime)reader["LastOrderDate"] : DateTime.MinValue
						});
					}
				}
			}
			return list;
		}

		// Метод для получения общего итога по всем клиентам
		public CustomerTotalReport GetGrandTotal()
		{
			using (var conn = new SqlConnection(_connectionString))
			using (var cmd = new SqlCommand(@"
                SELECT 
                    COUNT(o.OrderID) as TotalOrders,
                    SUM(o.Hours) as TotalHours,
                    SUM(car.PricePerHour * o.Hours) as TotalSpent
                FROM [Order] o
                INNER JOIN Car car ON o.CarID = car.CarID", conn))
			{
				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						return new CustomerTotalReport
						{
							FullName = "ВСЕГО ПО ВСЕМ КЛИЕНТАМ",
							TotalOrders = reader["TotalOrders"] != DBNull.Value ? (int)reader["TotalOrders"] : 0,
							TotalHours = reader["TotalHours"] != DBNull.Value ? (int)reader["TotalHours"] : 0,
							TotalSpent = reader["TotalSpent"] != DBNull.Value ? (decimal)reader["TotalSpent"] : 0
						};
					}
				}
			}
			return null;
		}
	}
}