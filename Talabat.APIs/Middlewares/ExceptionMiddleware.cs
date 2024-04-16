using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Middlewares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly IWebHostEnvironment _env;

		public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger ,IWebHostEnvironment env)
        {
			_next = next;
			_logger = logger;
			_env = env;
		}

        public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				// Take an action with the request

				await _next.Invoke(httpContext); // Go to the next Middleware

				// Take an action with the response
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message); // Developmment

				// in Production
				// log Error in Database || File

				httpContext.Response.StatusCode = 500;
				httpContext.Response.ContentType = "appication/json";

				var response = _env.IsDevelopment() ? new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString())
					:
					new ApiExceptionResponse(500);

				var options = new JsonSerializerOptions()
				{
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase
				};

				var json = JsonSerializer.Serialize(response, options);

				await httpContext.Response.WriteAsync(json);

			}

		}
	}
}
