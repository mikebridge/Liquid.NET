using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Filters.Array;
using Xunit;

namespace Liquid.NET.Tests.Filters.Array
{
    
    public class LastFilterTests
    {
        [Fact]
        public void It_Should_Return_The_Last_Element()
        {
            // Arrange
            LiquidCollection liquidCollection = new LiquidCollection {
                LiquidString.Create("a string"), 
                LiquidNumeric.Create(123), 
                LiquidNumeric.Create(456m),
                new LiquidBoolean(false)
            };
            var filter = new LastFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), liquidCollection).SuccessValue<LiquidBoolean>();

            // Assert
            Assert.Equal(liquidCollection[liquidCollection.Count - 1], result);

        }

        [Fact]
        public void It_Should_Return_The_Last_Char_Of_A_String()
        {
            // Arrange
            var filter = new LastFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), LiquidString.Create("Hello World")).SuccessValue<LiquidString>();

            // Assert
            Assert.Equal("d", result.StringVal);

        }

        [Fact]
        public void It_Should_Return_An_Error_If_Array_Is_Empty() // TODO: Check if this is the case
        {
            // Arrange
            LiquidCollection liquidCollection = new LiquidCollection();
            var filter = new LastFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), liquidCollection);

            // Assert
            Assert.True(result.IsError);

        }

        [Fact]
        public void It_Should_Return_An_Error_If_String_Is_Empty() // TODO: Check if this is the case
        {
            // Arrange
            var filter = new LastFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), LiquidString.Create(""));

            // Assert
            Assert.True(result.IsError);

        }

        [Fact]
        public void It_Should_Return_An_Error_If_String_Is_Null() // TODO: Check if this is the case
        {
            // Arrange
            var filter = new LastFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), LiquidString.Create(null));

            // Assert
            Assert.True(result.IsError);

        }

//        [Fact]
//        public void It_Should_Return_An_Error_If_Array_Is_Null() // TODO: Check if this is the case
//        {
//            // Arrange
//            var filter = new LastFilter();
//
//            // Act
//            var result = filter.Apply(new LiquidCollection(new Option<ILiquidValue>(null)));
//
//            // Assert
//            Assert.True(result.IsError);
//
//        }
    }
}
