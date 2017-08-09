using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Xunit;

namespace Liquid.NET.Tests.Constants
{
    
    public class LiquidRangeTests
    {
        [Theory]
        [InlineData(3, 7, new []{3,4,5,6,7})]
        [InlineData(7, 3, new[] { 7,6,5,4,3 })]
        [InlineData(0, 0, new [] { 0 })]
        public void It_Should_Generate_Some_Values(int start, int end, int[] expected )
        {
            // Arrange
            var generatorValue = new LiquidRange(LiquidNumeric.Create(start), LiquidNumeric.Create(end));

            // Act
            var result = generatorValue.AsEnumerable();

            // Assert
            Assert.Equal(expected.ToList(), result.Select(x => Convert.ToInt32(x.Value)));

        }

        [Fact]
        public void It_Should_Generate_Some_Values_Descending()
        {
            // Arrange
            var generatorValue = new LiquidRange(LiquidNumeric.Create(5), LiquidNumeric.Create(2));

            // Act
            var result = generatorValue.AsEnumerable();

            // Assert
            Assert.Equal(new List<int> { 5,4,3,2 }, result.Select(x => Convert.ToInt32(x.Value)));

        }

        [Fact]
        public void It_Should_Determine_The_Length_Of_a_Generator()
        {
            // Arrange
            var generatorValue = new LiquidRange(LiquidNumeric.Create(2), LiquidNumeric.Create(5));

            // Assert
            Assert.Equal(4, generatorValue.Length);

        }

        [Fact]
        public void It_Should_Determine_The_Length_Of_a_Descending_Generator()
        {
            // Arrange
            var generatorValue = new LiquidRange(LiquidNumeric.Create(5), LiquidNumeric.Create(2));
            
            // Assert
            Assert.Equal(4, generatorValue.Length);

        }


    }
}
