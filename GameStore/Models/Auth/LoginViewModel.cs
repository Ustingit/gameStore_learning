using System;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Models.Auth
{
	public class LoginViewModel
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		public string Password { get; set; }
	}
}