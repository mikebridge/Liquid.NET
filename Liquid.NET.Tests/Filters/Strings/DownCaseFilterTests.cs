using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]    
    public class DownCaseFilterTests
    {
        [Test]
        public void It_Should_Put_A_String_In_Local_Case()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"TeSt\" | downcase }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : test"));

        }
    }
}
