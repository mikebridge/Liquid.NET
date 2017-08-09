using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests.Expressions
{
    
    public class VariableReferenceTests
    {
        [Fact]
        public void It_Should_Derefence_A_Variable()
        {
            // Arrange
            var variableReference = new VariableReference("myvar");
            var templateContext = new TemplateContext();
            templateContext.DefineLocalVariable("myvar", LiquidString.Create("HELLO"));

            // Act
            var result = variableReference.Eval(templateContext, new List<Option<ILiquidValue>>()).SuccessValue<LiquidString>();

            // Assert
            Assert.Equal("HELLO", result.Value);
        }

        [Fact]
        public void It_Should_Derefence_A_Variable_Missing_Variable_As_None()
        {
            // Arrange
            var variableReference = new VariableReference("myvar");
            var templateContext = new TemplateContext();

            // Act
            var result = variableReference.Eval(templateContext, new List<Option<ILiquidValue>>());

            // Assert
            Assert.False(result.SuccessResult.HasValue);


        }

    }
}
