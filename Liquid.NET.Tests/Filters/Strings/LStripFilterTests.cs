using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class LStripFilterTests
    {
        [Fact]
        public void It_Should_Strip_Whitespace_on_The_Left()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ '   too many spaces           ' | lstrip }}");

            // Assert
            Assert.Equal("Result : too many spaces           ", result);

        }
       
    }
}
