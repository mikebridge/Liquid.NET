using System.Collections.Generic;

using Liquid.NET.Constants;

using NUnit.Framework;

namespace Liquid.NET.Tests.Constants
{
    [TestFixture]
    public class ArrayValueTests
    {
        [Test]
        public void It_Should_Dereference_An_Array_Element()
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

            // Assert
            Assert.That(arrayValue.ValueAt(0), Is.EqualTo(objlist[0]));
        }

    }
}
