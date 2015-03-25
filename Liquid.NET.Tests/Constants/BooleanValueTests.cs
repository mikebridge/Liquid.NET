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
    public class BooleanValueTests
    {
        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void It_Should_Store_The_Value(bool val)
        {
            // Arrange
            var booleanSymbol = new BooleanValue(val);
            var result = booleanSymbol.Value;

            // Assert
            Assert.That(result, Is.EqualTo(val));

        }
    }
}
