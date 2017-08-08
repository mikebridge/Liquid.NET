using System;
using System.Collections.Generic;
using System.Numerics;
using Liquid.NET.Constants;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests
{
    [TestFixture]
    public class TemplateContextTests
    {
        [Test]
        public void It_Should_Reference_A_Defined_Name()
        {
            // Arrange
            const string varname = "hello";
            var templateContext = new TemplateContext();            
            templateContext.DefineLocalVariable(varname, LiquidString.Create("HELLO"));

            // Act
            var result = templateContext.SymbolTableStack.Reference(varname);

            // Assert
            Assert.That(result.SuccessValue<LiquidString>().StringVal, Is.EqualTo("HELLO"));

        }

        [Test]
        public void It_Should_Add_A_Register()
        {
            // Arrange
            const string varname = "hello";
            var templateContext = new TemplateContext();
            

            // Act
            var orig = new BigInteger(123);
            templateContext.Registers.Add(varname, orig);
            var val = (BigInteger) templateContext.Registers[varname];

            // Assert
            Assert.That(val, Is.EqualTo(orig));
        }

        [Test]
        public void It_Should_Add_A_Register_Via_The_Context()
        {
            // Arrange
            const string varname = "hello";
            var templateContext = new TemplateContext().WithRegisters(
                new Dictionary<String, Object> {{varname, "TEST"}});
        

            // Act
            var val = (String)templateContext.Registers[varname];

            // Assert
            Assert.That(val, Is.EqualTo("TEST"));
        }

        [Test]
        public void It_Should_Add_A_Local_Variable_Via_The_Context()
        {
            // Arrange
            const string varname = "hello";
            var templateContext = new TemplateContext().WithLocalVariables(
                new Dictionary<String,Option<ILiquidValue>> { { varname, LiquidString.Create("TEST") } });


            // Act
            var val = templateContext.SymbolTableStack.Reference(varname);

            // Assert
            Assert.That(val.SuccessValue<LiquidString>().StringVal, Is.EqualTo("TEST"));
        }


        [Test]
       
        public void It_Should_Implicitly_Cast_A_Null_LocalVariable_To_None()
        {
            // Arrange
            var templateContext = new TemplateContext().DefineLocalVariable("test", null);

            Assert.That(templateContext.SymbolTableStack.Reference("test"), Is.EqualTo(LiquidValue.None));
        }


        [Test]
        //[ExpectedException(typeof(ArgumentNullException))]
        public void It_Should_THrow_Error_When_No_ASTGenerator()
        {
            // Arrange
            Assert.That(() => new TemplateContext().WithASTGenerator(null),
                Throws.TypeOf<ArgumentNullException>());
        }


    }
}
