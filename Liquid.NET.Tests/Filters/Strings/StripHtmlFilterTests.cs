using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class StripHtmlFilterTests
    {
        [Test]
        public void It_Should_Strip_Html()
        {
            // Arrange
            const string s = "{{ \"<h1>Hello</h1> World\" | strip_html }}";
            // Act
            var result = RenderingHelper.RenderTemplate(s);
            // Assert
            Assert.That(result, Is.EqualTo("Hello World"));

        }


    }
}
