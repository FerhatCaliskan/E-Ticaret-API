﻿using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.UploadProductImage
{
	public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
	{
		readonly IStorageService _storageService;
		readonly IProductReadRepository _productReadRepository;
		readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

		public UploadProductImageCommandHandler(IProductReadRepository productReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IStorageService storageService)
		{
			_storageService = storageService;
			_productReadRepository = productReadRepository;
			_productImageFileWriteRepository = productImageFileWriteRepository;
		}

		public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
		{
			List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("photo-images", request.Files);
			ETicaretAPI.Domain.Entities.Product product = await _productReadRepository.GetByIdAsync(request.Id);

			await _productImageFileWriteRepository.AddRangeAsync(result.Select(r => new ETicaretAPI.Domain.Entities.ProductImageFile
			{
				FileName = r.fileName,
				Path = r.pathOrContainerName,
				Storage = _storageService.StorageName,
				Products = new List<ETicaretAPI.Domain.Entities.Product>() { product }
			}).ToList());

			await _productImageFileWriteRepository.SaveAsync();
			return new();
		}
	}
}
