using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Attributes
{
    internal class CacheAttribute(int DurationInSec = 90) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Create Cache Key
            // baseUrl/api/Controller/
            // {{baseUrl}}/api/Products/10
            // {{baseUrl}}/api/Products?brandId=1&typeId=2
            string cacheKey = CreateCacheKey(context.HttpContext.Request);

            // Search for Value with cache key
            ICacheService cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var cacheValue = await cacheService.GetAsync(cacheKey);
            // Return value is not null (Cached)
            if(cacheValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = cacheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            // Invoke .Next
            var executedContext = await next.Invoke();
            // Set Value with Cache Key
            if(executedContext.Result is OkObjectResult result)
            {
                await cacheService.SetAsync(cacheKey, result, TimeSpan.FromSeconds(DurationInSec));
            }
            // Return value
        }
        private string CreateCacheKey(HttpRequest request)
        {
            // {{baseUrl}}/api/Products?brandId=1&typeId=2
            // {{baseUrl}}/api/Products?typeId=2&brandId=1
            StringBuilder key = new StringBuilder();
            key.Append(request.Path);
            key.Append("?");
            foreach(var item in request.Query.OrderBy(Q => Q.Key))
            {
                key.Append($"{item.Key}={item.Value}&");
            }
            return key.ToString();
        }
    }
}
