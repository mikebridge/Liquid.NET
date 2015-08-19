using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class PrependFilterTests
    {

        [Test]
        public void It_Should_Prepend_Text_To_A_String()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"test\" | prepend : \"ABC\" }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : ABCtest"));
        }

        [Test]
        public void It_Should_Prepend_Text_To_A_Number()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 123 | prepend : \"ABC\" }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : ABC123"));
        }

        [Test]
        public void It_Should_Prepend_An_Int_To_An_int()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 456 | prepend : 123 }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 123456"));
        }

        [Test]
        public void It_Should_Prepend_Nil_A_String()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"test\" | prepend : x }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : test"));
        }

    }
}
