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
                NumericValue.Create(123), 
                NumericValue.Create(456m),
                new BooleanValue(false)
            };
            ArrayValue arrayValue = new ArrayValue(objlist);
            var filter = new PositionFilter(NumericValue.Create(0));
           
            // Act
            var result = filter.Apply(new TemplateContext(), arrayValue).SuccessValue<StringValue>();

            // Assert
            Assert.That(result, Is.EqualTo(objlist[0]));
        }
    }
}
