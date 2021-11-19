using Airslip.Common.Auth.Functions.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Functions.Interfaces;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Models;
using Airslip.Common.Types.Configuration;
using Airslip.Common.Types.Failures;
using Airslip.Common.Types.Hateoas;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities;
using Airslip.Common.Utilities.Extensions;
using JetBrains.Annotations;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Airslip.Common.Functions.Implementations
{
    public class FunctionApiTools : IFunctionApiTools
    {
        public readonly string BaseUri;
        public readonly ILogger Logger;

        public FunctionApiTools(IOptions<PublicApiSettings> publicApiOptions, ILogger logger)
        {
            BaseUri = publicApiOptions.Value.Base.ToBaseUri();
            Logger = logger;
        }

        public async Task<bool> CanAuthenticate(HttpRequestData req,
            FunctionContext executionContext)
        {
            IApiRequestAuthService requestHandler = executionContext.InstanceServices
                .GetService<IApiRequestAuthService>()!;
            KeyAuthenticationResult authenticationResult = await requestHandler.Handle(req);
            if (authenticationResult.AuthResult == AuthResult.Success) return true;
            
            Logger.Error("Authorisation unsuccessful {Message}", 
                authenticationResult.Message);
            
            return false;
        }
        
        public async Task<HttpResponseData> OkResponse<T>(HttpRequestData req, T response) 
            where T: class, IResponse
        {
            if (response is LinkResourceBase @base)
            {
                @base.AddHateoasLinks<T>(BaseUri);
                @base.AddChildHateoasLinks(@base, BaseUri);
            }

            return await _generateResponse(req, response, 
                response is ISuccess ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
        }
        
        public async Task<HttpResponseData> NotFound<T>(HttpRequestData req, T response)
            where T: class, IResponse
        {
            return await _generateResponse(req, response, HttpStatusCode.NotFound);
        }

        public async Task<HttpResponseData> Unauthorised<T>(HttpRequestData req, T response)
            where T: class, IResponse
        {
            return await _generateResponse(req, response, HttpStatusCode.Unauthorized);
        }

        private async Task<HttpResponseData> _generateResponse<T>(HttpRequestData req, T response, 
            HttpStatusCode statusCode)
            where T: class, IResponse
        {
            var responseData = req.CreateResponse(statusCode);
            await responseData.WriteAsJsonAsync(response);
            return responseData;
        }
        
        public async Task<HttpResponseData> BadRequest<T>(HttpRequestData req, T failure)
            where T: class, IResponse
        {
            switch (failure)
            {
                case ErrorResponse response:
                    Logger.Error("Bad request error: {ErrorMessage}", response.Message);
                    return await _generateResponse(req, new ApiErrorResponse(response), HttpStatusCode.BadRequest);
                case ErrorResponses response:
                    Logger.Error("Bad request errors: {ErrorMessages}",
                        string.Join(",", response.Errors.Select(errorResponse => errorResponse.Message)));
                    return await _generateResponse(req, new ApiErrorResponse(response.Errors), HttpStatusCode.BadRequest);
                case IFail response:
                    Logger.Error("Fail errors: {ErrorMessages}", response.ErrorCode);
                    return await _generateResponse(req, new ApiErrorResponse(new ErrorResponse(response.ErrorCode)), HttpStatusCode.BadRequest);
                default:
                    throw new ArgumentException("Unknown response type.", nameof(failure));
            }
        }

        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        public class ApiErrorResponse : IFail
        {
            public long Timestamp { get; }
            public string CorrelationId { get; } = CommonFunctions.GetId();
            public IEnumerable<ErrorResponse> Errors { get; }

            public ApiErrorResponse(ErrorResponse error)
                : this(new[] { error })
            {
            }

            public ApiErrorResponse(IEnumerable<ErrorResponse> errors)
            {
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                Errors = errors;
            }

            public string ErrorCode { get; } = "ApiError";
        }
        
        public Task<HttpResponseData> RepositoryActionToResult<TModel>(HttpRequestData req, RepositoryActionResultModel<TModel> theResult) 
            where TModel : class, IModel
        {
            return theResult.ResultType switch
            {
                ResultType.NotFound => NotFound(req, theResult),
                ResultType.FailedValidation or ResultType.FailedVerification => BadRequest(req, theResult),
                _ => OkResponse(req, theResult)
            };
        }

        public Task<HttpResponseData> CommonResponseHandler<TExpectedType>(HttpRequestData req, IResponse response) 
            where TExpectedType : class, IResponse
        {
            return response switch
            {
                TExpectedType r => OkResponse(req, r),
                NotFoundResponse r => NotFound(req, r),
                IFail r => BadRequest(req, r),
                _ => throw new InvalidOperationException()
            };
        }
    }
}