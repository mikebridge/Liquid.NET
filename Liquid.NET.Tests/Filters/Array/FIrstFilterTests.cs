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

            LiquidCollection liquidCollection = new LiquidCollection{
                new LiquidString("a string"), 
                LiquidNumeric.Create(123), 
                LiquidNumeric.Create(456m),
                new LiquidBoolean(false)
            };
            var filter = new FirstFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), liquidCollection).SuccessValue<LiquidString>();

            // Assert
            Assert.That(result, Is.EqualTo(liquidCollection[0]));

        }

        [Test]
        public void It_Should_Return_The_First_Char_Of_A_String()
        {
            // Arrange
            var filter = new FirstFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), new LiquidString("Hello World")).SuccessValue<LiquidString>();

            // Assert
            Assert.That(result.Value, Is.EqualTo("H"));

        }

        [Test]
        public void It_Should_Return_An_Error_If_Array_Is_Empty() // TODO: Check if this is the case
        {
            // Arrange
            LiquidCollection liquidCollection = new LiquidCollection();
            var filter = new FirstFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), liquidCollection);

            // Assert
            Assert.That(result.IsError, Is.True);

        }

        [Test]
        public void It_Should_Return_An_Error_If_String_Is_Empty() // TODO: Check if this is the case
        {
            // Arrange
            var filter = new FirstFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), new LiquidString(""));

            // Assert
            Assert.That(result.IsError, Is.True);

        }

        [Test]
        public void It_Should_Return_An_Error_If_String_Is_Null() // TODO: Check if this is the case
        {
            // Arrange
            var filter = new FirstFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), new LiquidString(null));

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
//            var result = filter.Apply(new LiquidCollection(null));
//
//            // Assert
//            Assert.That(result.IsError, Is.True);
//
//        }


    }
}
