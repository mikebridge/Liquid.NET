using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests.Expressions
{
    [TestFixture]
    public class AndExpressionTests
    {
        [Test]
        [TestCase(true, true, true)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(false, false, false)]
        public void It_Should_AND_Two_Arguments(bool expected, bool expr1, bool expr2)
        {
            // Arrange

            var expr = new AndExpression();

            // Act
            var result = expr.Eval(SymbolTableStackFactory.CreateSymbolTableStack(new TemplateContext()), new List<Option<IExpressionConstant>>
            {
                new BooleanValue(expr1),
                new BooleanValue(expr2)
            }).SuccessValue<BooleanValue>().BoolValue;

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
