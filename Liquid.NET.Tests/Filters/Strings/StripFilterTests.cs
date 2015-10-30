using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class StripFilterTests
    {
        [Test]
        public void It_Should_Strip_Whitespace_on_Both_Sides()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ '   too many spaces           ' | strip }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : too many spaces"));

        }
    }
}
