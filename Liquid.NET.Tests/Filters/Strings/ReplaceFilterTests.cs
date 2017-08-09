using Xunit;
using System.Globalization;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class ReplaceFilterTests
    {
        [Fact]
        public void It_Should_Replace_All_Instances_Of_Text_In_A_String()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"Hello, world. Goodbye, world.\" | replace : \"world\", 'mars' }}");

            // Assert
            Assert.Equal("Result : Hello, mars. Goodbye, mars.", result);
        }

        [Fact]
        public void It_Should_Replace_All_Instances_Of_A_Number_With_A_String()
        {
            // Arrange
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-CA");
            var result = RenderingHelper.RenderTemplate("Result : {{ 123456789123456789 | replace : '456' , 'x'}}");

            // Assert
            Assert.Equal("Result : 123x789123x789.0", result);
        }
    }
}
