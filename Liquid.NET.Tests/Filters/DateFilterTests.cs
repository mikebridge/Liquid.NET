using System;
using Liquid.NET.Constants;
using Xunit;

namespace Liquid.NET.Tests.Filters
{
    /// <summary>
    /// See:  See: http://www.ruby-doc.org/core/Time.html#method-i-strftime
    /// </summary>
       
    public class DateFilterTests
    {
        [Theory]
        [InlineData("%y", "15")]
        [InlineData("%B", "March")]
        [InlineData("%m/%d/%Y", "03/30/2015")]
        public void It_Should_Format_A_Date(String format, string expected)
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            DateTime dateTime = new DateTime(2015, 3, 30, 23, 1, 12);
            ctx.DefineLocalVariable("mydate", new LiquidDate(dateTime));
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ mydate | date: \""+format+"\" }}", ctx);

            // Assert
            Assert.Equal("Result : "+expected, result);
        }

        [Fact]
        public void It_Should_Ignore_An_Empty_Date()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("mydate", new LiquidDate(null));
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ mydate | date: \"%y\" }}", ctx);

            // Assert
            Assert.Equal("Result : ", result);

        }

        [Fact]
        public void It_Should_Ignore_A_Missing_DateValue()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            //ctx.Define("mydate", null);
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ mydate | date: \"%y\" }}", ctx);

            // Assert
            Assert.Equal("Result : ", result);

        }

        [Fact]
        public void It_Should_Return_The_Default_When_No_Format()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            DateTime dateTime = new DateTime(2015, 8, 19, 23, 1, 12);
            ctx.DefineLocalVariable("mydate", new LiquidDate(dateTime));

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ mydate | date }}", ctx);

            // Assert
            Assert.Equal("Result : 08/19/2015", result);

        }

        [Fact]
        public void It_Should_Not_Fail_When_Format_Is_Invalid()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            DateTime dateTime = new DateTime(2015, 3, 30, 23, 1, 12);
            ctx.DefineLocalVariable("mydate", new LiquidDate(dateTime));

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ mydate | date : \"%V\" }}", ctx);

            // Assert
            Assert.Equal("Result : %V", result);

        }


        [Fact]
        public void It_Should_Format_With_Hour_Day_Month_Minute_Meridian_Second()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            DateTime localDateTime = new DateTime(2014, 9, 13, 16, 29, 55, DateTimeKind.Local); //Saturday, September 13, 2014, 4:29:55 PM MDT/MST
            ctx.DefineLocalVariable("mydate", new LiquidDate(localDateTime));
            String resultString = "HourOfDay24 16, HourOfDay12 04, DayOfYear 256, MonthOfYear 09, Minute 29, Meridian PM, Second 55";
            
            // Act
            String result = RenderingHelper.RenderTemplate("Result: {{ mydate | date: 'HourOfDay24 %H, HourOfDay12 %I, DayOfYear %j, MonthOfYear %m, Minute %M, Meridian %p, Second %S' }}", ctx);

            // Assert
            Logger.Log(result);
            Assert.Equal("Result: " + resultString, result.Trim());
        }

    }
}
