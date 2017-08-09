using System;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Tests.Helpers;
using Xunit;

namespace Liquid.NET.Tests.Symbols
{
    
    public class SymbolTableStackTests
    {
        [Fact]
        public void It_Should_Retrieve_A_Defined_Value()
        {
            // Arrange
            const string str = "This is a test.";
            var templateContext = new TemplateContext();
            templateContext.DefineLocalVariable("test", LiquidString.Create(str));
            var stack = StackHelper.CreateSymbolTableStack(templateContext);

            // Act
            var result = stack.Reference("test");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(str, result.SuccessValue<LiquidString>().StringVal);

        }

        [Fact]
        public void It_Should_Retrieve_A_Defined_Local_Registry_Value()
        {
            // Arrange
            const string str = "This is a test.";
            var templateContext = new TemplateContext();
            templateContext.DefineLocalRegistryVariable("test", str);
            var stack = StackHelper.CreateSymbolTableStack(templateContext);

            // Act
            var result = stack.ReferenceLocalRegistryVariable("test");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(str, result);
            
        }


        [Fact]
        public void It_Should_Retrieve_An_Error_When_Missing()
        {
            // Arrange
            var stack = StackHelper.CreateSymbolTableStack();

            // Act
            var result = stack.Reference("test");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsError);
            //Assert.That(result, Is.TypeOf<Undefined>());

        }

        [Fact]
        public void A_Locally_Scoped_Variable_Should_Override_A_Global_Variable()
        {
            // Arrange
            var stack = StackHelper.CreateSymbolTableStack();
            stack.Define("hello", LiquidString.Create("HELLO"));
            var localScope = new SymbolTable();
            stack.Push(localScope);
            stack.Define("hello", LiquidString.Create("HI"));

            // Act
            var result = stack.Reference("hello");

            // Assert
            Assert.Equal("HI", result.SuccessValue<LiquidString>().StringVal);

        }


        [Fact]
        public void A_Global_Variable_Should_Reemerge_When_Scope_Override_Removed()
        {
            // Arrange
            const string varname = "hello";
            var stack = StackHelper.CreateSymbolTableStack();
            stack.Define(varname, LiquidString.Create("HELLO"));
            var localScope = new SymbolTable();
            stack.Push(localScope);
            stack.Define(varname, LiquidString.Create("HI"));
            stack.Pop();

            // Act
            var result = stack.Reference(varname);

            // Assert
            Assert.Equal("HELLO", result.SuccessValue<LiquidString>().StringVal);

        }

        [Fact]
        public void A_Parent_Scope_Should_Be_Consulted_When_Child_Scope_Has_No_Reference()
        {
            // Arrange
            var stack = StackHelper.CreateSymbolTableStack(); 
            stack.Define("hello", LiquidString.Create("HELLO"));
            var localScope = new SymbolTable();
            stack.Push(localScope);

            // Act
            var result = stack.Reference("hello");

            // Assert
            Assert.Equal("HELLO", result.SuccessValue<LiquidString>().StringVal);

        }


        [Fact]
        public void It_Should_Find_A_Variable_On_The_Top_Of_The_Stack()
        {
            // Arrange
            var stack = StackHelper.CreateSymbolTableStack();
            stack.Define("hello", LiquidString.Create("HELLO"));

            bool found = false;
            
            // Act
            stack.FindVariable("hello", (st, v) => found = true, () => { throw new Exception("This shouldn't happen"); });

            // Assert
            Assert.True(found);

        }

        [Fact]
        public void It_Should_Find_A_Variable_Nested()
        {
            // Arrange
            var stack = StackHelper.CreateSymbolTableStack();
            stack.Define("hello", LiquidString.Create("HELLO"));
            var localScope = new SymbolTable();
            stack.Push(localScope);
            bool found = false;

            // Act
            stack.FindVariable("hello", (st, v) => found = true, () => { throw new Exception("This shouldn't happen"); });

            // Assert
            Assert.True(found);

        }



    }
}
