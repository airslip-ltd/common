using Airslip.Common.Types.Enums;
using Airslip.Common.Utilities.Extensions;
using FluentAssertions;
using Xunit;

namespace Airslip.Common.Utilities.UnitTests
{
    public class EnumExtensionsTests
    {
        [Theory]
        [InlineData("shopify")]
        [InlineData("Shopify")]
        public void Can_remove_accents_from_string(string posProvider)
        {
            bool canParse = EnumExtensions.TryParseIgnoreCase(posProvider, out PosProviders provider);

            canParse.Should().BeTrue();
            Assert.True(provider == PosProviders.Shopify);
        }
    }
}