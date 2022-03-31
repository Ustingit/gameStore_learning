﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.StoreDomain.Entities
{
	/// <summary>
	/// не содержит никакой функциональности, поэтому в модульном тестировании не нуждается.
	/// </summary>
	public class ShippingDetails
	{
		[Required(ErrorMessage = "Укажите как вас зовут")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Вставьте первый адрес доставки")]
		[Display(Name = "Первый адрес")]
		public string Line1 { get; set; }

		[Display(Name = "Второй адрес")]
		public string Line2 { get; set; }

		[Display(Name = "Третий адрес")]
		public string Line3 { get; set; }

		[Required(ErrorMessage = "Укажите страну")]
		[Display(Name = "Страна")]
		public string Country { get; set; }

		[Required(ErrorMessage = "Укажите город")]
		[Display(Name = "Город")]
		public string City { get; set; }

		public bool GiftWrap { get; set; }
	}
}
