﻿using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Common;
using ETicaretAPI.Domain.Entities.Idenity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Persistance.Contexts
{
	public class ETicaretAPIDbContext : IdentityDbContext<AppUser, AppRole, string>
	{
		public ETicaretAPIDbContext(DbContextOptions options) : base(options)
		{ }
		public DbSet<Product> Products { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Domain.Entities.File> Files { get; set; }
		public DbSet<ProductImageFile> ProductImageFiles { get; set; }
		public DbSet<InvoiceFile> InvoiceFiles { get; set; }
		public DbSet<Basket> Baskets { get; set; }
		public DbSet<BasketItem> BasketsItems { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Order>().HasKey(b => b.Id);

			builder.Entity<Basket>()
				.HasOne(b => b.Order)
				.WithOne(o => o.Basket)
				.HasForeignKey<Order>(b => b.Id);

			base.OnModelCreating(builder);
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			//ChangeTracker : Entityler üzerinde yapılan değişikliklerin ya da yeni eklenen verinin yakalanmasını takip eder. Update operasyonlarında track eilen verileri yakalayıp elde etmemizi sağlar.

			var datas = ChangeTracker.Entries<BaseEntity>();

			foreach (var data in datas)
			{
				_ = data.State switch
				{
					EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
					EntityState.Modified => data.Entity.UpdatedDate = DateTime.UtcNow,
					_ => DateTime.UtcNow
				};
			}

			return await base.SaveChangesAsync(cancellationToken);
		}
	}
}
