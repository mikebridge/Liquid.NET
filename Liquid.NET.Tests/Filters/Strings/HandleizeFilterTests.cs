using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class HandleizeFilterTests
    {
        [Test]
        public void It_SHould_Handleize_Text()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ '100% M & Ms!!!' | handleize }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 100-m-ms"));
        }

        [Test]
        [TestCase("ı","i")]
        [TestCase("ł","l")]
        [TestCase("Ł","l")]
        [TestCase("đ","d")]
        [TestCase("ß","ss")]
        [TestCase("ø","o")]
        [TestCase("Þ","th")]
        public void It_SHould_Handle_Some_Euro_Chars(String input, String expected)
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ '"+input+"' | handleize }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : "+expected));
        }

        [Test]
        public void It_SHould_Stop_At_80_Chars()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("{{ '123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890' | handleize }}");

            // Assert
            Assert.That(result.Length, Is.EqualTo(80));
        }

        [Test]
        public void It_Should_Convert_Null_To_Space()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("{{ nil | handleize }}");

            // Assert
            Assert.That(result, Is.EqualTo(""));
        }

        [Test]
        public void It_Should_Prevent_Duplicate_Hyphens()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ 'Test   Test' | handleize }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : test-test"));
        }

    }
}
