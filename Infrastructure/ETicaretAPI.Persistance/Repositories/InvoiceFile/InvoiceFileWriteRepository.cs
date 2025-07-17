using ETicaretAPI.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistance.Repositories
{
	internal class InvoiceFileWriteRepository : WriteRepository<ETicaretAPI.Domain.Entities.InvoiceFile>, ETicaretAPI.Application.Repositories.IInvoiceFileWriteRepository
	{
		public InvoiceFileWriteRepository(ETicaretAPIDbContext context) : base(context)
		{
		}
	}
}
