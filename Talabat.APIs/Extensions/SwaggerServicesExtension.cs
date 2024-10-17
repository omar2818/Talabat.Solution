using Microsoft.OpenApi.Models;

namespace Talabat.APIs.Extensions
{
	public static class SwaggerServicesExtension
	{
		public static IServiceCollection AddSwaggerServices(this IServiceCollection Services)
		{
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			Services.AddEndpointsApiExplorer();
			Services.AddSwaggerGen( c =>
			{
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Talabat.APIs", Version = "v1" });
            });

			return Services;
		}

		public static WebApplication UseSwaggerMiddleware(this WebApplication app)
		{
			app.UseSwagger();
			app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Talabat.APIs v1"));

			//app.UseDeveloperExceptionPage();

			return app;
		}
	}
}
