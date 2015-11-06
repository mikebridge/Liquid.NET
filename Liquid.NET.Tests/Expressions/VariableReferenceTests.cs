using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests.Expressions
{
    [TestFixture]
    public class VariableReferenceTests
    {
        [Test]
        public void It_Should_Derefence_A_Variable()
        {
            // Arrange
            var variableReference = new VariableReference("myvar");
            var templateContext = new TemplateContext();
            templateContext.DefineLocalVariable("myvar", new LiquidString("HELLO"));

            // Act
            var result = variableReference.Eval(templateContext, new List<Option<IExpressionConstant>>()).SuccessValue<LiquidString>();

            // Assert
            Assert.That(result.Value, Is.EqualTo("HELLO"));
        }

        [Test]
        public void It_Should_Derefence_A_Variable_Missing_Variable_As_None()
        {
            // Arrange
            var variableReference = new VariableReference("myvar");
            var templateContext = new TemplateContext();

            // Act
            var result = variableReference.Eval(templateContext, new List<Option<IExpressionConstant>>());

            // Assert
            Assert.That(result.SuccessResult.HasValue, Is.False);


        }

    }
}
