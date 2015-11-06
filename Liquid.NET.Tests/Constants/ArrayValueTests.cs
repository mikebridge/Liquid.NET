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
            var arrayValue = new ArrayValue();

            // Act
            arrayValue.MetaData["test"] = expected;

            // Assert
            Assert.That(arrayValue.MetaData.ContainsKey("test"));
            Assert.That(arrayValue.MetaData["test"], Is.EqualTo(expected));
            
        }

        [Test]
        public void It_Should_Clear_An_Array()
        {
            var arrayValue = new ArrayValue{new StringValue("test")};
            arrayValue.Clear();
            Assert.That(arrayValue.Count, Is.EqualTo(0));
        }


        [Test]
        public void It_Should_Remove_An_Element_From_An_Array()
        {
            var arrayValue = new ArrayValue { new StringValue("test") };
            arrayValue.Remove(new StringValue("test"));
            Assert.That(arrayValue.Count, Is.EqualTo(0));
        }

        [Test]
        public void It_Should_Remove_An_Element_At_An_Index_In_An_Array()
        {
            var arrayValue = new ArrayValue { new StringValue("test") };
            arrayValue.RemoveAt(0);
            Assert.That(arrayValue.Count, Is.EqualTo(0));
        }
        [Test]
        public void It_Should_Find_An_Element_By_Value()
        {
            var arrayValue = new ArrayValue { new StringValue("test") };
            var index = arrayValue.IndexOf(new StringValue("test"));
            Assert.That(index, Is.EqualTo(0));
        }
        [Test]
        public void It_Should_Insert_An_Element()
        {
            var arrayValue = new ArrayValue { new StringValue("test") };
            arrayValue.Insert(0,new StringValue("test 1"));
            Assert.That(arrayValue.IndexOf(new StringValue("test 1")), Is.EqualTo(0));
        }
        [Test]
        public void It_Should_Set_An_Element()
        {
            var arrayValue = new ArrayValue { new StringValue("test") };
            arrayValue[0] = new StringValue("test 1");
            Assert.That(arrayValue.IndexOf(new StringValue("test 1")), Is.EqualTo(0));
        }
    }
}
