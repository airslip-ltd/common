using Airslip.Common.Types.Extensions;
using FluentAssertions;
using System.Text;
using Xunit;

namespace Airslip.Common.Types.Tests
{
    public class StringExtensionsTests
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
        
        [Fact]
        public void Can_remove_accents_from_string()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string str = "Caffè Nero";
            string expectedString = "Caffe Nero";

            string normalisedString = str.RemoveAccents();
            
            normalisedString.Should().Be(expectedString);
        }
    }
}