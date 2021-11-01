using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Types.Transaction;
using System.Collections.Generic;

namespace Airslip.Common.Matching.Interfaces
{
    public interface IMatchable : IEntityWithId
    {
        long TimeStamp { get; }
        List<MatchMetadata> Metadata { get; }
    }
}