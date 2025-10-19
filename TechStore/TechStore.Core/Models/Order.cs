using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TechStore.Core.Models
{
	public enum OrderStatus
	{
		Pending,        // Сформирован (ожидает обработки)
		Processing,     // Принят (в обработке)
		ReadyForPickup, // Готов к выдаче
		Completed,      // Выполнен
		Cancelled       // Отменен
	}

	public class Order
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public User User { get; set; } // Навигационное свойство

		public DateTime OrderDate { get; set; }
		public OrderStatus Status { get; set; }

		[Column(TypeName = "decimal(18, 2)")]
		public decimal TotalAmount { get; set; }

		// Адрес доставки
		[Required(ErrorMessage = "Адрес доставки обязателен.")]
		public string ShippingAddress { get; set; }
		public string PhoneNumber { get; set; }

		public ICollection<OrderItem> OrderItems { get; set; } // Список товаров в заказе
	}
}
