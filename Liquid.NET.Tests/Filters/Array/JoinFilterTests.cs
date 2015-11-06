using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Filters.Array;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Array
{
    [TestFixture]
    public class JoinFilterTests
    {
        [Test]
        public void It_Should_Join_An_Array_With_A_String()
        {
            // Arrange
            LiquidCollection liquidCollection = new LiquidCollection {
                new LiquidString("a string"), 
                LiquidNumeric.Create(123), 
                LiquidNumeric.Create(456m),
                new LiquidBoolean(false)
            };
            var filter = new JoinFilter(new LiquidString(", "));

            // Act
            var result = filter.Apply(new TemplateContext(), liquidCollection).SuccessValue<LiquidString>();
            //Logger.Log(result.ToString());

            // Assert
            Assert.That(result.StringVal, Is.EqualTo("a string, 123, 456.0, false"));
     
        }

        [Test]
        public void It_Should_Join_String_With_A_String()
        {
            // Arrange
                
            var filter = new JoinFilter(new LiquidString(", "));

            // Act
            var result = filter.Apply(new TemplateContext(), new LiquidString("Hello World")).SuccessValue<LiquidString>();

            // Assert
            Assert.That(result.Value, Is.EqualTo("H, e, l, l, o,  , W, o, r, l, d"));

        }

        [Test]        
        public void It_Should_Join_With_Nil()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("{% assign arr = 'a,b,c' | split: ',' %}Result : {{ arr| join }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : abc"));
        }

    }
}
