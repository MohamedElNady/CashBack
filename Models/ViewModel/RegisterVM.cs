﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cash_Back.Models.ViewModel
{
	public class RegisterVM
	{
		
		[Required]
		[DataType(DataType.Text)]
		[RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
		[Display(Name = "Name")]
		public string Name { get; set; }
		
		[Required]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email")]
		public string UserEmail {  get; set; }
		
		[Required]
		[DataType(DataType.Password)]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)] 
		public string Password {  get; set; }
		
		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPass {  get; set; }
	}
}
