using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class EscapeFilterTests
    {
        [Fact]
        public void It_Should_Escape_Html_Entities()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"<p>test</p>\" | escape }}");

            // Assert
            Assert.Equal("Result : &lt;p&gt;test&lt;/p&gt;", result);

        }
        //
    }
}
