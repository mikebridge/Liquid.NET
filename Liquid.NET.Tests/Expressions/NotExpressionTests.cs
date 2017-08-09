using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests.Expressions
{
    
    public class NotExpressionTests
    {
        [Fact]
        public void It_Should_Negate_An_Argument()
        {
            // Arrange
            var boolTrue = new LiquidBoolean(true);
            var expr = new NotExpression();

            // Act
            var result = expr.Eval(new TemplateContext(), new List<Option<ILiquidValue>>{boolTrue}).SuccessValue<LiquidBoolean>();

            // Assert
            Assert.False((bool) result.Value);

        }

        [Fact]
        public void It_Should_Not_Accept_Two_Arguments()
        {
            // Arrange

            var expr = new NotExpression();

            // Act
            var result = expr.Eval(new TemplateContext(), new List<Option<ILiquidValue>>
            {
                new LiquidBoolean(true),
                new LiquidBoolean(false),
            });

            Assert.True(result.IsError);
        }

    }
}
