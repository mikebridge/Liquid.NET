using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class LStripFilterTests
    {
        [Test]
        public void It_Should_Strip_Whitespace_on_The_Left()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ '   too many spaces           ' | lstrip }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : too many spaces           "));

        }
       
    }
}
