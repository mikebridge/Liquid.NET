using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class RStripFilterTests
    {
        [Fact]
        public void It_Should_Strip_Whitespace_on_The_Right()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ '   too many spaces           ' | rstrip }}");

            // Assert
            Assert.Equal("Result :    too many spaces", result);

        }
    }
}
