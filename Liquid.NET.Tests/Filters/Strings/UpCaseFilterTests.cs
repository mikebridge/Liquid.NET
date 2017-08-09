using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class UpCaseFilterTests
    {
        [Fact]
        public void It_Should_Put_A_String_In_Upper_Case()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"test\" | upcase }}");

            // Assert
            Assert.Equal("Result : TEST", result);

        }

        [Fact]
        public void It_Should_Not_Fail_For_Nil()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{  nil | upcase }}");

            // Assert
            Assert.Equal("Result : ", result);

        }

        [Fact]
        public void It_Should_Not_Fail_For_Numerics()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 1 | upcase }}");

            // Assert
            Assert.Equal("Result : 1", result);

        }

        [Fact]
        public void It_Should_Put_A_Bool_In_Upper_Case()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ true | upcase }}");

            // Assert
            Assert.Equal("Result : TRUE", result);

        }

        [Fact]
        public void It_Should_Ignore_An_Extra_Arg()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"Test\" | upcase: true }}");

            // Assert
            Assert.Equal("Result : TEST", result);

        }

    }
}
