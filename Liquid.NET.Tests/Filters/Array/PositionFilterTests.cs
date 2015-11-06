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

            ArrayValue arrayValue = new ArrayValue {
                new StringValue("a string"), 
                NumericValue.Create(123), 
                NumericValue.Create(456m),
                new BooleanValue(false)
            };
            var filter = new PositionFilter(NumericValue.Create(0));
           
            // Act
            var result = filter.Apply(new TemplateContext(), arrayValue).SuccessValue<StringValue>();

            // Assert
            Assert.That(result, Is.EqualTo(arrayValue[0]));
        }
    }
}
