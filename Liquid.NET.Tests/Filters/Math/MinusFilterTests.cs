using Xunit;

namespace Liquid.NET.Tests.Filters.Math
{
    
    public class MinusFilterTests
    {
        [Fact]
        public void It_Should_Subtract_One_Number_From_Another()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 23 | minus: 12 }}");

            // Assert
            Assert.Equal("Result : 11", result);
        }

        [Fact]
        public void It_Should_Cast_Strings_To_Numbers_When_Subtracting()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"23\" | minus: \"12\" }}");

            // Assert
            Assert.Equal("Result : 11", result);
        }

        [Fact]
        public void It_Should_Treat_Null_As_Zero()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ x | minus: \"12\" }}");

            // Assert
            Assert.Equal("Result : -12", result);
        }
    }
}
