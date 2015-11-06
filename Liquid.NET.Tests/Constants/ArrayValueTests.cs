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
            ArrayValue arrayValue = new ArrayValue {
                new StringValue("a string"), 
                NumericValue.Create(123), 
                NumericValue.Create(456m),
                new BooleanValue(false)
            };

            // Assert
            var valueAt = arrayValue.ValueAt(0);
            Assert.That(valueAt.Value, Is.EqualTo("a string"));
        }

        [Test]
        public void It_Should_Access_Size_Property_Of_An_Array()
        {
            // Arrange
            ArrayValue arrayValue = new ArrayValue
            {
                new StringValue("a string"),
                NumericValue.Create(123),
                NumericValue.Create(456m),
                new BooleanValue(false)
            };
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
            ArrayValue arrayValue = new ArrayValue();

            // Act
            arrayValue.MetaData["test"] = expected;

            // Assert
            Assert.That(arrayValue.MetaData.ContainsKey("test"));
            Assert.That(arrayValue.MetaData["test"], Is.EqualTo(expected));
            
        }


    }
}
