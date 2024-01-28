using Application.Contracts.IServices;
using Application.Services;
using Application.Services.Inventory.Setup;

namespace Api.Extensions
{
    public static class InventoryExtensions
    {
        public static IServiceCollection InventoryServices(this IServiceCollection services)
        {
            services.AddScoped<IStoreService, StoreService>();

            return services;
        }
    }
}
