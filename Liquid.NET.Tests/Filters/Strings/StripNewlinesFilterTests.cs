using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class StripNewlinesFilterTests
    {
        [Test]
        public void It_Should_Strip_Newlines_From_A_String()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ \"IT\r\nShould\r\nStrip\rNewlines\n\" | strip_newlines }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : ITShouldStripNewlines"));

        }

        [Test]
        public void It_Should_Strip_Obscure_Newlines()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ \"1\u000A2\u000B3\u000C4\u000D5\u20286\u20297\u00858\" | strip_newlines }}");

            // Assert
            //Assert.That(result, Is.EqualTo("Result : 1 2 3 4 5 6 7 8"));
            Assert.That(result, Is.EqualTo("Result : 12345678"));

        }

    }
}
