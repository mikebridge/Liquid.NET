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
    public class EasyValueComparerTests
    {
        [Test]
        public void It_Should_Compare_Two_Equal_Values()
        {
            // Arrange
            var str1 = new StringValue("hello");
            var str2 = new StringValue("hello");
            
            // Act
            Assert.That(new EasyValueComparer().Equals(str1, str2), Is.True);

        }

        [Test]
        public void It_Should_Compare_Two_Unequal_Values()
        {
            // Arrange
            var str1 = new StringValue("hello X");
            var str2 = new StringValue("hello");

            // Act
            Assert.That(new EasyValueComparer().Equals(str1, str2), Is.False);

        }

    }
}
