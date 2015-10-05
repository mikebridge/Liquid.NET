using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Tags
{
    [TestFixture]
    public class CustomSyntaxBlockTagTests
    {
        [Test]
        public void It_Should_Parse_A_Custom_Tag()
        {
            // Act
            var templateContext = new TemplateContext().WithAllFilters();

            var result = RenderingHelper.RenderTemplate("Result : {% myfor x to y %}", templateContext);
            Console.WriteLine(result);

            // Assert
            Assert.That(result, Is.EqualTo("Result : 1 2 3"));

        }

        [Test]
        public void It_Should_Parse_A_Custom_Tag_With_Reserved_words()
        {
            // Act
            var templateContext = new TemplateContext().WithAllFilters();

            var result = RenderingHelper.RenderTemplate("Result : {% myfor x in y %}", templateContext);
            Console.WriteLine(result);

            // Assert
            Assert.That(result, Is.EqualTo("Result : 1 2 3"));

        }
    }
}
