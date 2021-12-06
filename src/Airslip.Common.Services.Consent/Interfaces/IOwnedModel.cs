namespace Airslip.Common.Services.Consent.Interfaces
{
    public interface IOwnedModel
    {
        string UserId { get; }
        string? EntityId { get; }
    }
}