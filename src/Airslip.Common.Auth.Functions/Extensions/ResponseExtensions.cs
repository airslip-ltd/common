using Airslip.Common.Types.Failures;
using Airslip.Common.Types.Interfaces;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Functions.Extensions
{
    public static class ResponseExtensions
    {
        public static async Task<HttpResponseData> CommonResponseHandler<TExpectedType>(
            this HttpRequestData requestData,
            IResponse response)
            where TExpectedType : class, IResponse
        {
            HttpResponseData httpResponseData = requestData.CreateResponse();

            switch (response)
            {
                case TExpectedType r:
                    await httpResponseData.WriteAsJsonAsync(r, HttpStatusCode.OK);
                    break;
                case NotFoundResponse r:
                    await httpResponseData.WriteAsJsonAsync(r, HttpStatusCode.NotFound);
                    break;
                case ErrorResponses r:
                    await httpResponseData.WriteAsJsonAsync(r, HttpStatusCode.BadRequest);
                    break;
                case ErrorResponse r:
                    await httpResponseData.WriteAsJsonAsync(r, HttpStatusCode.BadRequest);
                    break;
                case IFail r:
                    await httpResponseData.WriteAsJsonAsync(r, HttpStatusCode.BadRequest);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return httpResponseData;
        }
    }
}