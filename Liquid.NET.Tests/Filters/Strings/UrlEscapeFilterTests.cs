using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class UrlEscapeFilterTests
    {
        [Fact]
        public void It_Should_UrlEscape_A_String()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"<hello> & <shopify>\" | url_escape }}");
           
            // Assert
            Assert.Equal("Result : %3Chello%3E%20&%20%3Cshopify%3E", result);

        }
    }
}
