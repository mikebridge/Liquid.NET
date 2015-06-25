using Liquid.NET.Constants;
using Liquid.NET.Filters.Strings;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class RemoveFilterTests
    {
        [Test]
        public void It_Should_Remove_an_Integer()
        {
            // Arrange
            var removeFilter = new RemoveFilter(new StringValue("123"));

            // Act
            var result = removeFilter.Apply(new StringValue("Remove the 123 in this string.")).SuccessValue<StringValue>();

            // Assert
            Assert.That(result.Value, Is.EqualTo("Remove the  in this string."));

        }

        [Test]
        public void It_Should_Remove_MultipleText_From_A_String()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"testtest\" | remove : \"te\" }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : stst"));
        }

        [Test]
        public void It_Should_Remove_A_Number_From_A_Number()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 123456789 | remove : 456 }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 123789"));
        }

    }
}
