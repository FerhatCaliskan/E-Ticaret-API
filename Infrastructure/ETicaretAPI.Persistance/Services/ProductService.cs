using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistance.Services
{
	public class ProductService : IProductService
	{
		readonly IProductReadRepository _productReadRepository;
		readonly IQRCodeService _qRCodeService;

		public ProductService(IProductReadRepository productReadRepository, IQRCodeService qRCodeService)
		{
			_productReadRepository = productReadRepository;
			_qRCodeService = qRCodeService;
		}

		public async Task<byte[]> QrCodeToProuctAsync(string productId)
		{
			Product product = await _productReadRepository.GetByIdAsync(productId);
			if (product == null)
				throw new Exception("Ürün Bulunamadı");

			var plainObject = new
			{
				product.Id,
				product.Name,
				product.Stock,
				product.Price,
				product.CreatedDate,
			};
			string plainText = JsonSerializer.Serialize(plainObject);

			return _qRCodeService.GenerateQRCode(plainText);
		}
	}
}
