using System;
using Xunit;

namespace Liquid.NET.Tests.Tags
{
    
    public class CaseWhenElseBlockTagTests
    {
        [Fact]
        public void It_Should_Parse_A_Simple_Case_Tag()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {% case 'test' %}{% when 'test' %}TEST{% endcase %}");

            // Act
            Logger.Log(result);
            // Act

            // Assert
            Assert.Equal("Result : TEST", result);

        }

        [Fact]
        public void It_Should_Parse_A_Nested_Case_Tag()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {% case 'test' %}{% when 'test' %}{% case 'test' %}{% when 'nottest' %}NOT TEST{% else %}SUCCESS{% endcase %}{% endcase %}");

            // Act
            Logger.Log(result);
            // Act

            // Assert
            Assert.Equal("Result : SUCCESS", result);

        }

        [Theory]
        [InlineData("cake", "This is a cake")]
        [InlineData("cookie", "This is a cookie")]
        [InlineData("apple", "This is not a cake nor a cookie")]
        public void It_Should_Parse_A_Case_Tag(String handle, string expected)
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate(GetTestData(handle));

            // Act
            Logger.Log(result);

            // Assert
            Assert.Equal(expected, result.Trim());

        }

        [Fact]
        public void It_Should_Allow_Multiple_Cases_Separated_By_Commas()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("{% case 1%}{% when 1,2,3 %}One{%else%}Not One{%endcase%}");

            // Act
            Logger.Log(result);

            // Assert
            Assert.Equal("One", result.Trim());
        }

        [Fact]
        public void It_Should_Allow_Multiple_Cases_Separated_By_Or()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("{% case 1%}{% when 1 or 2 or 3 %}One{%else%}Not One{%endcase%}");

            // Act
            Logger.Log(result);

            // Assert
            Assert.Equal("One", result.Trim());
        }

        private String GetTestData(String handle)
        {
            return @"{% assign handle = '"+handle+@"' %}
{% case handle %}
  {% when 'cake' %}
     This is a cake
  {% when 'cookie' %}
     This is a cookie
  {% else %}
     This is not a cake nor a cookie
{% endcase %}";
        }
    }
}
