using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin
{
	public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
	{
		readonly UserManager<Domain.Entities.Idenity.AppUser> _userManager;
		readonly ITokenHandler _tokenHandler;

		public GoogleLoginCommandHandler(UserManager<Domain.Entities.Idenity.AppUser> userManager, ITokenHandler tokenHandler)
		{
			_userManager = userManager;
			_tokenHandler = tokenHandler;
		}

		public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
		{
			var settings = new GoogleJsonWebSignature.ValidationSettings()
			{
				Audience = new List<string> { "1014084207702-1ujuor263499euibv95qud7j3qj48vqj.apps.googleusercontent.com" }
			};

			var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

			var info = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);
			Domain.Entities.Idenity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

			bool result = user != null;
			if (user == null)
			{
				user = await _userManager.FindByEmailAsync(payload.Email);
				if (user == null)
				{
					user = new()
					{
						Id = Guid.NewGuid().ToString(),
						Email = payload.Email,
						UserName = payload.Email,
						NameSurname = payload.Name,
					};
					var identityResult = await _userManager.CreateAsync(user);
					result = identityResult.Succeeded;
				}
			}

			if (result)
			{
				await _userManager.AddLoginAsync(user, info);//AspNetUserLogins tablosuna ekler
			}
			else
			{
				throw new Exception("Invalid external authentication");
			}

			Token token = _tokenHandler.CreateAccessToken(5);

			return new()
			{
				Token = token
			};

		}
	}
}
