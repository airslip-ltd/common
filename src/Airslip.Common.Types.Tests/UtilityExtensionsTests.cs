﻿using Airslip.Common.Types.Configuration;
using Airslip.Common.Types.Extensions;
using FluentAssertions;
using Xunit;

namespace Airslip.Common.Types.Tests
{
    public class UtilityExtensionsTests
    {
        [Fact]
        public void Can_detect_string_in_list()
        {
            var result = "myval".InList("myval", "anotherval");

            result.Should().BeTrue();
        }
        
        [Fact]
        public void Can_detect_int_in_list()
        {
            var result = 1000.InList(1000, 1001);

            result.Should().BeTrue();
        }
        
        [Fact]
        public void Can_detect_enum_in_list()
        {
            var result = MyEnum.Val1.InList(MyEnum.Val1, MyEnum.Val2);

            result.Should().BeTrue();
        }
        
        [Fact]
        public void Doesnt_detect_string_in_list()
        {
            var result = "myval".InList("notmyval", "anotherval");

            result.Should().BeFalse();
        }
        
        [Fact]
        public void Doesnt_detect_int_in_list()
        {
            var result = 1000.InList(2000, 2001);

            result.Should().BeFalse();
        }
        
        [Fact]
        public void Can_create_base_uri()
        {
            PublicApiSetting setting = new()
            {
                BaseUri = "https://test.airslip.com",
                UriSuffix = "airslip",
                Version = "v1"
            };

            string baseUri = setting.ToBaseUri();
            
            baseUri.Should().Be("https://test.airslip.com/airslip/v1");
        }

        private enum MyEnum
        {
            Val1,
            Val2
        }
    }
}