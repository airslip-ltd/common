using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Airslip.Common.Utilities.Extensions
{
    public static class WebExtensions
    {
        public static T GetQueryParams<T>(this string query) where T : class
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(query);
            Dictionary<string, string?> formDictionary = nvc.AllKeys.ToDictionary(p => p!, p => nvc[p]);
            string json = Json.Serialize(formDictionary);
            return Json.Deserialize<T>(json);
        }

        public static IEnumerable<KeyValuePair<string, string>> GetQueryParams(this string query, bool isBase64Encoded = false)
        {
            int i = query.IndexOf("?", StringComparison.Ordinal);
            string queryWithoutBaseUrl = i != -1 ? query[i..] : query;
            
            NameValueCollection nvc = HttpUtility.ParseQueryString(queryWithoutBaseUrl);

            if (!nvc.HasKeys())
                return new List<KeyValuePair<string, string>>();
            
            return nvc.AllKeys.SelectMany(
                nvc.GetValues!,
                (k, v) => new KeyValuePair<string, string>(k!, isBase64Encoded ? v.Replace(" ", "+") : v));
        }
        
        public static KeyValuePair<string, string> Get(this IEnumerable<KeyValuePair<string, string>> source, string key)
        {
            return source.FirstOrDefault(pair => pair.Key == key);
        }
        
        public static string GetValue(this IEnumerable<KeyValuePair<string, string>> source, string key)
        {
            return source.FirstOrDefault(pair => pair.Key == key).Value;
        }
    }
}