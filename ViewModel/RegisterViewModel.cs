using System.ComponentModel.DataAnnotations;

namespace lab1.ViewModel
{
	public class RegisterViewModel
	{
		[Required]
		[Display(Name = "Email")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required]
		[Display(Name = "Рік народження")]
		[Range(1900, 2020, ErrorMessage = "Рік народження повинен бути в межах [1900, 2020]")]
		public int Year { get; set; }

		[Required]
		[Display(Name = "Пароль")]
		[DataType(DataType.Password)]
		public string Password{ get; set; }

		[Required]
		[Compare("Password", ErrorMessage = "Паролі не співпадають")]
		[DataType(DataType.Password)]
		[Display(Name = "Підтвердження паролю")]
		public string PasswordConfirm { get; set; }
	}
}
