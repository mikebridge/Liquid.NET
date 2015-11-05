using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
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
            var result = expr.Eval(new TemplateContext(), new List<Option<IExpressionConstant>>
            {
                new BooleanValue(expr1),
                new BooleanValue(expr2)
            }).SuccessValue<BooleanValue>().BoolValue;

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void It_Should_Not_Accept_Three_Arguments()
        {
            // Arrange

            var expr = new AndExpression();

            // Act
            // ReSharper disable once UnusedVariable
            var result = expr.Eval(new TemplateContext(), new List<Option<IExpressionConstant>>
            {
                new BooleanValue(true),
                new BooleanValue(false),
                new BooleanValue(false)
            });
        }

    }
}
