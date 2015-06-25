using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Filters.Array;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Array
{
    [TestFixture]
    public class JoinFilterTests
    {
        [Test]
        public void It_Should_Join_An_Array_With_A_String()
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
            var filter = new JoinFilter(new StringValue(", "));

            // Act
            var result = filter.Apply(arrayValue).SuccessValue<StringValue>();

            // Assert
            Assert.That(result.StringVal, Is.EqualTo("a string, 123, 456, false"));
     
        }

        [Test]
        public void It_Should_Join_String_With_A_String()
        {
            // Arrange
                
            var filter = new JoinFilter(new StringValue(", "));

            // Act
            var result = filter.Apply(new StringValue("Hello World")).SuccessValue<StringValue>();

            // Assert
            Assert.That(result.Value, Is.EqualTo("H, e, l, l, o,  , W, o, r, l, d"));

        }

        [Test]
        [Ignore]
        public void It_Should_Test_Empty_Conditions()
        {
            // TODO
        }

        [Test]
        [Ignore]
        public void It_Should_Test_Variables()
        {
            // TODO
        }


    }
}
