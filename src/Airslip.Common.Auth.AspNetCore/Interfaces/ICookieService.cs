using Airslip.Common.Auth.Models;
using Microsoft.AspNetCore.Http;

namespace Airslip.Common.Auth.AspNetCore.Interfaces
{
    public interface ICookieService
    {
        void UpdateCookie(UserToken userToken);
        string GetCookieValue(HttpRequest request);
    }
}