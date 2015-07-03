using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class ReplaceFirstFilterTests
    {

        [Test]
        public void It_Should_Replace_First_Text_In_A_String()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"Hello, world. Goodbye, world.\" | replace_first : \"world\", 'mars' }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : Hello, mars. Goodbye, world."));
        }

        [Test]
        public void It_Should_Replace_A_Number_With_A_String()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 123456789123456789 | replace_first : '456' , 'x'}}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 123x789123456789.0"));
        }



    }
}
