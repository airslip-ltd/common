using Airslip.Common.Auth.Functions.Interfaces;
using Airslip.Common.Auth.Interfaces;
using System.Linq;

namespace Airslip.Common.Auth.Functions.Implementations
{
    public class FunctionContextHeaderLocator : IHttpHeaderLocator
    {
        private readonly IFunctionContextAccessor _functionContextAccessor;

        public FunctionContextHeaderLocator(IFunctionContextAccessor functionContextAccessor)
        {
            _functionContextAccessor = functionContextAccessor;
        }
        
        public string? GetValue(string headerValue, string? defaultValue = null)
        {
            if (!(_functionContextAccessor.Headers?.Contains(headerValue) ?? false)) 
                return defaultValue;
            
            return _functionContextAccessor.Headers!.GetValues(headerValue).First();
        }
    }
}