﻿using System.ComponentModel.DataAnnotations;

namespace CorporateRiskManagementSystemBack.ViewModels
{
	public class LoginViewModel
	{
		
		[Required(ErrorMessage = "Не указан Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Не указан пароль")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		
	}
}
