using FluentAssertions;
using Xunit;

namespace Airslip.Common.Types.Tests
{
    public class CurrencyTests
    {
        [Fact]
        public void Can_convert_string_decimal_to_unit_value()
        {
            long? pennies = Currency.ConvertToUnit("12.99");
            pennies.Should().Be(1299);
        }
        
        [Fact]
        public void Can_convert_string_whole_number_to_unit_value()
        {
            long? pennies = Currency.ConvertToUnit("12");
            pennies.Should().Be(1200);
        }
        
        [Fact]
        public void Empty_currency_returns_null()
        {
            long? pennies = Currency.ConvertToUnit("");
            pennies.Should().Be(null);
        }
        
        [Fact]
        public void Can_convert_double_to_unit_value()
        {
            double value = 12.99;
            long? pennies = Currency.ConvertToUnit(value);
            pennies.Should().Be(1299);
        }
        
        [Fact]
        public void Can_convert_decimal_to_unit_value()
        {
            decimal value = (decimal)12.99;
            long? pennies = Currency.ConvertToUnit(value);
            pennies.Should().Be(1299);
        }
        
        [Fact]
        public void Can_convert_double_whole_number_to_unit_value()
        {
            double value = 12;
            long? pennies = Currency.ConvertToUnit(value);
            pennies.Should().Be(1200);
        }
        
        [Fact]
        public void Can_convert_decimal_whole_number_to_unit_value()
        {
            decimal value = 12;
            long? pennies = Currency.ConvertToUnit(value);
            pennies.Should().Be(1200);
        }
        
        [Fact]
        public void Can_convert_unit_value_to_decimal()
        {
            long value = 1299;
            decimal? currency = Currency.ConvertToTwoPlacedDecimal(value);
            currency.Should().Be((decimal)12.99);
        }
    }
}