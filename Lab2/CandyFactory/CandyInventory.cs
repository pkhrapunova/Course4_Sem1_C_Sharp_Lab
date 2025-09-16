using System;
using System.Collections;
using System.Collections.Generic;

namespace CandyFactory
{
	public class CandyInventory : IEnumerable<CandyBase>
	{
		private List<CandyBase> _candies = new List<CandyBase>(); // encapsulated


		public CandyBase this[int index]
		{
			get
			{
				if (index < 0 || index >= _candies.Count) throw new IndexOutOfRangeException();
				return _candies[index];
			}
			set
			{
				if (index < 0 || index >= _candies.Count) throw new IndexOutOfRangeException();
				if (value == null) throw new ArgumentNullException(nameof(value));
				_candies[index] = value;
			}
		}


		public int Count => _candies.Count;


		public void Add(CandyBase candy) => _candies.Add(candy);


		public bool Remove(CandyBase candy) => _candies.Remove(candy);


		public IEnumerator<CandyBase> GetEnumerator() => _candies.GetEnumerator();


		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
