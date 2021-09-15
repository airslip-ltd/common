using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Implementations;
using System.Collections.Generic;
using System.Security.Claims;

namespace Airslip.Common.Auth.Models
{
    public class QrCodeToken : TokenBase
    {
        public string StoreId { get; private set; } = "";
        public string CheckoutId { get; private set; } = "";
        public string QrCodeKey { get; private set; } = "";
        
        public override void SetCustomClaims(List<Claim> tokenClaims)
        {
            StoreId = tokenClaims.GetValue(AirslipClaimTypes.STORE_ID);
            CheckoutId = tokenClaims.GetValue(AirslipClaimTypes.CHECKOUT_ID);
            QrCodeKey = tokenClaims.GetValue(AirslipClaimTypes.QR_CODE_KEY);
        }
    }
}