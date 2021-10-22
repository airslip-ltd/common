using FluentAssertions;
using Xunit;

namespace Airslip.Common.Types.Tests
{
    public class CompositeIdTests
    {
        [Theory]
        [InlineData("c313209f1a1d473f915e1279ce377216|tx_0000ACUhTn5Jx8IM0eWPJ3")]
        [InlineData("188e8320dc054165b2dd775df4fc668b|13d8ea7a8dd9eaf4200cbbfe563a64f2.1")]
        public void Can_check_if_composite_id(string value)
        {
            bool isCompositeId = CompositeId.CheckIsComposite(value);
            isCompositeId.Should().BeTrue();
        }
        
        [Theory]
        [InlineData("c313209f1a1d473f915e1279ce377216|")]
        [InlineData("188e8320dc054165b2dd775df4fc668b")]
        public void Can_fail_if_invalid_composite_id(string value)
        {
            bool isCompositeId = CompositeId.CheckIsComposite(value);
            isCompositeId.Should().BeTrue();
        }
    }
}