using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class UpCaseFilterTests
    {
        [Test]
        public void It_Should_Put_A_String_In_Upper_Case()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"test\" | upcase }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : TEST"));

        }
    }
}
