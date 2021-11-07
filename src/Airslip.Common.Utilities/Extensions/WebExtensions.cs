using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Airslip.Common.Utilities.Extensions
{
    public static class WebExtensions
    {
        public static T GetQueryParams<T>(this string query) where T : class
        {
            NameValueCollection nvc = System.Web.HttpUtility.ParseQueryString(query);
            Dictionary<string, string?> formDictionary = nvc.AllKeys.ToDictionary(p => p!, p => nvc[p]);
            string json = Json.Serialize(formDictionary);
            return Json.Deserialize<T>(json);
        }
    }
}