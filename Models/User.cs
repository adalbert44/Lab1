using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace lab1.Models
{
	public class User :IdentityUser
	{
		[Required(ErrorMessage = "Поле не повинно бути порожнім")]
		[Range(1900, 2020, ErrorMessage = "Некоректний рік народження")]
		public int Year { get; set; }
	}
}
