using System;
using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class HandleizeFilterTests
    {
        [Fact]
        public void It_SHould_Handleize_Text()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ '100% M & Ms!!!' | handleize }}");

            // Assert
            Assert.Equal("Result : 100-m-ms", result);
        }

        [Theory]
        [InlineData("ı","i")]
        [InlineData("ł","l")]
        [InlineData("Ł","l")]
        [InlineData("đ","d")]
        [InlineData("ß","ss")]
        [InlineData("ø","o")]
        [InlineData("Þ","th")]
        public void It_SHould_Handle_Some_Euro_Chars(String input, String expected)
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ '"+input+"' | handleize }}");

            // Assert
            Assert.Equal("Result : "+expected, result);
        }

        [Fact]
        public void It_SHould_Stop_At_80_Chars()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("{{ '123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890' | handleize }}");

            // Assert
            Assert.Equal(80, result.Length);
        }

        [Fact]
        public void It_Should_Convert_Null_To_Space()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("{{ nil | handleize }}");

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void It_Should_Prevent_Duplicate_Hyphens()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ 'Test   Test' | handleize }}");

            // Assert
            Assert.Equal("Result : test-test", result);
        }

    }
}
