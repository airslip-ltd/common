using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Airslip.Common.Types
{
    public static class StringExtensions
    {
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static string ToPascalCase(this string original)
        {
            IEnumerable<string> pascalCase = original.GetEnumerablePascalCase();

            return string.Concat(pascalCase);
        }

        public static string ToSpacedPascalCase(this string original)
        {
            string[] pascalCaseWords = original.GetEnumerablePascalCase().ToArray();
            string result =
                pascalCaseWords.Aggregate(string.Empty, (current, pascalCase) => current + $"{pascalCase} ");

            return result is null ? string.Empty : result.Substring(0, result.Length - 1);
        }

        private static IEnumerable<string> GetEnumerablePascalCase(this string original)
        {
            Regex invalidCharsRgx = new("[^_a-zA-Z0-9]");
            Regex whiteSpace = new(@"(?<=\s)");
            Regex startsWithLowerCaseChar = new("^[a-z]");
            Regex firstCharFollowedByUpperCasesOnly = new("(?<=[A-Z])[A-Z0-9]+$");
            Regex lowerCaseNextToNumber = new("(?<=[0-9])[a-z]");
            Regex upperCaseInside = new("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");

            return invalidCharsRgx.Replace(whiteSpace.Replace(original, "_"), string.Empty)
                .Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpper()))
                .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLower()))
                .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpper()))
                .Select(w => upperCaseInside.Replace(w, m => m.Value.ToLower()));
        }

        public static string ToCamelCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1) return char.ToLowerInvariant(str[0]) + str[1..];
            return str;
        }

        public static long ToUnixTimeMilliseconds(this string value)
        {
            return DateTimeOffset.Parse(value, DateTimeFormatInfo.InvariantInfo).ToUnixTimeMilliseconds();
        }

        public static bool IsInArray(this string subject, params string[] arr)
        {
            return arr
                .Select(item => subject.Equals(item, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault(isInArray => isInArray);
        }

        public static bool CheckIsUrl(this string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }

        public static bool TryParseUtcDateTime(this string datetimeString)
        {
            return DateTime.TryParseExact(datetimeString, "o", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out _);
        }

        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
    }
}