﻿using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Infrastructure.Enums;
using ETicaretAPI.Infrastructure.Services.Storage;
using ETicaretAPI.Infrastructure.Services.Storage.Azure;
using ETicaretAPI.Infrastructure.Services.Storage.Local;
using ETicaretAPI.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;

namespace ETicaretAPI.Infrastructure
{
	public static class ServiceRegistration
	{
		public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddScoped<IStorageService, StorageService>();
			serviceCollection.AddScoped<ITokenHandler, TokenHandler>();
		}
		public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : Storage, IStorage
		{
			serviceCollection.AddScoped<IStorage, T>();
		}
		public static void AddStorage(this IServiceCollection serviceCollection, StorageType storageType)
		{
			switch (storageType)
			{
				case StorageType.Local:
					serviceCollection.AddScoped<IStorage, LocalStorage>();
					break;
				case StorageType.Azure:
					serviceCollection.AddScoped<IStorage, AzureStorage>();
					break;
				default:
					serviceCollection.AddScoped<IStorage, LocalStorage>();
					break;
			}
		}
	}
}
