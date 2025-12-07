using DomainLayer.Contracts;
using E_Commerce.Web.CustomMiddleWares;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Web.Extensions
{
    public static class WebApplicationRegistration
    {
        public static async Task SeedDataBaseAsync(this WebApplication app)
        {
            var Scope = app.Services.CreateScope();

            var service = Scope.ServiceProvider.GetRequiredService<IDataSeeding>();

            await service.DataSeedAsync();
            await service.IdentityDataSeedAsync();
        }
        public static IApplicationBuilder UseCustomExceptionMiddleWares(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionHandlerMiddleWare>();

            return app;
        }
        public static IApplicationBuilder UseSwaggerMiddleWares(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.ConfigObject = new ConfigObject()
                {
                    DisplayRequestDuration = true
                };

                options.DocumentTitle = "E-Commerce API G01";

                options.JsonSerializerOptions = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                options.DocExpansion(DocExpansion.None);
                options.EnableFilter();
                options.EnablePersistAuthorization();
            });

            return app;
        }
    }
}
