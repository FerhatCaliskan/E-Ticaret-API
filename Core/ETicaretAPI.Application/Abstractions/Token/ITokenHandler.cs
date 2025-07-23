using ETicaretAPI.Domain.Entities.Idenity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Token
{
	public interface ITokenHandler
	{
		DTOs.Token CreateAccessToken(int minute, AppUser appUser);
		string CreateRefreshToken();
	}
}
