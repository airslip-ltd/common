using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Airslip.Common.Types.Tests
{
    public class ListTests
    {
        [Fact]
        public void Can_remove_first_value_from_list()
        {
            List<string> list = new()
            {
                "0","1","2"
            };

            list.RemoveFirst();
            
            List<string> expectedList = new()
            {
                "1","2"
            };

            list.Should().BeEquivalentTo(expectedList);
        }
    }
}