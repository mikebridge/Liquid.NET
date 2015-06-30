using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;
using Liquid.NET.Tests.Helpers;
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
            var result = expr.Eval(StackHelper.CreateSymbolTableStack(), new List<Option<IExpressionConstant>>()
            {
                new BooleanValue(expr1),
                new BooleanValue(expr2)
            }).SuccessValue<BooleanValue>().BoolValue;

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
