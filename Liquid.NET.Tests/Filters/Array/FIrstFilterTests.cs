using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Filters.Array;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Array
{

    [TestFixture]
    public class FirstFilterTests
    {
        [Test]
        public void It_Should_Return_The_First_Element()
        {
            // Arrange
            IList<IExpressionConstant> objlist = new List<IExpressionConstant>
            {
                new StringValue("a string"), 
                new NumericValue(123), 
                new NumericValue(456m),
                new BooleanValue(false)
            };
            ArrayValue arrayValue = new ArrayValue(objlist);
            var filter = new FirstFilter();

            // Act
            var result = filter.Apply(arrayValue);

            // Assert
            Assert.That(result, Is.EqualTo(objlist[0]));

        }

        [Test]
        public void It_Should_Return_The_First_Char_Of_A_String()
        {
            // Arrange
            var filter = new FirstFilter();

            // Act
            var result = filter.Apply(new StringValue("Hello World"));

            // Assert
            Assert.That(result.Value, Is.EqualTo("H"));

        }

        [Test]
        public void It_Should_Return_An_Error_If_Array_Is_Empty() // TODO: Check if this is the case
        {
            // Arrange
            ArrayValue arrayValue = new ArrayValue(new List<IExpressionConstant>());
            var filter = new FirstFilter();

            // Act
            var result = filter.Apply(arrayValue);

            // Assert
            Assert.That(result.HasError, Is.True);

        }

        [Test]
        public void It_Should_Return_An_Error_If_String_Is_Empty() // TODO: Check if this is the case
        {
            // Arrange
            var filter = new FirstFilter();

            // Act
            var result = filter.Apply(new StringValue(""));

            // Assert
            Assert.That(result.HasError, Is.True);

        }

        [Test]
        public void It_Should_Return_An_Error_If_String_Is_Null() // TODO: Check if this is the case
        {
            // Arrange
            var filter = new FirstFilter();

            // Act
            var result = filter.Apply(new StringValue(null));

            // Assert
            Assert.That(result.HasError, Is.True);

        }

        [Test]
        public void It_Should_Return_An_Error_If_Array_Is_Null() // TODO: Check if this is the case
        {
            // Arrange
            var filter = new FirstFilter();

            // Act
            var result = filter.Apply(new ArrayValue(null));

            // Assert
            Assert.That(result.HasError, Is.True);

        }


    }
}
