using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Airslip.Common.Auth.Interfaces
{
    public interface ITokenDecodeService<TTokenType> 
        where TTokenType : IDecodeToken, new()
    {
        TTokenType GetCurrentToken();
        TTokenType GetToken(string tokenValue);
        Tuple<TTokenType, ICollection<Claim>> DecodeToken(string tokenValue);
        Tuple<TTokenType, ICollection<Claim>> DecodeTokenFromHeader(string headerValue);
    }
}