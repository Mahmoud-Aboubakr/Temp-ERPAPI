global using Application.Contracts.Handlers;
global using Application.Contracts.Models;
global using Application.Contracts.Persistence;
global using Application;
global using AutoMapper;
global using Microsoft.AspNetCore.Mvc;
using Api.Extensions;
using Persistence;
using Microsoft.Extensions.FileProviders;
using Serilog;
using System.Text.Json.Serialization;

namespace Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new ProducesAttribute("application/json", "text/plain", "text/json"));
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            });

            builder.Services.AddCors();
            builder.Services.AddControllers();

            builder.Services.GeneralServices(builder.Configuration);
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.InventoryServices();

            builder.Services.AddPersistenceServices(builder.Configuration);

            var app = builder.Build();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseCors(builder =>
                builder
                    .WithOrigins("http://localhost:4200") // Specify the origin of your Angular app
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());

            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads")),
            //    RequestPath = "/uploads"
            //});
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }



            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            await app.Services.SeedData();

            app.Run();
        }
    }
}