using System;
using Xunit;

namespace Liquid.NET.Tests.Filters
{
    
    public class DefaultFilterTests
    {
        [Fact]
        public void It_Should_Return_Default_If_Null()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ mydate | default: \"Hello\" }}", ctx);

            // Assert
            Assert.Equal("Result : Hello", result);

        }

        [Fact]
        public void It_Should_Return_Default_If_Empty_String()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ \"\" | default: \"Hello\" }}", ctx);

            // Assert
            Assert.Equal("Result : Hello", result);

        }

        [Theory]
        [InlineData("\"  \"", "  ")]
        [InlineData("\"Test\"", "Test")]
        [InlineData("0", "0")]
        public void It_Should_Not_Return_Default_If_Not_Null_And_Not_Empty(String input, String expected)
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ "+input+" | default: \"Hello\" }}", ctx);

            // Assert
            Assert.Equal("Result : "+expected, result);

        }

        [Fact]
        public void It_Should_Return_Default_If_Array_Has_No_Elements()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% assign arr=\"\" | split: \"|\"%}{{ arr | default: \"DEFAULT\" }}", ctx);

            // Assert
            Assert.Equal("Result : DEFAULT", result);
        }

        [Fact]
        public void It_Should_Not_Return_Default_If_Array_Has_Elements()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% assign arr=\"1|2\" | split: \"|\"%}{{ arr | default: \"DEFAULT\" }}", ctx);

            // Assert
            Assert.Equal("Result : 12", result);
        }



        [Fact]
        public void It_Should_Return_Default_If_Array_Is_Null()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ arr | default: \"DEFAULT\" }}", ctx);

            // Assert
            Assert.Equal("Result : DEFAULT", result);
        }

    }
}
