using Airslip.Common.Types.Enums;

namespace Airslip.Common.Repository.Interfaces
{
    /// <summary>
    /// A simple interface defining the common properties you can expect on an Api facing model
    /// </summary>
    public interface IModelWithOwnership : IModel
    {
        string? EntityId { get; set; }
        
        AirslipUserType? AirslipUserType { get; set; }
    }
}