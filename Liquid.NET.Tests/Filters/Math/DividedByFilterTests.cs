using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Math
{
    [TestFixture]
    public class DividedByFilterTests
    {
        [Test]
        public void It_Should_Divide_A_Number_By_Another_Number()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 9 | divided_by: 3 }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 3"));
        }

        [Test]
        public void It_Should_Return_A_Decimal_When_Dividing_IntLike_Numbers()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 10 | divided_by: 4 }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 2.5"));
        }

        [Test]
        public void It_Should_Cast_Strings()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"10\" | divided_by: \"4\" }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 2.5"));
        }

    }
}
