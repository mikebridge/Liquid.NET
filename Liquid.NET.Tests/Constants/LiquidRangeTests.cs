using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Constants
{
    [TestFixture]
    public class LiquidRangeTests
    {
        [Test]
        [TestCase(3, 7, new []{3,4,5,6,7})]
        [TestCase(7, 3, new[] { 7,6,5,4,3 })]
        [TestCase(0, 0, new [] { 0 })]
        public void It_Should_Generate_Some_Values(int start, int end, int[] expected )
        {
            // Arrange
            var generatorValue = new LiquidRange(LiquidNumeric.Create(start), LiquidNumeric.Create(end));

            // Act
            var result = generatorValue.AsEnumerable();

            // Assert
            Assert.That(result.Select(x => x.Value), Is.EqualTo(expected.ToList()));

        }

        [Test]
        public void It_Should_Generate_Some_Values_Descending()
        {
            // Arrange
            var generatorValue = new LiquidRange(LiquidNumeric.Create(5), LiquidNumeric.Create(2));

            // Act
            var result = generatorValue.AsEnumerable();

            // Assert
            Assert.That(result.Select(x => x.Value), Is.EqualTo(new List<int> { 5,4,3,2 }));

        }

        [Test]
        public void It_Should_Determine_The_Length_Of_a_Generator()
        {
            // Arrange
            var generatorValue = new LiquidRange(LiquidNumeric.Create(2), LiquidNumeric.Create(5));

            // Assert
            Assert.That(generatorValue.Length, Is.EqualTo(4));

        }

        [Test]
        public void It_Should_Determine_The_Length_Of_a_Descending_Generator()
        {
            // Arrange
            var generatorValue = new LiquidRange(LiquidNumeric.Create(5), LiquidNumeric.Create(2));
            
            // Assert
            Assert.That(generatorValue.Length, Is.EqualTo(4));

        }


    }
}
