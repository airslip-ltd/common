using System;
using Airslip.Common.Types.Extensions;
using FluentAssertions;
using Xunit;

namespace Airslip.Common.Types.Tests
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void Can_get_earliest_date_in_epoch()
        {
            DateTimeOffset midDate = DateTimeOffset.Parse("2019-12-31");
            DateTimeOffset minDate = DateTimeOffset.Parse("2018-01-01");
            DateTimeOffset maxDate = DateTimeOffset.Parse("2020-02-13");

            DateTimeOffset[] dates = new DateTimeOffset[3];
            dates[0] = midDate;
            dates[1] = minDate;
            dates[2] = maxDate;

            long dateInEpoch = DateTimeExtensions.GetEarliestDateInEpoch(dates);
            dateInEpoch.Should().Be(1514764800000);
        }
        
        [Fact]
        public void Can_get_earliest_date_from_collection()
        {
            DateTimeOffset midDate = DateTimeOffset.Parse("2019-12-31");
            DateTimeOffset maxDate = DateTimeOffset.Parse("2020-02-13");
            DateTimeOffset minDate = DateTimeOffset.Parse("2018-01-01");

            DateTimeOffset[] dates = new DateTimeOffset[3];
            dates[0] = midDate;
            dates[1] = maxDate;
            dates[2] = minDate;

            DateTimeOffset date = DateTimeExtensions.GetEarliestDate(dates);
            date.Should().Be(new DateTimeOffset(new DateTime(2018, 01,01)));
        }
        
        [Fact]
        public void Can_get_months_between_dates()
        {
            DateTimeOffset startDate = DateTimeOffset.Parse("2018-02-28");
            DateTimeOffset endDate = DateTimeOffset.Parse("2020-02-13");

            int monthCount = DateTimeExtensions.GetMonthsBetweenDates(startDate, endDate);
            monthCount.Should().Be(23);
        }
    }
}