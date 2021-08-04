using System;

namespace Airslip.Common.Types
{
    public static class Common
    {
        public static string GetId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}