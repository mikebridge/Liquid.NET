using System.Collections.Generic;

using Liquid.NET.Constants;
using Liquid.NET.Filters.Strings;
using Liquid.NET.Symbols;

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
            var str = new StringValue("FOO");

            // Act
            symbolTable.DefineLocalVariable("foo", str);

            // Assert
            Assert.That(symbolTable.ReferenceLocalVariable("foo").SuccessValue<StringValue>(), Is.EqualTo(str));            

        }

        [Test]
        public void It_Should_Save_A_Filter_Reference()
        {
            // Arrange
            var symbolTable = new SymbolTable();
            //var str = new StringValue("FOO");


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
            var arrayValue = CreateArrayValue();
            var symbolTable = new SymbolTable();
            symbolTable.DefineLocalVariable(varname, arrayValue);

            // Act
            var arr = symbolTable.ReferenceLocalVariable(varname).SuccessValue<ArrayValue>();

            // Assert
            Assert.That(arr, Is.TypeOf<ArrayValue>());
        }

 

        private static ArrayValue CreateArrayValue()
        {
            IList<IExpressionConstant> objlist = new List<IExpressionConstant>
            {
                new StringValue("a string"),
                new NumericValue(123),
                new NumericValue(456m),
                new BooleanValue(false)
            };
            return new ArrayValue(objlist);
        }
    }
}
