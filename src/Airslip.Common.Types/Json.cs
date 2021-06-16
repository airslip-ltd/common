using System;
using Newtonsoft.Json;

namespace Airslip.Common.Types
{
    public static class Json
    {
        public const string MediaType = "application/json";

        public static string Serialize(object content)
        {
            return JsonConvert.SerializeObject(content);
        }

        public static T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value) ??
                   throw new InvalidOperationException("Value deserialized to null");
        }
    }
}