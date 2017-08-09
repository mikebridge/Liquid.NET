using Liquid.NET.Constants;
using Liquid.NET.Filters.Array;
using Xunit;

namespace Liquid.NET.Tests.Filters.Array
{

    
    public class FirstFilterTests
    {
        [Fact]
        public void It_Should_Return_The_First_Element()
        {
            // Arrange

            LiquidCollection liquidCollection = new LiquidCollection{
                LiquidString.Create("a string"), 
                LiquidNumeric.Create(123), 
                LiquidNumeric.Create(456m),
                new LiquidBoolean(false)
            };
            var filter = new FirstFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), liquidCollection).SuccessValue<LiquidString>();

            // Assert
            Assert.Equal(liquidCollection[0], result);

        }

        [Fact]
        public void It_Should_Return_The_First_Char_Of_A_String()
        {
            // Arrange
            var filter = new FirstFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), LiquidString.Create("Hello World")).SuccessValue<LiquidString>();

            // Assert
            Assert.Equal("H", result.Value);

        }

        [Fact]
        public void It_Should_Return_An_Error_If_Array_Is_Empty() // TODO: Check if this is the case
        {
            // Arrange
            LiquidCollection liquidCollection = new LiquidCollection();
            var filter = new FirstFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), liquidCollection);

            // Assert
            Assert.True(result.IsError);

        }

        [Fact]
        public void It_Should_Return_An_Error_If_String_Is_Empty() // TODO: Check if this is the case
        {
            // Arrange
            var filter = new FirstFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), LiquidString.Create(""));

            // Assert
            Assert.True(result.IsError);

        }

        [Fact]
        public void It_Should_Return_An_Error_If_String_Is_Null() // TODO: Check if this is the case
        {
            // Arrange
            var filter = new FirstFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), LiquidString.Create(null));

            // Assert
            Assert.True(result.IsError);

        }

//        [Fact]
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
