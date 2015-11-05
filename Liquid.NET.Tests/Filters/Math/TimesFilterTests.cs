using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Math
{
    [TestFixture]
    public class TimesFilterTests
    {
        [Test]
        public void It_Should_Multiply_Two_Numbers_Together()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 4 | times: 8 }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 32"));
        }

        [Test]
        public void It_Should_Cast_Strings()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"8\" | times: \"4\" }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 32"));
        }

        [Test]
        public void It_Should_Keep_The_Decimal_Places_1()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 4 | times: 8.0 }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 32.0"));
        }

        [Test]
        public void It_Should_Keep_The_Decimal_Places()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 4.0 | times: 8 }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 32.0"));
        }


    }
}
