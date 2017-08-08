using System;
using System.Globalization;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests.Utils
{
    [TestFixture]
    public class StrFTimeTests
    {
        //[SetCulture("en-GB")]
        [TestCase("%a", ExpectedResult = "Sun")]
        [TestCase("%A", ExpectedResult = "Sunday")]
        [TestCase("%b", ExpectedResult = "Jan")]
        [TestCase("%B", ExpectedResult = "January")]
        [TestCase("%^B", ExpectedResult = "JANUARY")]
        [TestCase("%^_10B", ExpectedResult = "   JANUARY")]
        [TestCase("%_^10B", ExpectedResult = "   JANUARY")]
        [TestCase("%c", ExpectedResult = "Sun Jan 08 14:32:14 2012")]
        [TestCase("%d", ExpectedResult = "08")]
        [TestCase("%-d", ExpectedResult = "8")]
        [TestCase("%e", ExpectedResult = " 8")]
        [TestCase("%H", ExpectedResult = "14")]
        [TestCase("%I", ExpectedResult = "02")]
        [TestCase("%j", ExpectedResult = "008")]
        [TestCase("%m", ExpectedResult = "01")]
        [TestCase("%-m", ExpectedResult = "1")]
        [TestCase("%_m", ExpectedResult = " 1")]
        [TestCase("%_3m", ExpectedResult = "  1")]
        [TestCase("%_10m", ExpectedResult = "         1")]
        [TestCase("%010m", ExpectedResult = "0000000001")]
        [TestCase("%M", ExpectedResult = "32")]
        [TestCase("%p", ExpectedResult = "PM")]
        [TestCase("%S", ExpectedResult = "14")]
        [TestCase("%U", ExpectedResult = "02")]
        [TestCase("%-U", ExpectedResult = "2")]
        [TestCase("%W", ExpectedResult = "01")]
        [TestCase("%w", ExpectedResult = "0")]
        [TestCase("%x", ExpectedResult = "08/01/2012")]
        [TestCase("%X", ExpectedResult = "14:32:14")]
        [TestCase("%y", ExpectedResult = "12")]
        [TestCase("%Y", ExpectedResult = "2012")]
        [TestCase("%_2Y", ExpectedResult = "2012")]
        [TestCase("%", ExpectedResult = "%")]
        public string The_Date_Should_Be_Formatted_With_StrFTime(string format, string expected)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-GB");
            return new DateTime(2012, 1, 8, 14, 32, 14).ToStrFTime(format);
        }

        [Test]
        public void The_Time_Zone_Should_Be_Formatted_With_StrFTime()
        {
            var now = DateTimeOffset.Now;
            string timeZoneOffset = now.ToString("zzz");
            Assert.That(now.DateTime.ToStrFTime("%Z"), Is.EqualTo(timeZoneOffset));
        }
    }
}