using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class AppendFilterTests
    {
        [Fact]
        public void It_Should_Append_Text_To_A_String()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"test\" | append : \".jpg\" }}");

            // Assert
            Assert.Equal("Result : test.jpg", result);
        }

        [Fact]
        public void It_Should_Append_Text_To_Empty()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"\" | append : \"jpg\" }}");

            // Assert
            Assert.Equal("Result : jpg", result);
        }

        [Fact]
        public void It_Should_Append_Text_To_A_Number()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 123 | append : \".jpg\" }}");

            // Assert
            Assert.Equal("Result : 123.jpg", result);
        }

        [Fact]
        public void It_Should_Append_Text_To_Nothing()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ x | append : \".jpg\" }}");

            // Assert
            Assert.Equal("Result : .jpg", result);
        }

        [Fact] public void It_Should_Append_Nil_To_Nothing()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 'Test' | append : x }}");

            // Assert
            Assert.Equal("Result : Test", result);
        }
    }
}
