using Airslip.Common.Types.Interfaces;
using System.Net;

namespace Airslip.Common.Utilities.Models
{
    public record HttpRequestResult<TResponse>(bool IsSuccessStatusCode, HttpStatusCode StatusCode, 
        string Content, TResponse? Response = null) where TResponse : class, IResponse;

}