using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace E_Commerce.Web.Factories
{
    public static class APiResponseFactory
    {
        public static IActionResult GenerateApiValidationErrorsResponse(ActionContext context)
        {
            var errors = context.ModelState.Where(M => M.Value.Errors.Any())
                                .Select(M => new ValidationError()
                                {
                                    Field = M.Key,
                                    Errors = M.Value.Errors.Select(E => E.ErrorMessage)
                                });
            var response = new ValidationErrorToReturn()
            {
                ValidationErrors = errors
            };
            return new BadRequestObjectResult(response);
        }
    }
}
