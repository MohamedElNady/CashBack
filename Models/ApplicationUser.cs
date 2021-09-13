using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cash_Back.Models
{
	public class ApplicationUser: IdentityUser
	{
		public string Birthdate { get; set; }
		public string Address { get; set; }
		public string Gender { get; set; }
		public string Name {  get; set; }
		public string Role { get; set; }
	}
}
