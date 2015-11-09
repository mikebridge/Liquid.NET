using System;
using Liquid.NET.Constants;
using Liquid.NET.Filters.Strings;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests.Expressions
{
    [TestFixture]
    public class SymbolTableTests
    {
        [Test]
        public void It_Should_Save_A_Variable()
        {
            // Arrange
            var symbolTable = new SymbolTable();
            var str = LiquidString.Create("FOO");

            // Act
            symbolTable.DefineLocalVariable("foo", str);

            // Assert
            Assert.That(symbolTable.ReferenceLocalVariable("foo").SuccessValue<LiquidString>(), Is.EqualTo(str));            

        }

        [Test]
        public void It_Should_Save_A_Filter_Reference()
        {
            // Arrange
            var symbolTable = new SymbolTable();
            //var str = LiquidString.Create("FOO");


            // Act
            symbolTable.DefineFilter<UpCaseFilter>("upcase");

            // Assert
            Assert.That(symbolTable.ReferenceFilter("upcase"), Is.EqualTo(typeof(UpCaseFilter)));

        }

        [Test]
        // TODO: clean this up
        public void It_Should_Find_An_Array()
        {

            // Arrange
            const string varname = "objlist";
            var liquidCollection = CreateArrayValue();
            var symbolTable = new SymbolTable();
            symbolTable.DefineLocalVariable(varname, liquidCollection);

            // Act
            var arr = symbolTable.ReferenceLocalVariable(varname).SuccessValue<LiquidCollection>();

            // Assert
            Assert.That(arr, Is.TypeOf<LiquidCollection>());
        }

        [Test]
        public void It_Should_Return_Null_When_Macro_Missing()
        {
            // Arrange
            var symbolTable = new SymbolTable();
    
            // Act
            var macro = symbolTable.ReferenceMacro("test");

            // Assert
            Assert.That(macro, Is.Null);
        }


        [Test]
        public void It_Set_Null_Variable_To_None()
        {
            // Arrange
            var symbolTable = new SymbolTable();
            Option<ILiquidValue> val = null;

            // Act            
            symbolTable.DefineLocalVariable("test", val);
            var result = symbolTable.ReferenceLocalVariable("test");

            // Assert
            Assert.That(result.SuccessResult.HasValue, Is.False);

        }

        [Test]
        public void It_Should_Return_Null_When_Registry_Null()
        {
            // Arrange
            var symbolTable = new SymbolTable();

            // Act            
            var result = symbolTable.ReferenceLocalRegistryVariable("test");

            // Assert
            Assert.That(result, Is.Null);

        }



        private static LiquidCollection CreateArrayValue()
        {
            return new LiquidCollection{
                LiquidString.Create("a string"),
                LiquidNumeric.Create(123),
                LiquidNumeric.Create(456m),
                new LiquidBoolean(false)
            };
        }
    }
}
