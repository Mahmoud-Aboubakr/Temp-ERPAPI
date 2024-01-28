using System.Diagnostics;
using Application;
using Application.Contracts.Handlers;
using Application.Contracts.Models;
using Application.Contracts.Persistence;
using Application.Handlers;
using Application.Helpers;
using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Persistence.Data;
using Persistence.Repositories;
using Persistence.UnitOfWork;
using Swashbuckle.AspNetCore.Filters;

namespace Api.Extensions
{
    public static class GeneralsExtensions
    {
        public static IServiceCollection GeneralServices(this IServiceCollection services, IConfiguration config)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            //services.AddSwaggerGen();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });
            services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
            {
                //if (Environment.OSVersion.Platform == PlatformID.Unix)
                //	optionsBuilder.UseSqlServer(config.GetConnectionString("MoDatabase"));
                //else
                optionsBuilder.UseSqlServer(config.GetConnectionString("ApplicationDatabase"));
               });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddSingleton<IResponseModel, ResponseModel>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IResponseModelHandler, ResponseModelHandler>();
            services.AddSingleton<IMessageHandler, MessageHandler>();
            services.AddSingleton(typeof(IPaginatedModelHandler), typeof(PagedList));

            return services;
        }
    }
}
