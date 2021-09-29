﻿using Airslip.Common.Types.Configuration;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Airslip.Common.Types.Extensions
{
    public static class StringExtensions
    {
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
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

        public static string RemoveAccents(this string accentedStr)
        {
            byte[] tempBytes = Encoding.GetEncoding("ISO-8859-8").GetBytes(accentedStr);
            return Encoding.UTF8.GetString(tempBytes);
        }
        
        public static string ToApiUrl(this PublicApiSetting publicApiSetting)
        {
            return string.IsNullOrEmpty(publicApiSetting.UriSuffix) ? $"{publicApiSetting.BaseUri}" :  $"{publicApiSetting.BaseUri}/{publicApiSetting.UriSuffix}";
        }
        
        public static Stream ToStream(this string s)
        {
            return s.ToStream(Encoding.UTF8);
        }

        public static Stream ToStream(this string s, Encoding encoding)
        {
            return new MemoryStream(encoding.GetBytes(s));
        }
    }
}