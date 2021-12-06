using Airslip.Common.Services.Consent.Models;
using Airslip.Common.Types.Interfaces;
using JetBrains.Annotations;

namespace Airslip.SmartReceipts.Api.Core.Models
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public record BankResponse : BankModel, ISuccess
    {
        
    }
}