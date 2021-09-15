namespace Airslip.Common.Auth.Interfaces
{
    public interface IHttpHeaderLocator
    {
        string? GetValue(string headerValue, string? defaultValue = null);
    }
}