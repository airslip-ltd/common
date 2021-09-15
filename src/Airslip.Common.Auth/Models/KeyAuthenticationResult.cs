namespace Airslip.Common.Auth.Models
{
    public record KeyAuthenticationResult
    {
        public bool Success { get; init; }
        public string? Message { get; init; }

        public static KeyAuthenticationResult Fail(string message)
        {
            return new KeyAuthenticationResult
            {
                Success = false,
                Message = message
            };
        }
        
        public static KeyAuthenticationResult Valid()
        {
            return new KeyAuthenticationResult
            {
                Success = true
            };
        }
    }
}