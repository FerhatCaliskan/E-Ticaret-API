﻿using ETicaretAPI.Application.Features.Commands.Product.CreateProduct;
using ETicaretAPI.Application.Features.Commands.Product.RemoveProduct;
using ETicaretAPI.Application.Features.Commands.Product.UpdateProduct;
using ETicaretAPI.Application.Features.Commands.ProductImageFile.ChangeShowcaseImage;
using ETicaretAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage;
using ETicaretAPI.Application.Features.Commands.ProductImageFile.UploadProductImage;
using ETicaretAPI.Application.Features.Queries.Product.GetAllProduct;
using ETicaretAPI.Application.Features.Queries.Product.GetByIdProduct;
using ETicaretAPI.Application.Features.Queries.ProductImageFile.GetProductImages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ETicaretAPI.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		readonly IMediator _mediator;
		readonly ILogger<ProductsController> _logger;

		public ProductsController(

			IMediator mediator, ILogger<ProductsController> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}
		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
		{
			GetAllProductQueryResponse response = await _mediator.Send(getAllProductQueryRequest);
			return Ok(response);
		}
		[HttpGet("{Id}")]
		public async Task<IActionResult> Get([FromRoute] GetByIdProductQueryRequest getByIdProductQueryRequest)
		{
			GetByIdProductQueryResponse response = await _mediator.Send(getByIdProductQueryRequest);
			return Ok(response);
		}
		[HttpPost]
		[Authorize(AuthenticationSchemes = "Admin")]
		public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
		{
			CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
			return StatusCode((int)HttpStatusCode.Created);
		}
		[HttpPut]
		[Authorize(AuthenticationSchemes = "Admin")]
		public async Task<IActionResult> Put([FromBody] UpdateProductCommandRequest updateProductCommandRequest)
		{
			UpdateProductCommandResponse response = await _mediator.Send(updateProductCommandRequest);
			return Ok();
		}
		[HttpDelete("{Id}")]
		[Authorize(AuthenticationSchemes = "Admin")]
		public async Task<IActionResult> Delete([FromRoute] RemoveProductCommandRequest removeProductCommandRequest)
		{
			RemoveProductCommandResponse response = await _mediator.Send(removeProductCommandRequest);
			return Ok();
		}
		[HttpPost("[action]")]
		[Authorize(AuthenticationSchemes = "Admin")]
		public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest uploadProductImageCommandRequest)
		{
			uploadProductImageCommandRequest.Files = Request.Form.Files;
			UploadProductImageCommandResponse response = await _mediator.Send(uploadProductImageCommandRequest);
			return Ok();
		}
		[HttpGet("[action]/{id}")]
		[Authorize(AuthenticationSchemes = "Admin")]
		public async Task<IActionResult> GetProductImages([FromRoute] GetProductImagesQueryRequest getProductImagesQueryRequest)
		{
			List<GetProductImagesQueryResponse> response = await _mediator.Send(getProductImagesQueryRequest);
			return Ok(response);
		}
		[HttpDelete("[action]/{Id}")]
		[Authorize(AuthenticationSchemes = "Admin")]
		public async Task<IActionResult> DeleteProductImage([FromRoute] RemoveProductImageCommandRequest removeProductImageCommandRequest, [FromQuery] string imageId)
		{
			removeProductImageCommandRequest.ImageId = imageId;
			RemoveProductImageCommandResponse response = await _mediator.Send(removeProductImageCommandRequest);
			return Ok();
		}

		[HttpGet("[action]")]
		[Authorize(AuthenticationSchemes = "Admin")]
		public async Task<IActionResult> ChangeShowcaseImage([FromQuery] ChangeShowcaseImageCommandRequest changeShowcaseImageCommandRequest)
		{
			ChangeShowcaseImageCommandResponse response = await _mediator.Send(changeShowcaseImageCommandRequest);
			return Ok();
		}
	}
}
