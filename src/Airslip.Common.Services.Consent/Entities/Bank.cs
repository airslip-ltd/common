using Airslip.Common.Repository.Entities;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Services.Consent.Enums;
using Airslip.Common.Services.Consent.Interfaces;
using Airslip.Common.Utilities.Extensions;
using System;
using System.Collections.Generic;

namespace Airslip.Common.Services.Consent.Entities
{
    public record Bank(
        string Id,
        string TradingName,
        string AccountName,
        string EnvironmentType,
        ICollection<string> CountryCodes) : IEntity, IFromDataSource
    {
        public string Id { get; set; } = Id;
        public BasicAuditInformation? AuditInformation { get; set; }
        public EntityStatus EntityStatus { get; set; }
        public DataSources DataSource { get; set; } = DataSources.Unknown;
        public long TimeStamp { get; set; } = DateTime.UtcNow.ToUnixTimeMilliseconds();
    }
}