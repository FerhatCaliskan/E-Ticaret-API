using ETicaretAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.Order.CreateOrder
{
	public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
	{
		readonly IOrderService _orderService;

		public CreateOrderCommandHandler(IOrderService orderService)
		{
			_orderService = orderService;
		}

		public Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
		{
			_orderService.CreateOrderAsync(new()
			{
				Address = request.Address,
				Description = request.Description,

			});
		}
	}
}
