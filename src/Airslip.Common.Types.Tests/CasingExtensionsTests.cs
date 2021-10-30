﻿using Airslip.Common.Types.Extensions;
using FluentAssertions;
using System.Text;
using Xunit;

namespace Airslip.Common.Types.Tests
{
    public class CasingExtensionsTests
    {
        [Fact]
        public void Can_get_snake_case_from_camel_case()
        {
            string str = "BarclaycardCommercialPayments";

            string snakeCaseString = str.ToSnakeCase();
            
            string expectedString = "barclaycard_commercial_payments";

            snakeCaseString.Should().Be(expectedString);
        }
        
        [Fact]
        public void Can_get_snake_case_from_spaced_camel_case()
        {
            string str = "Barclaycard Commercial Payments";
            string expectedString = "barclaycard_commercial_payments";

            string snakeCaseString = str.ToSnakeCase();
            
            snakeCaseString.Should().Be(expectedString);
        }
        
        [Theory]
        [InlineData("ICanGetAString")]
        [InlineData("ICan_GetAString")]
        [InlineData("i-can-get-a-string")]
        [InlineData("i_can_get_a_string")]
        [InlineData("I Can Get A String")]
        public void Can_get_kebab_case_from_string(string value)
        {
            string expectedString = "i-can-get-a-string";

            string snakeCaseString = value.ToKebabCasing();
            
            snakeCaseString.Should().Be(expectedString);
        }
    }
}