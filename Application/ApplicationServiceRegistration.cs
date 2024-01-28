using Application.Contracts.IServices;
using Application.Services;
using Application.Services.Inventory.Setup;
using Domain.Entities.HR.Setup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IIdentitificationTypeService, IdentitificationTypeService>();
            services.AddScoped<IItemService, ItemService>();
            return services;
        }
    }
}
