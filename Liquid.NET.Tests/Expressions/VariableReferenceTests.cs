using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;
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
            templateContext.Define("myvar", new StringValue("HELLO"));
            var symbolTableStack = SymbolTableStackFactory.CreateSymbolTableStack(templateContext);

            // Act
            var result = variableReference.Eval(symbolTableStack, new List<IExpressionConstant>());

            // Assert
            Assert.That(result.Value, Is.EqualTo("HELLO"));


        }
    }
}
