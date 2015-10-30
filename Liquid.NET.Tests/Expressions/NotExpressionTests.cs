using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;
using Liquid.NET.Tests.Helpers;
using Liquid.NET.Tests.Tags;
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
            var boolTrue = new BooleanValue(true);
            var expr = new NotExpression();

            // Act
            var result = expr.Eval(new TemplateContext(), new List<Option<IExpressionConstant>>{boolTrue}).SuccessValue<BooleanValue>();

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
                new BooleanValue(true),
                new BooleanValue(false),
            });

            Assert.That(result.IsError);
        }

    }
}
