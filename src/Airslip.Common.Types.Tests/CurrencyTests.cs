using System.Globalization;
using FluentAssertions;
using Xunit;

namespace Airslip.Common.Types.Tests
{
    public class CurrencyTests
    {
        [Fact]
        public void Can_convert_decimal_to_unit_value()
        {
            long? pennies = Currency.ConvertToUnit("12.99");
            pennies.Should().Be(1299);
        }
        
        [Fact]
        public void Can_convert_unit_to_unit_value()
        {
            long? pennies = Currency.ConvertToUnit("12");
            pennies.Should().Be(12);
        }
        
        [Fact]
        public void Empty_currency_returns_null()
        {
            long? pennies = Currency.ConvertToUnit("");
            pennies.Should().Be(null);
        }
    }
}