using System.Collections.Generic;
using System.Globalization;
using System.Security.Policy;
using Liquid.NET.Constants;
using Liquid.NET.Utils;
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
            var valueAt = arrayValue.ValueAt(0);
            Assert.That(valueAt.Value, Is.EqualTo(objlist[0].Value));
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
            var ctx = new TemplateContext().WithAllFilters();
            ctx.DefineLocalVariable("myarray", arrayValue);
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ myarray.size }}", ctx);

            // Assert
          
            Assert.That(result, Is.EqualTo("Result : 4" ));
        }

        [Test]
        public void It_Should_Save_Meta_Data()
        {
            // Arrange

            var expected = "Hello";
            ArrayValue arrayValue = new ArrayValue(new List<IExpressionConstant>());

            // Act
            arrayValue.MetaData["test"] = expected;

            // Assert
            Assert.That(arrayValue.MetaData.ContainsKey("test"));
            Assert.That(arrayValue.MetaData["test"], Is.EqualTo(expected));
            
        }


    }
}
