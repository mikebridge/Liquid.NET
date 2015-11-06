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
            ArrayValue arrayValue = new ArrayValue {
                new StringValue("a string"), 
                NumericValue.Create(123), 
                NumericValue.Create(456m),
                new BooleanValue(false)
            };
            var filter = new JoinFilter(new StringValue(", "));

            // Act
            var result = filter.Apply(new TemplateContext(), arrayValue).SuccessValue<StringValue>();
            //Logger.Log(result.ToString());

            // Assert
            Assert.That(result.StringVal, Is.EqualTo("a string, 123, 456.0, false"));
     
        }

        [Test]
        public void It_Should_Join_String_With_A_String()
        {
            // Arrange
                
            var filter = new JoinFilter(new StringValue(", "));

            // Act
            var result = filter.Apply(new TemplateContext(), new StringValue("Hello World")).SuccessValue<StringValue>();

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
