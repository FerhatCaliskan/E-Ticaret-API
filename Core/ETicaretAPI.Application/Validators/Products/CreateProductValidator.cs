using ETicaretAPI.Application.ViewModels.Products;
using FluentValidation;

namespace ETicaretAPI.Application.Validators.Products
{
	public class CreateProductValidator : AbstractValidator<VM_Create_Product>
	{
		public CreateProductValidator()
		{
			RuleFor(p => p.Name)
				.NotEmpty()
				.NotNull()
					.WithMessage("Ürün adı boş geçilemez.")
				.MaximumLength(150)
				.MinimumLength(3)
					.WithMessage("Ürün adı 3 ile 150 karakter arasında olmalıdır.");

			RuleFor(p => p.Stock)
				.NotEmpty()
				.NotNull()
					.WithMessage("Stok bilgisi boş geçilemez.")
				.Must(s => s >= 0)
					.WithMessage("Stok bilgisi 0 veya daha büyük olmalıdır.");
	
			RuleFor(p => p.Price)
				.NotEmpty()
				.NotNull()
					.WithMessage("Fiyat bilgisi boş geçilemez.")
				.Must(p => p >= 0)
					.WithMessage("Fiyat bilgisi 0 veya daha büyük olmalıdır.");

		}
	}
}
