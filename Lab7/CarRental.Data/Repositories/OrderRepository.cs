using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.Data.Models;
using System.Data.Entity;

namespace CarRental.Data
{
	public class OrderRepository : IRepository<Order>
	{
		private readonly CarRentalDbContext _context;

		public OrderRepository()
		{
			_context = new CarRentalDbContext();
		}

		// Получить все заказы
		public IEnumerable<Order> GetAll()
		{
			try
			{
				return _context.Orders.ToList();
			}
			catch (Exception ex)
			{
				throw new Exception($"Ошибка при загрузке заказов: {ex.Message}", ex);
			}
		}

		// Получить заказ по ID
		public Order GetById(int orderId)
		{
			return _context.Orders.Find(orderId);
		}

		// Добавление заказа
		public void Insert(Order order)
		{
			_context.Orders.Add(order);
			_context.SaveChanges();
		}

		// Обновление заказа - ИСПРАВЛЕНО для EF6
		public void Update(Order order)
		{
			var existingOrder = _context.Orders.Find(order.OrderID);
			if (existingOrder != null)
			{
				_context.Entry(existingOrder).CurrentValues.SetValues(order);
				_context.SaveChanges();
			}
		}

		// Удаление заказа - ИСПРАВЛЕНО для EF6
		public void Delete(int orderId)
		{
			try
			{
				var order = _context.Orders.Find(orderId);
				if (order != null)
				{
					_context.Orders.Remove(order);
					_context.SaveChanges();
				}
			}
			catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
			{
				if (ex.InnerException?.Message?.Contains("REFERENCE") == true)
				{
					throw new Exception("Нельзя удалить этот заказ, так как он связан с другими данными.");
				}
				throw new Exception($"Ошибка при удалении заказа: {ex.Message}", ex);
			}
			catch (Exception ex)
			{
				throw new Exception($"Ошибка при удалении заказа: {ex.Message}", ex);
			}
		}
		// Получить заказы за определенный период
		public IEnumerable<Order> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
		{
			return _context.Orders
				.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
				.ToList();
		}

		// Получить заказы конкретного клиента
		public IEnumerable<Order> GetOrdersByCustomer(int customerId)
		{
			return _context.Orders
				.Where(o => o.CustomerID == customerId)
				.ToList();
		}
	}
}