using System;

namespace CandyFactory
{
	public abstract class CandyBase : IInspectable
	{
		protected string _name;
		protected double _weight;
		protected decimal _price;
		public static decimal TaxRate { get; set; } = 0.08m;

		public string Name
		{
			get => _name;
			protected set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("Имя не может быть пустым или состоять из пробелов");
				_name = value;
			}
		}

		public double Weight
		{
			get => _weight;
			protected set
			{
				if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
				_weight = value;
			}
		}

		public decimal Price
		{
			get => Math.Round(_price * (1 + TaxRate), 2);
			protected set
			{
				if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
				_price = value;
			}
		}

		public CandyBase(string name, double weight, decimal basePrice)
		{
			Name = name;
			Weight = weight;
			Price = basePrice;
		}

		public abstract void Prepare();
		public virtual string GetLabel() => $"{Name} - {Weight}g - {Price} руб.";

		public override string ToString() => GetLabel();

		public virtual bool Inspect() => true;

		public static MixedCandy operator +(CandyBase a, CandyBase b)
		{
			if (a == null || b == null) throw new ArgumentNullException();
			var name = a.Name + "+" + b.Name;
			var weight = a.Weight + b.Weight;
			var basePrice = a._price + b._price;
			return new MixedCandy(name, weight, basePrice, a, b);
		}
	}
}