using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class EscapeFilterTests
    {
        [Test]
        public void It_Should_Escape_Html_Entities()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"<p>test</p>\" | escape }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : &lt;p&gt;test&lt;/p&gt;"));

        }
        //
    }
}
