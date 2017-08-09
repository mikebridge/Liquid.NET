using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests.Expressions
{
    
    public class AndExpressionTests
    {
        [Theory]
        [InlineData(true, true, true)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        [InlineData(false, false, false)]
        public void It_Should_AND_Two_Arguments(bool expected, bool expr1, bool expr2)
        {
            // Arrange

            var expr = new AndExpression();

            // Act
            var result = expr.Eval(new TemplateContext(), new List<Option<ILiquidValue>>
            {
                new LiquidBoolean(expr1),
                new LiquidBoolean(expr2)
            }).SuccessValue<LiquidBoolean>().BoolValue;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        //[ExpectedException(typeof(Exception))]
        public void It_Should_Not_Accept_Three_Arguments()
        {
            // Arrange

            var expr = new AndExpression();

            // Act
            // ReSharper disable once UnusedVariable
            Assert.Throws<Exception>(() => expr.Eval(new TemplateContext(), new List<Option<ILiquidValue>>
            {
                new LiquidBoolean(true),
                new LiquidBoolean(false),
                new LiquidBoolean(false)
            }));
        }

    }
}
