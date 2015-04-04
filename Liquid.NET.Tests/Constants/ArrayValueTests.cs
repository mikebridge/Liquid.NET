using System.Collections.Generic;
using System.Globalization;
using System.Security.Policy;
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

        [Test]
        public void It_Should_Access_Size_Property_Of_An_Array()
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
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ myarray.size }}");

            // Assert
          
            Assert.That(result, Is.EqualTo("Result : 4" ));
        }


    }
}
