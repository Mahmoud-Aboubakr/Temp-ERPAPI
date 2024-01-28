using Application.Contracts.Identity;
using Application.Contracts.Persistence.Identity;
using Application.Models.Authentication;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Persistence.Data;
using Persistence.Repositories.Identity;
using Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class PersistenceServiceExtensions
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddTransient<IAuthenticationService, AuthenticationService>();
            
            services.AddScoped<IChangePasswordReasonRepository, ChangePasswordReasonRepository>();
            services.AddScoped<IAppUserChangePasswordReasonRepository, AppUserChangePasswordReasonRepository>();
            services.AddScoped<IAppUserRoleRepository, AppUserRoleRepository>();
            services.AddScoped<IRolePrivilegeRepository, RolePrivilegeRepository>();

            services.AddIdentityCore<AppUser>()
                .AddRoles<AppUserRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddSignInManager<SignInManager<AppUser>>();

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = configuration["JwtSettings:Audience"],
                ValidateIssuer = true,
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"])),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = tokenValidationParameters;
                    });
            services.AddAuthorization();

            return services;
        }
    }
}
