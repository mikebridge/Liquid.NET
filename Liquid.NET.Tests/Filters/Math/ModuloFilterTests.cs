using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Math
{
    [TestFixture]
    public class ModuloFilterTests
    {
        [Test]
        [TestCase("5", "3", "2")]
        [TestCase("2", "3", "2")]
        [TestCase("0", "2", "0")]
        public void It_Should_Modulo_Two_Numbers(String arg1, String arg2, String expected)
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ " + arg1 + "  | modulo: " + arg2 + " }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expected));
        }

        [Test]
        public void It_Should_Error_When_Zero()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ 2  | modulo 2 }}");

            // Assert
            Assert.That(result, Is.EqualTo("Liquid error: divided by 0"));
        }

    }
}
