using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Application.Features.Commands.AppUser.LoginUser
{
	public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
	{
		readonly UserManager<Domain.Entities.Idenity.AppUser> _userManager;
		readonly SignInManager<Domain.Entities.Idenity.AppUser> _signInManager;
		readonly ITokenHandler _tokenHandler;

		public LoginUserCommandHandler(
			UserManager<Domain.Entities.Idenity.AppUser> userManager,
			SignInManager<Domain.Entities.Idenity.AppUser> signInManager,
			ITokenHandler tokenHandler)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenHandler = tokenHandler;
		}

		public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
		{
			Domain.Entities.Idenity.AppUser user = await _userManager.FindByNameAsync(request.UsernameOrEmail);
			if (user == null)
			{
				user = await _userManager.FindByEmailAsync(request.UsernameOrEmail);
			}
			if (user == null)
			{
				throw new NotFoundUserException();
			}

			SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
			if (result.Succeeded) // authentication başarılı
			{
				Token token = _tokenHandler.CreateAccessToken(5);
				return new LoginUserSuccessCommandResponse()
				{
					Token = token
				};
			}
			//return new LoginUserErrorCommandResponse()
			//{
			//	Message = "Kullanıcı adı veya şifre hatalı..."
			//};
			throw new AuthenticationErrorException();
		}
	}
}
