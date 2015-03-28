using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class RStripFilterTests
    {
        [Test]
        public void It_Should_Strip_Whitespace_on_The_Right()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ '   too many spaces           ' | rstrip }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result :    too many spaces"));

        }
    }
}
