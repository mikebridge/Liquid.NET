using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class ReplaceFirstFilterTests
    {

        [Fact]
        public void It_Should_Replace_First_Text_In_A_String()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"Hello, world. Goodbye, world.\" | replace_first : \"world\", 'mars' }}");

            // Assert
            Assert.Equal("Result : Hello, mars. Goodbye, world.", result);
        }

        [Fact]
        public void It_Should_Replace_A_Number_With_A_String()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 123456789123456789 | replace_first : '456' , 'x'}}");

            // Assert
            Assert.Equal("Result : 123x789123456789.0", result);
        }



    }
}
