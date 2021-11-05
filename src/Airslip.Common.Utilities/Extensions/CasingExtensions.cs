using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Airslip.Common.Utilities.Extensions
{
    public static class CasingExtensions
    {
        public static string ToCamelCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1) return char.ToLowerInvariant(str[0]) + str[1..];
            return str;
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

            return result.Substring(0, result.Length - 1);
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

        public static string ToSnakeCase(this string str) =>
            string.Concat(str.Replace(" ", "").Select((x, i) =>
                i > 0 && char.IsUpper(x) && !char.IsUpper(str[i - 1]) ? $"_{x}" : x.ToString())).ToLower();

        // Needs improving
        public static string ToKebabCasing(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return Regex.Replace(
                input.Replace("_", "-"),
                @"(?<!^)(?<!-)((?<=\p{Ll})\p{Lu}|\p{Lu}(?=\p{Ll}))", 
                "-$1")
                .ToLower()
                .Replace(" ", "-")
                .Replace("--", "-");
        }
    }
}