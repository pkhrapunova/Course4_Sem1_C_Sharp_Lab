using System.ComponentModel.DataAnnotations;

namespace TechStore.Core.Models
{
	public class User
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Имя пользователя обязательно.")]
		[StringLength(50, ErrorMessage = "Имя пользователя не может быть длиннее 50 символов.")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Email обязателен.")]
		[EmailAddress(ErrorMessage = "Некорректный формат Email.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Пароль обязателен.")]
		[StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 до 100 символов.")]
		public string PasswordHash { get; set; } // Хэш пароля
		public string Role { get; set; } = "User"; // Например, "User" или "Admin"
	}
}