using ETicaretAPI.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistance.Repositories
{
	internal class InvoiceFileReadRepository : ReadRepository<ETicaretAPI.Domain.Entities.InvoiceFile>, ETicaretAPI.Application.Repositories.IInvoiceFileReadRepository
	{
		public InvoiceFileReadRepository(ETicaretAPIDbContext context) : base(context)
		{
		}
	}
}
