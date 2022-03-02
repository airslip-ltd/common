using Airslip.Common.Repository.Types.Entities;
using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Enums;
using System.Collections.Generic;

namespace Airslip.Common.Repository.UnitTests.Common;

public class MyEntityWithAdditionalOwners : MyEntity, IAdditionalOwners
{
    public ICollection<AdditionalOwner> AdditionalOwners { get; set; } = new List<AdditionalOwner>();
}