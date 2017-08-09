using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class PrependFilterTests
    {

        [Fact]
        public void It_Should_Prepend_Text_To_A_String()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"test\" | prepend : \"ABC\" }}");

            // Assert
            Assert.Equal("Result : ABCtest", result);
        }

        [Fact]
        public void It_Should_Prepend_Text_To_A_Number()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 123 | prepend : \"ABC\" }}");

            // Assert
            Assert.Equal("Result : ABC123", result);
        }

        [Fact]
        public void It_Should_Prepend_An_Int_To_An_int()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 456 | prepend : 123 }}");

            // Assert
            Assert.Equal("Result : 123456", result);
        }

        [Fact]
        public void It_Should_Prepend_Nil_A_String()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"test\" | prepend : x }}");

            // Assert
            Assert.Equal("Result : test", result);
        }

    }
}
