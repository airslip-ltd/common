using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Airslip.Common.Types
{
    public static class Json
    {
        public const string MediaType = "application/json";

        public static string Serialize(object value, Casing casing = Casing.CAMEL_CASE, Formatting formatting = Formatting.None)
        {
            JsonSerializerSettings jsonSerializerSettings = GetJsonSerializerSettings(casing, formatting);

            return JsonConvert.SerializeObject(value, jsonSerializerSettings);
        }

        public static T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value) ??
                   throw new InvalidOperationException("Value deserialized to null");
        }
        
        private static JsonSerializerSettings GetJsonSerializerSettings(Casing casing, Formatting formatting)
        {
            return casing switch
            {
                Casing.CAMEL_CASE => new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = formatting
                },
                Casing.SNAKE_CASE => new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    },
                    Formatting = formatting
                },
                _ => new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = formatting
                }
            };
        }
    }
}