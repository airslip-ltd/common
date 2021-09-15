using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Airslip.Common.Auth.Interfaces
{
    public interface ITokenDecodeService<TTokenType> 
        where TTokenType : IDecodeToken, new()
    {
        TTokenType GetCurrentToken();

        Tuple<TTokenType, ICollection<Claim>> DecodeExistingToken(string tokenValue);
    }
}