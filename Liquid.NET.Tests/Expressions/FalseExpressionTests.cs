using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests.Expressions
{
    [TestFixture]
    public class FalseExpressionTests
    {
        [Test]
        public void It_Should_Return_False()
        {
            var expr = new FalseExpression();

            Assert.Fail("FIx this");
//            var result = expr.Accept(new TemplateContext(), new List<Option<ILiquidValue>>());
//            Assert.That(result.IsSuccess);
//            Assert.That(result.SuccessValue<LiquidBoolean>().BoolValue, Is.False);
        }

    }
}
