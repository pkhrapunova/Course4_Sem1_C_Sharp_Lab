using System.Data.Entity;
using CarRental.Web.Models.Interfaces;
using CarRental.Web.Models.Models;

namespace CarRental.Web.Models.Repositories
{	
	public class CarRepository : IRepository<Csar>
	{
		private readonly CarRentalDbContext _context;

		public CarRepository()
		{
			_context = new CarRentalDbContext();
		}

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
		public void Insert(Car car)
		{
			_context.Cars.Add(car);
			_context.SaveChanges();
		}
		public void Update(Car car)
		{
			var existingCar = _context.Cars.Find(car.CarID);
			if (existingCar != null)
			{
				_context.Entry(existingCar).CurrentValues.SetValues(car);

				_context.Entry(existingCar).State = EntityState.Modified;
				_context.SaveChanges();
			}
			else
			{
				throw new ArgumentException($"Автомобиль с ID {car.CarID} не найден");
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
		public Car GetById(int carId)
		{
			return _context.Cars
				.Include(c => c.Orders) 
				.FirstOrDefault(c => c.CarID == carId);
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