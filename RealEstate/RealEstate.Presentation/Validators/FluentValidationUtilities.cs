using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RealEstate.Presentation.Validators
{
    internal static class FluentValidationUtilities
    {
        public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState)
        {
            foreach (var error in result.Errors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
    }
}
