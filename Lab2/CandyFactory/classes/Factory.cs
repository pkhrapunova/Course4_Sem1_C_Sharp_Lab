using System;
using System.Collections.Generic;
using System.Linq;

namespace CandyFactory
{
	// 🔹 общий интерфейс для всех фабрик
	public interface IFactory
	{
		IEnumerable<CandyBase> GetAll();
		string Name { get; } // Добавляем свойство Name в интерфейс
	}

	// 🔹 обобщённая фабрика
	public class Factory<T> : IFactory where T : CandyBase
	{
		protected List<T> items = new List<T>();
		public string Name { get; protected set; } // Добавляем свойство Name

		public Factory(string name)
		{
			Name = name;
		}

		public void Add(T item) => items.Add(item);

		public IEnumerable<T> GetAll() => items;

		// Реализация интерфейса
		IEnumerable<CandyBase> IFactory.GetAll() => items;

		// 🔍 поиск по предикату
		public IEnumerable<T> FindByPredicate(Func<T, bool> predicate)
		{
			return items.Where(predicate);
		}
	}
}