using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModule;
using E_Commerce.Web.CustomMiddleWares;
using E_Commerce.Web.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data.Contexts;
using Persistence.Identity;
using Persistence.Repositories;
using Service;
using Service.MappingProfiles;
using ServiceAbstraction;
using Shared.ErrorModels;
using System.Threading.Tasks;

namespace E_Commerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container.
            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplicationServices();
            builder.Services.AddWebApplicationServices();
            builder.Services.AddJWTServices(builder.Configuration);
            #endregion

            var app = builder.Build();

            #region Data Seeding
            await app.SeedDataBaseAsync();
            #endregion

            #region Configure the HTTP request pipeline.
            // Configure the HTTP request pipeline.
            #region Custom MiddleWare
            //app.Use(async (RequestContext, NextMiddleware) =>
            //{
            //    Console.WriteLine("Request Under Processing");
            //    await NextMiddleware.Invoke();
            //    Console.WriteLine("Waiting Response");
            //    Console.WriteLine(RequestContext.Response.Body);
            //}); 
            #endregion

            app.UseCustomExceptionMiddleWares();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleWares();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
