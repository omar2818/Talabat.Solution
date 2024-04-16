namespace Talabat.APIs.Extensions
{
	public static class SwaggerServicesExtension
	{
		public static IServiceCollection AddSwaggerServices(this IServiceCollection Services)
		{
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			Services.AddEndpointsApiExplorer();
			Services.AddSwaggerGen();

			return Services;
		}

		public static WebApplication UseSwaggerMiddleware(this WebApplication app)
		{
			app.UseSwagger();
			app.UseSwaggerUI();

			//app.UseDeveloperExceptionPage();

			return app;
		}
	}
}
