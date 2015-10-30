using Liquid.NET.Filters.Strings;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class NewlineToBrFilterTests
    {
        [Test]
        public void It_Should_Replace_Newlines_From_A_String()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ \"IT\r\n\r\nShould\r\n\rStrip\rNewlines\n\" | newline_to_br }}");

            // Assert
            const string br = NewlineToBrFilter.BR;
            Assert.That(result, Is.EqualTo("Result : IT" + br + br + "Should" + br + br + "Strip" + br + "Newlines" + br ));

        }

        [Test]
        public void It_Should_Replace_Obscure_Newlines()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ \"1\u000A2\u000B3\u000C4\u000D5\u20286\u20297\u00858\" | newline_to_br }}");

            // Assert
            const string br = NewlineToBrFilter.BR;
            Assert.That(result, Is.EqualTo("Result : 1" + br + "2" + br + "3" + br + "4" + br + "5" + br + "6" + br + "7" + br + "8"));

        }

    }
}
