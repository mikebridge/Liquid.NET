using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class TypeOfFilterTests
    {
        [Test]
        public void It_Should_See_A_String()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"test\" | type_of }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : string"));

        }

        [Test]
        public void It_Should_See_Nil()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ x | type_of }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : nil"));
        }


    }
}
