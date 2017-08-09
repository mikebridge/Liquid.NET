using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class RemoveFirstFilterTests
    {
     
        [Fact]
        public void It_Should_Remove_First_Text_From_A_String()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"Hello, world. Goodbye, world.\" | remove_first : \"world\" }}");

            // Assert
            Assert.Equal("Result : Hello, . Goodbye, world.", result);
        }

        [Fact]
        public void It_Should_Remove_A_Number_From_A_Number()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 123456789123456789 | remove_first : 456 }}");

            // Assert
            Assert.Equal("Result : 123789123456789.0", result);
        }
    }
}
