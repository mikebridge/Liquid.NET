using Xunit;

namespace Liquid.NET.Tests.Tags
{
    
    public class UnlessBlockTagTests
    {
        [Fact]
        public void It_Should_Render_If_False()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% unless false %}OK{% endunless %}");

            // Assert
            Assert.Equal("Result : OK", result);
        }

        [Fact]
        public void It_Should_Not_Render_If_True()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% unless true %}OK{% endunless %}");

            // Assert
            Assert.Equal("Result : ", result);
        }

        [Fact]
        public void It_Should_Render_Else()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% unless true %}OK{% else %}Else{% endunless %}");

            // Assert
            Assert.Equal("Result : Else", result);
        }

        [Fact]
        public void It_Should_Render_Elsif()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% unless true %}OK{% elsif true %}Else If{% else %}Else{% endunless %}");

            // Assert
            Assert.Equal("Result : Else If", result);
        }

        [Fact]
        public void It_Should_Render_Second_Elsif()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% unless true %}OK{% elsif false %}Else If{% elsif true %}second else{% else %}Else{% endunless %}");

            // Assert
            Assert.Equal("Result : second else", result);
        }

        [Fact]
        public void It_Should_Render_Nested_Unless_Statements()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% unless false %}UNLESS1{% if false %}NOT OK{% endif %}{% unless false %}UNLESS2{% endunless %}{% endunless %}");

            // Assert
            Assert.Equal("Result : UNLESS1UNLESS2", result);
        }
    }
}
