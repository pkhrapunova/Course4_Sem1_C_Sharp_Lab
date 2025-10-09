using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.Data.Models;

namespace CarRental.Data
{
	public class ServiceRepository : IRepository<Service>
	{
		private readonly CarRentalDbContext _context;

		public ServiceRepository()
		{
			_context = new CarRentalDbContext();
		}

		// Получить все услуги
		public IEnumerable<Service> GetAll()
		{
			try
			{
				return _context.Services.ToList();
			}
			catch (Exception ex)
			{
				throw new Exception($"Ошибка при загрузке услуг: {ex.Message}", ex);
			}
		}

		// Получить услугу по ID
		public Service GetById(int id)
		{
			return _context.Services.Find(id);
		}

		// Добавление услуги
		public void Insert(Service entity)
		{
			_context.Services.Add(entity);
			_context.SaveChanges();
		}

		// Обновление услуги
		public void Update(Service entity)
		{
			var existing = _context.Services.Find(entity.ServiceID);
			if (existing != null)
			{
				_context.Entry(existing).CurrentValues.SetValues(entity);
				_context.SaveChanges();
			}
		}

		// Удаление услуги
		public void Delete(int id)
		{
			try
			{
				var service = _context.Services.Find(id);
				if (service != null)
				{
					_context.Services.Remove(service);
					_context.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Ошибка при удалении услуги: {ex.Message}", ex);
			}
		}

		// 🔎 Поиск услуги по названию (регистронезависимый)
		public IEnumerable<Service> SearchByName(string name)
		{
			return _context.Services
				.Where(s => s.Name.ToLower().Contains(name.ToLower()))
				.ToList();
		}

		// 📊 Сортировка по цене
		public IEnumerable<Service> GetSortedByPrice(bool ascending = true)
		{
			if (ascending)
				return _context.Services.OrderBy(s => s.Price).ToList();
			else
				return _context.Services.OrderByDescending(s => s.Price).ToList();
		}
	}
}
