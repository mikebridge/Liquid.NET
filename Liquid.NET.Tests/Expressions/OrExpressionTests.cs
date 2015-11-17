using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests.Expressions
{
    [TestFixture]
    public class OrExpressionTests
    {
        [Test]
        [TestCase(true, true, true)]
        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(false, false, false)]
        public void It_Should_OR_Two_Arguments(bool expected, bool expr1, bool expr2)
        {
            // Arrange

            var expr = new OrExpression();

            // Act
            Assert.Fail("FIx this");
//            var result = expr.Accept(new TemplateContext(), new List<Option<ILiquidValue>>()
//            {
//                new LiquidBoolean(expr1),
//                new LiquidBoolean(expr2)
//            }).SuccessValue<LiquidBoolean>().BoolValue;
//
//            // Assert
//            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
