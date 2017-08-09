using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class UrlParamEscapeFilterTests
    {
        [Fact]
        public void It_Should_Render_Url_Params()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"<hello> & <shopify>\" | url_param_escape }}");

            // Assert
            Assert.Equal("Result : %3Chello%3E%20%26%20%3Cshopify%3E", result);
        }
    }
}
