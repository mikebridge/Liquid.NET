using System.Collections.Generic;
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

            ArrayValue arrayValue = new ArrayValue{
                new StringValue("a string"), 
                NumericValue.Create(123), 
                NumericValue.Create(456m),
                new BooleanValue(false)
            };
            var filter = new FirstFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), arrayValue).SuccessValue<StringValue>();

            // Assert
            Assert.That(result, Is.EqualTo(arrayValue[0]));

        }

        [Test]
        public void It_Should_Return_The_First_Char_Of_A_String()
        {
            // Arrange
            var filter = new FirstFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), new StringValue("Hello World")).SuccessValue<StringValue>();

            // Assert
            Assert.That(result.Value, Is.EqualTo("H"));

        }

        [Test]
        public void It_Should_Return_An_Error_If_Array_Is_Empty() // TODO: Check if this is the case
        {
            // Arrange
            ArrayValue arrayValue = new ArrayValue();
            var filter = new FirstFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), arrayValue);

            // Assert
            Assert.That(result.IsError, Is.True);

        }

        [Test]
        public void It_Should_Return_An_Error_If_String_Is_Empty() // TODO: Check if this is the case
        {
            // Arrange
            var filter = new FirstFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), new StringValue(""));

            // Assert
            Assert.That(result.IsError, Is.True);

        }

        [Test]
        public void It_Should_Return_An_Error_If_String_Is_Null() // TODO: Check if this is the case
        {
            // Arrange
            var filter = new FirstFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), new StringValue(null));

            // Assert
            Assert.That(result.IsError, Is.True);

        }

//        [Test]
//        public void It_Should_Return_An_Error_If_Array_Is_Null() // TODO: Check if this is the case
//        {
//            // Arrange
//            var filter = new FirstFilter();
//
//            // Act
//            var result = filter.Apply(new ArrayValue(null));
//
//            // Assert
//            Assert.That(result.IsError, Is.True);
//
//        }


    }
}
