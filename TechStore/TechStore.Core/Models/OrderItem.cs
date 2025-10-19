using System.ComponentModel.DataAnnotations.Schema;

namespace TechStore.Core.Models
{
	public class OrderItem
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public Order Order { get; set; } // Навигационное свойство

		public int ProductId { get; set; }
		public Product Product { get; set; } // Навигационное свойство

		public int Quantity { get; set; }

		[Column(TypeName = "decimal(18, 2)")]
		public decimal Price { get; set; } // Цена товара на момент заказа (может отличаться от текущей)
	}
}