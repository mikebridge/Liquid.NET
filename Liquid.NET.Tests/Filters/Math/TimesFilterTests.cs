using Xunit;

namespace Liquid.NET.Tests.Filters.Math
{
    
    public class TimesFilterTests
    {
        [Fact]
        public void It_Should_Multiply_Two_Numbers_Together()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 4 | times: 8 }}");

            // Assert
            Assert.Equal("Result : 32", result);
        }

        [Fact]
        public void It_Should_Cast_Strings()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"8\" | times: \"4\" }}");

            // Assert
            Assert.Equal("Result : 32", result);
        }

        [Fact]
        public void It_Should_Keep_The_Decimal_Places_1()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 4 | times: 8.0 }}");

            // Assert
            Assert.Equal("Result : 32.0", result);
        }

        [Fact]
        public void It_Should_Keep_The_Decimal_Places()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 4.0 | times: 8 }}");

            // Assert
            Assert.Equal("Result : 32.0", result);
        }


    }
}
