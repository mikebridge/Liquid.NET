using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class StripFilterTests
    {
        [Fact]
        public void It_Should_Strip_Whitespace_on_Both_Sides()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ '   too many spaces           ' | strip }}");

            // Assert
            Assert.Equal("Result : too many spaces", result);

        }
    }
}
