using Airslip.Common.Types.Enums;
using System.Collections.Generic;

namespace Airslip.Common.Types.Transaction
{
    public record TransactionEnvelope(
        Transaction Transaction,
        AirslipUserType AirslipUserType,
        string TrackingId,
        string ApiKey,
        string EntityId,
        List<MatchMetadata> Metadata
    );
}