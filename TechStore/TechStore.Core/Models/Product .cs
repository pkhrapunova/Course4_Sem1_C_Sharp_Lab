using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechStore.Core.Models
{
	public class Product
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Название товара обязательно.")]
		[StringLength(100, ErrorMessage = "Название не может быть длиннее 100 символов.")]
		public string Name { get; set; }

		[StringLength(500, ErrorMessage = "Описание не может быть длиннее 500 символов.")]
		public string Description { get; set; }

		[Column(TypeName = "decimal(18, 2)")]
		[Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше нуля.")]
		public decimal Price { get; set; }

		public string ImageUrl { get; set; } // Для хранения пути к изображению товара

		public int CategoryId { get; set; }
		public Category Category { get; set; } // Навигационное свойство
	}
}
