using System;

namespace Airslip.Common.Types
{
    public static class CommonFunctions
    {
        public static string GetId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}