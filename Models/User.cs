using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace lab1.Models
{
	public class User :IdentityUser
	{
		public int Year { get; set; }
	}
}
