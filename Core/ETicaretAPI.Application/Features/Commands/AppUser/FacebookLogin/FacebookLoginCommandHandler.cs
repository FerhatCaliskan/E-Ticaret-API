﻿using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.AppUser.FacebookLogin
{
	public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
	{
		readonly IAuthService _authService;

		public FacebookLoginCommandHandler(IAuthService authService)
		{
			_authService = authService;
		}

		public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request, CancellationToken cancellationToken)
		{
			var token = await _authService.FacebookLoginAsync(request.AuthToken, 5);

			return new()
			{
				Token = token
			};
		}
	}
}
