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
    public class PositionFilterTests
    {
        [Test]
        public void It_Should_Return_An_Element_At_Position_0()
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
            var filter = new PositionFilter(new NumericValue(0));
           
            // Act
            var result = filter.Apply(arrayValue);

            // Assert
            Assert.That(result, Is.EqualTo(objlist[0]));
        }
    }
}
