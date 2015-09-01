using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Math
{
    [TestFixture]
    public class PlusFilterTests
    {

        [Test]
        public void It_Should_Add_Two_Numbers_Together()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 11 | plus: 12 }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 23"));
        }

        [Test]
        public void It_Should_Cast_Strings()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"11\" | plus: \"12\" }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 23"));
        }

        [Test]
        public void It_Should_Keep_Accuracy_In_A_Filter()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {% assign x = 1 | plus: 12.0 %}{{ x }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 13.0"));
        }

        [Test]
        public void It_Should_Add_To_Null()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("{% assign val=null %}Result : {{ val | plus: \"12\" }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 12"));
        }

    }
}
