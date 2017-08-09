using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests.Expressions
{
    
    public class FalseExpressionTests
    {
        [Fact]
        public void It_Should_Return_False()
        {
            var expr = new FalseExpression();
            var result = expr.Eval(new TemplateContext(), new List<Option<ILiquidValue>>());
            Assert.True(result.IsSuccess);
            Assert.False(result.SuccessValue<LiquidBoolean>().BoolValue);
        }

    }
}
