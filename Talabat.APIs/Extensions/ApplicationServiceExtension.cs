using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository.GenericRepository;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServiceExtension
	{
		public static IServiceCollection AddApplicationService(this IServiceCollection Services)
		{
			//Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
			//Services.AddScoped<IGenericRepository<ProductBrand>, GenericRepository<ProductBrand>>();
			//Services.AddScoped<IGenericRepository<ProductCategory>, GenericRepository<ProductCategory>>();

			Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

			//Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles));
			Services.AddAutoMapper(typeof(MappingProfiles));

			Services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = (actionContext) =>
				{
					var errors = actionContext.ModelState.Where(P => P.Value.Errors.Any())
														 .SelectMany(P => P.Value.Errors)
														 .Select(E => E.ErrorMessage);

					var response = new ApiVallidationErrorResponse()
					{
						Errors = errors
					};

					return new BadRequestObjectResult(response);
				};
			});

			return Services;
		}
	}
}
