﻿using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Application.Features.Commands.AppUser.LoginUser
{
	public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
	{
		readonly IAuthService _authService;

		public LoginUserCommandHandler(IAuthService authService)
		{
			_authService = authService;
		}

		public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
		{
			var token = await _authService.LoginAsync(request.UsernameOrEmail, request.Password, 5);
			return new LoginUserSuccessCommandResponse()
			{
				Token = token
			};
		}
	}
}
