using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Filters.Strings;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class RemoveFirstFilterTests
    {
     
        [Test]
        public void It_Should_Remove_First_Text_From_A_String()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"Hello, world. Goodbye, world.\" | remove_first : \"world\" }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : Hello, . Goodbye, world."));
        }

        [Test]
        public void It_Should_Remove_A_Number_From_A_Number()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 123456789123456789 | remove_first : 456 }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 123789123456789"));
        }
    }
}
