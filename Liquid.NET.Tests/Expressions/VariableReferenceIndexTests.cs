using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;
using Liquid.NET.Tests.Helpers;
using NUnit.Framework;

namespace Liquid.NET.Tests.Expressions
{
    [TestFixture]
    public class VariableReferenceIndexTests
    {
        [Test]
        public void It_Should_Dereference_A_Dictionary_Element()
        {
            // Arrange
            var variableReference = new VariableReference("myvar");
            var variableReferenceIndex = new VariableReferenceIndex(variableReference, "test");
            var templateContext = new TemplateContext();
            IDictionary<String, IExpressionConstant> dict = new Dictionary<string, IExpressionConstant> { { "test", new StringValue("aaa") } };
            templateContext.Define("myvar", new DictionaryValue(dict));
            var symbolTableStack = StackHelper.CreateSymbolTableStack(templateContext);

            // Act

            var result = variableReferenceIndex.Eval(symbolTableStack, new List<IExpressionConstant>());

            // Assert
            Assert.That(result.Value, Is.EqualTo("aaa"));

        }
    }
}
