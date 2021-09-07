using Airslip.Common.Auth.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Airslip.Common.Auth.Interfaces
{
    public interface ITokenService<TTokenType, TGenerateTokenType> 
        where TTokenType : TokenBase
        where TGenerateTokenType : GenerateTokenBase
    {
        TTokenType GetCurrentToken();

        Tuple<TTokenType, ICollection<Claim>> DecodeExistingToken(string tokenValue);

        string GenerateNewToken(ICollection<Claim> claims);
        
        string GenerateNewToken(TGenerateTokenType token);
    }
}