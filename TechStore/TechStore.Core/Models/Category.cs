using System.ComponentModel.DataAnnotations;

namespace TechStore.Core.Models
{
	public class Category
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Название категории обязательно.")]
		[StringLength(50, ErrorMessage = "Название категории не может быть длиннее 50 символов.")]
		public string Name { get; set; }
	}
}