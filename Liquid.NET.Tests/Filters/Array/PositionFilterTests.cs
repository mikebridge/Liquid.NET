using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Filters.Array;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Array
{
    [TestFixture]
    public class PositionFilterTests
    {
        [Test]
        public void It_Should_Return_An_Element_At_Position_0()
        {
            // Arrange

            LiquidCollection liquidCollection = new LiquidCollection {
                LiquidString.Create("a string"), 
                LiquidNumeric.Create(123), 
                LiquidNumeric.Create(456m),
                new LiquidBoolean(false)
            };
            var filter = new PositionFilter(LiquidNumeric.Create(0));
           
            // Act
            var result = filter.Apply(new TemplateContext(), liquidCollection).SuccessValue<LiquidString>();

            // Assert
            Assert.That(result, Is.EqualTo(liquidCollection[0]));
        }
    }
}
