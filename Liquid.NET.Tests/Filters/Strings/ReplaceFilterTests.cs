using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class ReplaceFilterTests
    {
        [Test]
        public void It_Should_Replace_All_Instances_Of_Text_In_A_String()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"Hello, world. Goodbye, world.\" | replace : \"world\", 'mars' }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : Hello, mars. Goodbye, mars."));
        }

        [Test]
        public void It_Should_Replace_All_Instances_Of_A_Number_With_A_String()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 123456789123456789 | replace : '456' , 'x'}}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 123x789123x789.0"));
        }
    }
}
