using Airslip.Common.Services.Consent.Enums;

namespace Airslip.Common.Services.Consent.Interfaces
{
    public interface IFromDataSource
    {
        DataSources DataSource { get; set; }
        long TimeStamp { get; set; }
    }
}