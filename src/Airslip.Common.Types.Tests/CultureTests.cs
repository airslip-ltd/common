using System;
using System.Globalization;
using Airslip.Common.Types.Extensions;
using FluentAssertions;
using Xunit;

namespace Airslip.Common.Types.Tests
{
    public class CultureTests
    {
        [Fact]
        public void Can_get_british_currency_symbol_from_iso417_currencyCode()
        {
            string currencySymbol = Culture.GetCurrencySymbol("GBP");
            currencySymbol.Should().Be("£");
        }
        
        [Fact]
        public void Can_get_american_currency_symbol_from_iso417_currencyCode()
        {
            string currencySymbol = Culture.GetCurrencySymbol("USD");
            currencySymbol.Should().Be("$");
        }
        
        [Fact]
        public void Can_get_euro_currency_symbol_from_iso417_currencyCode()
        {
            string currencySymbol = Culture.GetCurrencySymbol("EUR");
            currencySymbol.Should().Be("€");
        }

        [Fact]
        public void Can_get_culture_info_from_british_currency_code()
        {
            CultureInfo cultureInfo = Culture.GetCultureInfoFromCurrencyCode("GBP");

            cultureInfo.Name.Should().Be("en-GB");
        }
        
        [Fact]
        public void Can_get_culture_info_from_american_currency_code()
        {
            CultureInfo cultureInfo = Culture.GetCultureInfoFromCurrencyCode("USD");

            cultureInfo.Name.Should().Be("en-US");
        }
        
        [Fact]
        public void Can_get_culture_info_from_euro_currency_code()
        {
            CultureInfo cultureInfo = Culture.GetCultureInfoFromCurrencyCode("EUR");

            cultureInfo.Name.Should().Be("es-ES");
        }
    }
}