using System;
using System.Collections.Generic;
using System.Linq;

namespace CandyFactory
{
	public interface IFactory
	{
		IEnumerable<CandyBase> GetAll();
		string Name { get; } 
	}
	public class Factory<T> : IFactory where T : CandyBase
	{
		protected List<T> items = new List<T>();
		public string Name { get; protected set; } 
		public Factory(string name)
		{
			Name = name;
		}

		public void Add(T item) => items.Add(item);

		public IEnumerable<T> GetAll() => items;

		IEnumerable<CandyBase> IFactory.GetAll() => items;
		public IEnumerable<T> FindByPredicate(Func<T, bool> predicate)
		{
			return items.Where(predicate);
		}
	}
}