using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class TypeOfFilterTests
    {
        [Fact]
        public void It_Should_See_A_String()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"test\" | type_of }}");

            // Assert
            Assert.Equal("Result : string", result);

        }

        [Fact]
        public void It_Should_See_Nil()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ x | type_of }}");

            // Assert
            Assert.Equal("Result : nil", result);
        }


    }
}
