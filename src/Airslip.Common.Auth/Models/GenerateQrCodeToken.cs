using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;

namespace Airslip.Common.Auth.Models
{
    public class GenerateQrCodeToken : IGenerateToken
    {
        public GenerateQrCodeToken(string entityId, string storeId, string checkoutId, string qrCodeKey, AirslipUserType airslipUserType)
        {
            EntityId = entityId;
            StoreId = storeId;
            CheckoutId = checkoutId;
            QrCodeKey = qrCodeKey;
            AirslipUserType = airslipUserType;
        }

        public string EntityId { get; init; }
        public string StoreId { get; init; }
        public string CheckoutId { get; init; }
        public string QrCodeKey { get; init; }

        public AirslipUserType AirslipUserType { get; init; }

        public List<Claim> GetCustomClaims()
        {
            List<Claim> claims = new()
            {
                new Claim(AirslipClaimTypes.STORE_ID, StoreId),
                new Claim(AirslipClaimTypes.CHECKOUT_ID, CheckoutId),
                new Claim(AirslipClaimTypes.QR_CODE_KEY, QrCodeKey)
            };

            return claims;
        }
    }
}