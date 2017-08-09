using Liquid.NET.Constants;
using Liquid.NET.Filters.Strings;
using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class RemoveFilterTests
    {
        [Fact]
        public void It_Should_Remove_an_Integer()
        {
            // Arrange
            var removeFilter = new RemoveFilter(LiquidString.Create("123"));

            // Act
            var result = removeFilter.Apply(new TemplateContext(), LiquidString.Create("Remove the 123 in this string.")).SuccessValue<LiquidString>();

            // Assert
            Assert.Equal("Remove the  in this string.", result.Value);

        }

        [Fact]
        public void It_Should_Remove_MultipleText_From_A_String()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"testtest\" | remove : \"te\" }}");

            // Assert
            Assert.Equal("Result : stst", result);
        }

        [Fact]
        public void It_Should_Remove_A_Number_From_A_Number()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 123456789 | remove : 456 }}");

            // Assert
            Assert.Equal("Result : 123789", result);
        }

        [Fact]
        public void It_Should_Remove_Nil_From_A_String()
        {
            // Arrange

            //var result = RenderingHelper.RenderTemplate("Result : {{ \"test\" | remove : x }}");
            var template = LiquidTemplate.Create("Result : {{ \"test\" | remove : x }}");
            var result = template.LiquidTemplate.Render(new TemplateContext().WithAllFilters());

            // Assert
            Assert.Equal("Result : ERROR: Please specify a replacement string.", result.Result);
        }


    }
}
