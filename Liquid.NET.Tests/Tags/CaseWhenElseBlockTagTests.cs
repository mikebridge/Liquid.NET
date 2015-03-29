using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Tags
{
    [TestFixture]
    public class CaseWhenElseBlockTagTests
    {
        [Test]
        public void It_Should_Parse_A_Simple_Case_Tag()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {% case 'test' %}{% when 'test' %}TEST{% endcase %}");

            // Act
            Console.WriteLine(result);
            // Act

            // Assert
            Assert.That(result, Is.EqualTo("Result : TEST"));

        }

        [Test]
        public void It_Should_Parse_A_Nested_Case_Tag()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {% case 'test' %}{% when 'test' %}{% case 'test' %}{% when 'nottest' %}NOT TEST{% else %}SUCCESS{% endcase %}{% endcase %}");

            // Act
            Console.WriteLine(result);
            // Act

            // Assert
            Assert.That(result, Is.EqualTo("Result : SUCCESS"));

        }

        [Test]
        [TestCase("cake", "This is a cake")]
        [TestCase("cookie", "This is a cookie")]
        [TestCase("apple", "This is not a cake nor a cookie")]
        public void It_Should_Parse_A_Case_Tag(String handle, string expected)
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate(GetTestData(handle));

            // Act
            Console.WriteLine(result);

            // Assert
            Assert.That(result.Trim(), Is.EqualTo(expected));

        }

        private String GetTestData(String handle)
        {
            return @"{% assign handle = '"+handle+@"' %}
{% case handle %}
  {% when 'cake' %}
     This is a cake
  {% when 'cookie' %}
     This is a cookie
  {% else %}
     This is not a cake nor a cookie
{% endcase %}";
        }
    }
}
