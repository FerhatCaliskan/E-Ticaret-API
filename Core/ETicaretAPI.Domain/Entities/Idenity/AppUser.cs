﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Domain.Entities.Idenity
{
	public class AppUser : IdentityUser<string>
	{
		public string NameSurname { get; set; }
		public string? RefreshToken { get; set; }
		public DateTime? ResfreshTokenEndDate { get; set; }
		public ICollection<Basket> Baskets { get; set; }
	}
}
