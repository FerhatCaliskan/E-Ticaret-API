using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistance.Repositories
{
	internal class ProductImageFileWriteRepository : WriteRepository<ProductImageFile>, ETicaretAPI.Application.Repositories.IProductImageFileWriteRepository
	{
		public ProductImageFileWriteRepository(ETicaretAPIDbContext context) : base(context)
		{
		}
	}
}
