using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class TruncateWordsFilterTests
    {
        [Test]
        public void It_Should_Truncate_By_Word()
        {
            // Arrange
            const String tmpl = @"{{ ""The cat came back the very next day"" | truncatewords: 4 }}";

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl);

            // Assert
            Assert.That(result, Is.EqualTo("The cat came back..."));
        }

        [Test]
        public void It_Should_Add_Something_Other_Than_Ellipses()
        {
            // Arrange
            const String tmpl = @"{{ ""The cat came back the very next day"" | truncatewords: 4, ""!!!"" }}";

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl);

            // Assert
            Assert.That(result, Is.EqualTo("The cat came back!!!"));
        }

        [Test]
        public void It_Should_Not_Add_Ellipses_With_Fewer_Words()
        {
            // Arrange
            const String tmpl = @"{{ ""The"" | truncatewords: 4 }}";

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl);

            // Assert
            Assert.That(result, Is.EqualTo("The"));
        }
        [Test]
        public void It_Should_Not_Count_Blank_Words()
        {
            // Arrange
            const String tmpl = @"{{ ""The cat     came    back the very next day"" | truncatewords: 4 }}";

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl);

            // Assert
            Assert.That(result, Is.EqualTo("The cat came back..."));
        }

    }
}
