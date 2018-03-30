using System;
using System.Globalization;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests.Utils
{
    
    public class StrFTimeTests
    {
        //[SetCulture("en-GB")]
        [Theory]
        [InlineData("%a", "Sun")]
        [InlineData("%A", "Sunday")]
        [InlineData("%b", "Jan")]
        [InlineData("%B", "January")]
        [InlineData("%^B", "JANUARY")]
        [InlineData("%^_10B", "   JANUARY")]
        [InlineData("%_^10B", "   JANUARY")]
        [InlineData("%c", "Sun Jan 08 14:32:14 2012")]
        [InlineData("%d", "08")]
        [InlineData("%-d", "8")]
        [InlineData("%e", " 8")]
        [InlineData("%H", "14")]
        [InlineData("%I", "02")]
        [InlineData("%j", "008")]
        [InlineData("%m", "01")]
        [InlineData("%-m", "1")]
        [InlineData("%_m", " 1")]
        [InlineData("%_3m", "  1")]
        [InlineData("%_10m", "         1")]
        [InlineData("%010m", "0000000001")]
        [InlineData("%M", "32")]
        [InlineData("%p", "PM")]
        [InlineData("%S", "14")]
        [InlineData("%U", "02")]
        [InlineData("%-U", "2")]
        [InlineData("%W", "01")]
        [InlineData("%w", "0")]
        [InlineData("%x", "08/01/2012")]
        [InlineData("%X", "14:32:14")]
        [InlineData("%y", "12")]
        [InlineData("%Y", "2012")]
        [InlineData("%_2Y", "2012")]
        [InlineData("%", "%")]
        public void The_Date_Should_Be_Formatted_With_StrFTime(string format, string expected)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-GB");
            Assert.Equal(expected, new DateTime(2012, 1, 8, 14, 32, 14).ToStrFTime(format));
        }

        [Fact]
        public void The_Time_Zone_Should_Be_Formatted_With_StrFTime()
        {
            var now = DateTimeOffset.Now;
            string timeZoneOffset = now.ToString("zzz");
            Assert.Equal(timeZoneOffset, now.DateTime.ToStrFTime("%Z"));
        }
    }
}