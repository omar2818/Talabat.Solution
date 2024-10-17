using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services;
using Talabat.Core.Services.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.GenericRepository;
using Talabat.Service;
using Talabat.Service.AuthService;

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
            
			Services.AddScoped<IPaymentService, PaymentService>();
            
			Services.AddScoped<IUnitOfWork, UnitOfWork>();

            Services.AddScoped<IOrderService, OrderService>();

            Services.AddScoped<IAuthService, AuthService>();



            return Services;
		}

		public static IServiceCollection AddAuthServices(this IServiceCollection Services, IConfiguration Configuration)
		{
            Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["JWT:ValidIssuer"],
                        ValidateAudience = true,
                        ValidAudience = Configuration["JWT:ValidAudience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:AuthKey"] ?? string.Empty)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

			return Services;
        }
	}
}
