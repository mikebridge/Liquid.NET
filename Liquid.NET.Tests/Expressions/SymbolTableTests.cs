using System;
using Liquid.NET.Constants;
using Liquid.NET.Filters.Strings;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests.Expressions
{
    
    public class SymbolTableTests
    {
        [Fact]
        public void It_Should_Save_A_Variable()
        {
            // Arrange
            var symbolTable = new SymbolTable();
            var str = LiquidString.Create("FOO");

            // Act
            symbolTable.DefineLocalVariable("foo", str);

            // Assert
            Assert.Equal(str, symbolTable.ReferenceLocalVariable("foo").SuccessValue<LiquidString>());            

        }

        [Fact]
        public void It_Should_Save_A_Filter_Reference()
        {
            // Arrange
            var symbolTable = new SymbolTable();
            //var str = LiquidString.Create("FOO");


            // Act
            symbolTable.DefineFilter<UpCaseFilter>("upcase");

            // Assert
            Assert.Equal(typeof(UpCaseFilter), symbolTable.ReferenceFilter("upcase"));

        }

        [Fact]
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
            Assert.IsType<LiquidCollection>(arr);
        }

        [Fact]
        public void It_Should_Return_Null_When_Macro_Missing()
        {
            // Arrange
            var symbolTable = new SymbolTable();
    
            // Act
            var macro = symbolTable.ReferenceMacro("test");

            // Assert
            Assert.Null(macro);
        }


        [Fact]
        public void It_Set_Null_Variable_To_None()
        {
            // Arrange
            var symbolTable = new SymbolTable();
            Option<ILiquidValue> val = null;

            // Act            
            symbolTable.DefineLocalVariable("test", val);
            var result = symbolTable.ReferenceLocalVariable("test");

            // Assert
            Assert.False(result.SuccessResult.HasValue);

        }

        [Fact]
        public void It_Should_Return_Null_When_Registry_Null()
        {
            // Arrange
            var symbolTable = new SymbolTable();

            // Act            
            var result = symbolTable.ReferenceLocalRegistryVariable("test");

            // Assert
            Assert.Null(result);

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
