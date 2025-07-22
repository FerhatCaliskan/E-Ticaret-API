using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Facebook;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Features.Commands.AppUser.LoginUser;
using ETicaretAPI.Domain.Entities.Idenity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace ETicaretAPI.Persistance.Services
{
	public class AuthService : IAuthService
	{
		readonly IConfiguration _configuration;
		readonly HttpClient _httpClient;
		readonly UserManager<AppUser> _userManager;
		readonly ITokenHandler _tokenHandler;
		readonly SignInManager<AppUser> _signInManager;
		readonly IUserService _userService;
		public AuthService(
			IConfiguration configuration,
			IHttpClientFactory httpClientFactory,
			UserManager<AppUser> userManager,
			ITokenHandler tokenHandler,
			SignInManager<AppUser> signInManager,
			IUserService userService)
		{
			_configuration = configuration;
			_httpClient = httpClientFactory.CreateClient();
			_userManager = userManager;
			_tokenHandler = tokenHandler;
			_signInManager = signInManager;
			_userService = userService;
		}

		async Task<Token> CreateUserExternalAsync(AppUser user, string email, string name, UserLoginInfo info, int accessTokenLifeTime)
		{
			bool result = user != null;
			if (user == null)
			{
				user = await _userManager.FindByEmailAsync(email);
				if (user == null)
				{
					user = new()
					{
						Id = Guid.NewGuid().ToString(),
						Email = email,
						UserName = email,
						NameSurname = name
					};
					var identityResult = await _userManager.CreateAsync(user);
					result = identityResult.Succeeded;
				}
			}

			if (result)
			{
				await _userManager.AddLoginAsync(user, info); //AspNetUserLogins tablosuna ekler

				Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime); //token üretim
				await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 5);
				return token;
			}
			throw new Exception("Invalid external authentication");
		}

		public async Task<Token> FacebookLoginAsync(string authToken, int accessTokenLifeTime)
		{
			string accessTokenResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["SocialLogin:Facebook:Client_ID"]}&client_secret={_configuration["SocialLogin:Facebook:Client_Secret"]}&grant_type=client_credentials");

			FacebookAccessTokenResponse? facebookAccessTokenResponse = JsonSerializer.Deserialize<FacebookAccessTokenResponse>(accessTokenResponse);

			string userAccessTokenValidation = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={facebookAccessTokenResponse?.AccessToken}");

			FacebookUserAccessTokenValidation? validation = JsonSerializer.Deserialize<FacebookUserAccessTokenValidation>(userAccessTokenValidation);

			if (validation?.Data.IsValid != null)
			{
				string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={authToken}");

				FacebookUserInfoResponse? userInfo = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);

				var info = new UserLoginInfo("FACEBOOK", validation.Data.UserId, "FACEBOOK");
				AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

				return await CreateUserExternalAsync(user, userInfo.Email, userInfo.Name, info, accessTokenLifeTime);
			}
			throw new Exception("Invalid external authentication.");
		}

		public async Task<Token> GoogleLoginAsync(string idToken, int accessTokenLifeTime)
		{
			var settings = new GoogleJsonWebSignature.ValidationSettings()
			{
				Audience = new List<string> { _configuration["SocialLogin:Google:Client_ID"] }
			};

			var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

			var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");
			AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

			return await CreateUserExternalAsync(user, payload.Email, payload.Name, info, accessTokenLifeTime);
		}

		public async Task<Token> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTime)
		{
			AppUser user = await _userManager.FindByNameAsync(usernameOrEmail);
			if (user == null)
				user = await _userManager.FindByEmailAsync(usernameOrEmail);

			if (user == null)
				throw new NotFoundUserException();

			SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
			if (result.Succeeded) //Authentication başarılı!
			{
				Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime);
				await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 5);
				return token;
			}
			throw new AuthenticationErrorException();
		}
	}
}
