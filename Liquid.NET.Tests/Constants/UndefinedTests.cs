using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Constants
{
    [TestFixture]
    public class UndefinedTests
    {
        [Test]
        public void It_Should_Create_An_Undefined_value()
        {
            // Arrange
            var undefined = new Undefined("origfield");
            var result = undefined.Value;

            // Assert
            Assert.That(result, Is.EqualTo("UNDEFINED: origfield"));

        }
    }
}
