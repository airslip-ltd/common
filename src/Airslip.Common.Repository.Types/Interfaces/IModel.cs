using Airslip.Common.Repository.Types.Enums;

namespace Airslip.Common.Repository.Types.Interfaces
{
    /// <summary>
    /// A simple interface defining the common properties you can expect on an Api facing model
    /// </summary>
    public interface IModel
    {
        string? Id { get; set; }
        EntityStatus EntityStatus { get; set; }
    }
}