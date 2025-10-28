using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Web.Models
{
	[Table("Cars")]
	public class Car
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CarID { get; set; }

		[Required]
		[Display(Name = "Номер автомобиля")]
		public string CarNumber { get; set; }

		[Required]
		[Display(Name = "Марка")]
		public string Make { get; set; }

		[Display(Name = "Пробег (км)")]
		public int Mileage { get; set; }

		[Required]
		[Display(Name = "Статус")]
		public string Status { get; set; }

		[Display(Name = "Количество мест")]
		public int Seats { get; set; }

		[Display(Name = "Цена за час")]
		public decimal PricePerHour { get; set; }

		[Display(Name = "Фото")]
		[Column(TypeName = "varbinary(max)")]
		[DataType(DataType.Upload)]
		[MaxLength(10485760)] 
		public byte[]? Photo { get; set; }

		[NotMapped]
		public string? PhotoBase64 => Photo != null ? Convert.ToBase64String(Photo) : null;

		public virtual ICollection<Order> Orders { get; set; }
	}
}