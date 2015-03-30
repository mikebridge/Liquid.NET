using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Tests.Helpers;
using NUnit.Framework;

namespace Liquid.NET.Tests.Symbols
{
    [TestFixture]
    public class SymbolTableStackTests
    {
        [Test]
        public void It_Should_Retrieve_A_Defined_Value()
        {
            // Arrange
            const string str = "This is a test.";
            var templateContext = new TemplateContext();
            templateContext.Define("test", new StringValue(str));
            var stack = StackHelper.CreateSymbolTableStack(templateContext);

            // Act
            var result = stack.Reference("test");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(str));

        }

        [Test]
        public void It_Should_Retrieve_An_Undefined_Value_When_Missing()
        {
            // Arrange
            var stack = StackHelper.CreateSymbolTableStack();

            // Act
            var result = stack.Reference("test");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Undefined>());

        }

        [Test]
        public void A_Locally_Scoped_Variable_Should_Override_A_Global_Variable()
        {
            // Arrange
            var stack = StackHelper.CreateSymbolTableStack();
            stack.Define("hello", new StringValue("HELLO"));
            var localScope = new SymbolTable();
            stack.Push(localScope);
            stack.Define("hello", new StringValue("HI"));

            // Act
            var result = stack.Reference("hello");

            // Assert
            Assert.That(result.Value, Is.EqualTo("HI"));

        }


        [Test]
        public void A_Global_Variable_Should_Reemerge_When_Scope_Override_Removed()
        {
            // Arrange
            const string varname = "hello";
            var stack = StackHelper.CreateSymbolTableStack();
            stack.Define(varname, new StringValue("HELLO"));
            var localScope = new SymbolTable();
            stack.Push(localScope);
            stack.Define(varname, new StringValue("HI"));
            stack.Pop();

            // Act
            var result = stack.Reference(varname);

            // Assert
            Assert.That(result.Value, Is.EqualTo("HELLO"));

        }

        [Test]
        public void A_Parent_Scope_Should_Be_Consulted_When_Child_Scope_Has_No_Reference()
        {
            // Arrange
            var stack = StackHelper.CreateSymbolTableStack(); 
            stack.Define("hello", new StringValue("HELLO"));
            var localScope = new SymbolTable();
            stack.Push(localScope);

            // Act
            var result = stack.Reference("hello");

            // Assert
            Assert.That(result.Value, Is.EqualTo("HELLO"));

        }




    }
}
