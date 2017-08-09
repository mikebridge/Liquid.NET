using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests.Expressions
{
    
    public class OrExpressionTests
    {
        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(false, false, false)]
        public void It_Should_OR_Two_Arguments(bool expected, bool expr1, bool expr2)
        {
            // Arrange

            var expr = new OrExpression();

            // Act
            var result = expr.Eval(new TemplateContext(), new List<Option<ILiquidValue>>()
            {
                new LiquidBoolean(expr1),
                new LiquidBoolean(expr2)
            }).SuccessValue<LiquidBoolean>().BoolValue;

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
