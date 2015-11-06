using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests.Expressions
{
    [TestFixture]
    public class NotExpressionTests
    {
        [Test]
        public void It_Should_Negate_An_Argument()
        {
            // Arrange
            var boolTrue = new LiquidBoolean(true);
            var expr = new NotExpression();

            // Act
            var result = expr.Eval(new TemplateContext(), new List<Option<IExpressionConstant>>{boolTrue}).SuccessValue<LiquidBoolean>();

            // Assert
            Assert.That(result.Value, Is.False);

        }

        [Test]
        public void It_Should_Not_Accept_Two_Arguments()
        {
            // Arrange

            var expr = new NotExpression();

            // Act
            var result = expr.Eval(new TemplateContext(), new List<Option<IExpressionConstant>>
            {
                new LiquidBoolean(true),
                new LiquidBoolean(false),
            });

            Assert.That(result.IsError);
        }

    }
}
