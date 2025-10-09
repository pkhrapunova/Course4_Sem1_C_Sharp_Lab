using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.Data.Models;
using System.Data.Entity;

namespace CarRental.Data
{
	public class CarRepository : IRepository<Car>
	{
		private readonly CarRentalDbContext _context;

		public CarRepository()
		{
			_context = new CarRentalDbContext();
		}

		// Получить все автомобили
		public IEnumerable<Car> GetAll()
		{
			try
			{
				return _context.Cars.ToList();
			}
			catch (Exception ex)
			{
				throw new Exception($"Ошибка при загрузке автомобилей: {ex.Message}", ex);
			}
		}

		public List<PopularCar> GetPopularCars()
		{
			var popularCars = _context.Cars
				.Select(c => new PopularCar
				{
					CarID = c.CarID,
					CarNumber = c.CarNumber,
					Make = c.Make,
					Status = c.Status,
					PricePerHour = c.PricePerHour,
					OrderCount = c.Orders.Count,
					TotalRentalHours = c.Orders.Sum(o => o.Hours),
					AverageRentalHours = c.Orders.Any() ? c.Orders.Average(o => (double)o.Hours) : 0
				})
				.OrderByDescending(c => c.OrderCount)
				.ToList();

			return popularCars;
		}

		// Получить автомобиль по ID
		public Car GetById(int carId)
		{
			return _context.Cars.Find(carId);
		}

		// Добавление автомобиля
		public void Insert(Car car)
		{
			_context.Cars.Add(car);
			_context.SaveChanges();
		}

		// Обновление автомобиля - ИСПРАВЛЕНО для EF6
		public void Update(Car car)
		{
			var existingCar = _context.Cars.Find(car.CarID);
			if (existingCar != null)
			{
				_context.Entry(existingCar).CurrentValues.SetValues(car);
				_context.SaveChanges();
			}
		}

		public void Delete(int carId)
		{
			try
			{
				var car = _context.Cars.Find(carId);
				if (car != null)
				{
					bool hasOrders = _context.Orders.Any(o => o.CarID == carId);
					if (hasOrders)
					{
						throw new Exception("Нельзя удалить автомобиль, так как у него есть заказы.");
					}

					_context.Cars.Remove(car);
					_context.SaveChanges();
				}
			}
			catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
			{
				if (ex.InnerException?.Message?.Contains("REFERENCE") == true ||
					ex.InnerException?.InnerException?.Message?.Contains("REFERENCE") == true)
				{
					throw new Exception("Нельзя удалить этот автомобиль, так как он связан с заказами.");
				}
				throw new Exception($"Ошибка при удалении автомобиля: {ex.Message}", ex);
			}
			catch (Exception ex)
			{
				throw new Exception($"Ошибка при удалении автомобиля: {ex.Message}", ex);
			}
		}

		public IEnumerable<Car> GetAvailableCars()
		{
			return _context.Cars
				.Where(c => c.Status == "Свободна")
				.ToList();
		}

		public void UpdateCarStatus(int carId, string status)
		{
			var car = _context.Cars.Find(carId);
			if (car != null)
			{
				car.Status = status;
				_context.SaveChanges();
			}
		}

		public List<CarCurrentMonth> GetCarsCurrentMonth()
		{
			var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
			var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

			var cars = _context.Cars
				.Select(c => new CarCurrentMonth
				{
					CarID = c.CarID,
					CarNumber = c.CarNumber,
					Make = c.Make,
					TotalHoursThisMonth = c.Orders
						.Where(o => o.OrderDate >= startOfMonth && o.OrderDate <= endOfMonth)
						.Sum(o => o.Hours)
				})
				.Where(c => c.TotalHoursThisMonth > 0)
				.ToList();

			return cars;
		}
	}
}