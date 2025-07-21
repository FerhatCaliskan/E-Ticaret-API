using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Facebook;
using Google.Apis.Auth.OAuth2;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUser.FacebookLogin
{
	public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
	{
		readonly UserManager<Domain.Entities.Idenity.AppUser> _userManager;
		readonly ITokenHandler _tokenHandler;
		readonly HttpClient _httpClient;
		readonly IConfiguration _configuration;

		public FacebookLoginCommandHandler(
			UserManager<Domain.Entities.Idenity.AppUser> userManager,
			ITokenHandler tokenHandler,
			IHttpClientFactory httpClientFactory,
			IConfiguration configuration)
		{
			_userManager = userManager;
			_tokenHandler = tokenHandler;
			_httpClient = httpClientFactory.CreateClient();
			_configuration = configuration;
		}

		public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request, CancellationToken cancellationToken)
		{
			string clientId = _configuration["SocialLogin:Facebook:ClientId"];
			string clientSecret = _configuration["SocialLogin:Facebook:ClientSecret"];

			string accessTokenResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={clientId}&client_secret={clientSecret}&grant_type=client_credentials");

			FacebookAccessTokenResponse facebookAccessTokenResponse = JsonSerializer.Deserialize<FacebookAccessTokenResponse>(accessTokenResponse);

			string userAccessTokenValidation = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={request.AuthToken}&access_token={facebookAccessTokenResponse.AccessToken}");

			FacebookUserAccessTokenValidation validation = JsonSerializer.Deserialize<FacebookUserAccessTokenValidation>(userAccessTokenValidation);

			if (validation.Data.IsValid)
			{
				string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={request.AuthToken}");

				FacebookUserInfoResponse userInfo = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);

				var info = new UserLoginInfo("FACEBOOK", validation.Data.UserId, "FACEBOOK");
				Domain.Entities.Idenity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

				bool result = user != null;
				if (user == null)
				{
					user = await _userManager.FindByEmailAsync(userInfo.Email);
					if (user == null)
					{
						user = new()
						{
							Id = Guid.NewGuid().ToString(),
							Email = userInfo.Email,
							UserName = userInfo.Email,
							NameSurname = userInfo.Name,
						};
						var identityResult = await _userManager.CreateAsync(user);
						result = identityResult.Succeeded;
					}
				}

				if (result)
				{
					await _userManager.AddLoginAsync(user, info);//AspNetUserLogins tablosuna ekler
					Token token = _tokenHandler.CreateAccessToken(5);
					return new()
					{
						Token = token
					};
				}
			}
			throw new Exception("Invalid external authentication");
		}
	}
}
