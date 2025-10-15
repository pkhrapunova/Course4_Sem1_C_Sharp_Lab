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
		public IEnumerable<Order> GetAll()
		{
			try
			{
				return _context.Orders
					.Include(o => o.Customer)
					.Include(o => o.Car)
					.ToList();
			}
			catch (Exception ex)
			{
				throw new Exception($"Ошибка при загрузке заказов: {ex.Message}", ex);
			}
		}
		public Order GetById(int orderId)
		{
			return _context.Orders
				.Include(o => o.Customer)
				.Include(o => o.Car)
				.FirstOrDefault(o => o.OrderID == orderId);
		}
		public void Insert(Order order)
		{
			_context.Orders.Add(order);
			_context.SaveChanges();
		}
		public void Update(Order order)
		{
			var existingOrder = _context.Orders.Find(order.OrderID);
			if (existingOrder != null)
			{
				_context.Entry(existingOrder).CurrentValues.SetValues(order);
				_context.SaveChanges();
			}
			else
			{
				throw new ArgumentException($"Заказ с ID {order.OrderID} не найден");
			}
		}

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
			catch (Exception ex)
			{
				throw new Exception($"Ошибка при удалении заказа: {ex.Message}", ex);
			}
		}
	}
}