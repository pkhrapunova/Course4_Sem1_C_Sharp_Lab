using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.Data.Models;

namespace CarRental.Data
{
	public class CustomerRepository : IRepository<Customer>
	{
		private readonly CarRentalDbContext _context;

		public CustomerRepository()
		{
			_context = new CarRentalDbContext();
		}

		// Получить всех клиентов
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

		// Получить клиента по ID
		public Customer GetById(int id)
		{
			return _context.Customers.Find(id);
		}

		// Добавление клиента
		public void Insert(Customer entity)
		{
			_context.Customers.Add(entity);
			_context.SaveChanges();
		}

		// Обновление клиента - ИСПРАВЛЕНО для EF6
		public void Update(Customer entity)
		{
			var existingCustomer = _context.Customers.Find(entity.CustomerID);
			if (existingCustomer != null)
			{
				_context.Entry(existingCustomer).CurrentValues.SetValues(entity);
				_context.SaveChanges();
			}
		}

		// Удаление клиента - ИСПРАВЛЕНО для EF6
		public void Delete(int id)
		{
			try
			{
				var customer = _context.Customers.Find(id);
				if (customer != null)
				{
					// Явная проверка на наличие заказов
					bool hasOrders = _context.Orders.Any(o => o.CustomerID == id);
					if (hasOrders)
					{
						throw new Exception("Нельзя удалить клиента, так как у него есть заказы.");
					}

					_context.Customers.Remove(customer);
					_context.SaveChanges();
				}
			}
			catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
			{
				if (ex.InnerException?.Message?.Contains("REFERENCE") == true ||
					ex.InnerException?.InnerException?.Message?.Contains("REFERENCE") == true)
				{
					throw new Exception("Нельзя удалить этого клиента, так как он связан с заказами.");
				}
				throw new Exception($"Ошибка при удалении клиента: {ex.Message}", ex);
			}
			catch (Exception ex)
			{
				throw new Exception($"Ошибка при удалении клиента: {ex.Message}", ex);
			}
		}
	}
}