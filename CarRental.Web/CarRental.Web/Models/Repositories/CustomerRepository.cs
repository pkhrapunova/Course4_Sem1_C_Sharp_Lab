using CarRental.Web.Models.Interfaces;
using CarRental.Web.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Web.Models.Repositories
{
	public class CustomerRepository : IRepository<Customer>
	{
		private readonly CarRentalDbContext _context;

		public CustomerRepository(CarRentalDbContext context)
		{
			_context = context;
		}
		public IEnumerable<Customer> GetAll()
		{
			try
			{
				return _context.Customers.ToList();
			}
			catch (Exception ex)
			{
				throw new Exception($"Ошибка при загрузке клиентов: {ex.Message}", ex);
			}
		}
		public Customer GetById(int id)
		{
			return _context.Customers
				.Include(c => c.Orders)
				.FirstOrDefault(c => c.CustomerID == id);
		}
		public void Insert(Customer entity)
		{
			_context.Customers.Add(entity);
			_context.SaveChanges();
		}
		public void Update(Customer entity)
		{
			var existingCustomer = _context.Customers.Find(entity.CustomerID);
			if (existingCustomer != null)
			{
				_context.Entry(existingCustomer).CurrentValues.SetValues(entity);
				_context.SaveChanges();
			}
			else
			{
				throw new ArgumentException($"Клиент с ID {entity.CustomerID} не найден");
			}
		}
		public void Delete(int customerId)
		{
			var customer = _context.Customers.Find(customerId);
			if (customer == null) return;

			bool hasOrders = _context.Orders.Any(o => o.CarID == customerId);
			if (hasOrders)
				throw new InvalidOperationException("Нельзя удалить.");

			try
			{
				_context.Customers.Remove(customer);
				_context.SaveChanges();
			}
			catch (DbUpdateException ex) when (ex.InnerException?.Message?.Contains("REFERENCE") == true)
			{
				throw new InvalidOperationException("Нельзя удалить.", ex);
			}
		}
		public IEnumerable<dynamic> GetTopSpendingCustomers()
		{
			return _context.Customers
				.Select(c => new
				{
					CustomerID = c.CustomerID,
					FullName = c.FullName,
					TotalOrders = c.Orders.Count,
					TotalSpent = c.Orders.Sum(o => o.Hours * o.Car.PricePerHour)
				})
				.OrderByDescending(x => x.TotalSpent)
				.Take(10)
				.ToList();
		}
		public IEnumerable<dynamic> GetCustomersWithOrdersThisMonth()
		{
			var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
			var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

			return _context.Customers
				.Where(c => c.Orders.Any(o => o.OrderDate >= startOfMonth && o.OrderDate <= endOfMonth))
				.Select(c => new
				{
					CustomerID = c.CustomerID,
					FullName = c.FullName,
					Phone = c.Phone,
					OrderCount = c.Orders.Count(o => o.OrderDate >= startOfMonth && o.OrderDate <= endOfMonth)
				})
				.ToList();
		}
	}
}