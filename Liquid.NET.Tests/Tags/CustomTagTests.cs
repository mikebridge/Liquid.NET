using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Tags
{
    [TestFixture]
    public class CustomTagTests
    {
        [Test]
        public void It_Should_Parse_A_Custom_Tag()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {% mytag \"hello\" 123 true %}");

            // Act


            // Assert
            Assert.That(result, Is.EqualTo("I heard \"hello\" 123 true"));

        }
    }
}
