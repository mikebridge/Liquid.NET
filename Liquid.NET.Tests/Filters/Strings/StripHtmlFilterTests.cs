using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class StripHtmlFilterTests
    {
        [Fact]
        public void It_Should_Strip_Html()
        {
            // Arrange
            const string s = "{{ \"<h1>Hello</h1> World\" | strip_html }}";
            // Act
            var result = RenderingHelper.RenderTemplate(s);
            // Assert
            Assert.Equal("Hello World", result);

        }


    }
}
