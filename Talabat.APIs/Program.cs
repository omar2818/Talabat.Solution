using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository._Identity;
using Talabat.Repository.GenericRepository.Data;

namespace Talabat.APIs
{
    public class Program
	{
		public static async Task Main(string[] args)
		{
			var WebApplicationBuilder = WebApplication.CreateBuilder(args);

			#region Configure Services
			// Add services to the container.

			WebApplicationBuilder.Services.AddControllers();

			WebApplicationBuilder.Services.AddSwaggerServices();

			WebApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
			{
				options.UseSqlServer(WebApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
			});

            WebApplicationBuilder.Services.AddDbContext<AppllicationIdentityDbContext>(options =>
            {
                options.UseSqlServer(WebApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));
            });

            WebApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>((servicesProvider) =>
			{
				var connection = WebApplicationBuilder.Configuration.GetConnectionString("RedisConnection");
				return ConnectionMultiplexer.Connect(connection);
			});

			WebApplicationBuilder.Services.AddApplicationService();

			#endregion

			var app = WebApplicationBuilder.Build();

			#region Apply All Pending Migrations[Update-Database] and Data Seeding
			
			using var scope = app.Services.CreateScope();

			var services = scope.ServiceProvider;

			var _dbContext = services.GetRequiredService<StoreContext>();
			var _IdentitydbContext = services.GetRequiredService<AppllicationIdentityDbContext>();

			var loggerFactory = services.GetRequiredService<ILoggerFactory>();
			var logger = loggerFactory.CreateLogger<Program>();

			try
			{
				await _dbContext.Database.MigrateAsync();

				await StoreContextSeed.SeedAsync(_dbContext);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "An error has been occured during the migration");

			}

            try
            {
                await _IdentitydbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error has been occured during the migration");

            }

            #endregion

            #region Configure Kestrel Middleareas

            app.UseMiddleware<ExceptionMiddleware>();


			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwaggerMiddleware();
			}

			app.UseStatusCodePagesWithReExecute("/errors/{0}");

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			//app.UseAuthorization();


			app.MapControllers(); 
			#endregion

			app.Run();
		}
	}
}
