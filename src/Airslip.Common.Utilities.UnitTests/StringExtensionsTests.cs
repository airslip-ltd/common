﻿using Airslip.Common.Utilities.Extensions;
using FluentAssertions;
using System.Text;
using Xunit;

namespace Airslip.Common.Utilities.UnitTests
{
    public class StringExtensionsTests
    {
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