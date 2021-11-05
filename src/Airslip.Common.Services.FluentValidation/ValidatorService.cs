using Airslip.Common.Types;
using Airslip.Common.Types.Failures;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities.Extensions;
using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using System.Threading.Tasks;

namespace Airslip.Common.Services.FluentValidation
{
    public static class ValidatorService
    {
        // public static async Task ValidateFunction<T>(
        //     this AbstractValidator<T> validator,
        //     T request,
        //     HttpResponseData response) where T : class
        // {
        //     ValidationResult? validationResult = await validator.ValidateAsync(request);
        //
        //     if (validationResult.IsValid)
        //         return;
        //
        //     ErrorResponse[] errors = validationResult.Errors
        //         .Select(error =>
        //             new ErrorResponse(
        //                 error.ErrorCode,
        //                 error.ErrorMessage,
        //                 error.CustomState.ToDictionary<object>()))
        //         .ToArray();
        //
        //     await response.WriteAsJsonAsync(errors, HttpStatusCode.BadRequest);
        // }
        
        public static async Task<IResponse> Validate<T>(
            this AbstractValidator<T> validator,
            T request) where T : class
        {
            ValidationResult? validationResult = await validator.ValidateAsync(request);

            if (validationResult.IsValid)
                return Success.Instance;

            ErrorResponse[] errors = validationResult.Errors
                .Select(error =>
                    new ErrorResponse(
                        error.ErrorCode,
                        error.ErrorMessage,
                        error.CustomState.ToDictionary<object>()))
                .ToArray();

            return new ErrorResponses(errors);
        }
    }
}