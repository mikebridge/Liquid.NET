using System.Numerics;
using Liquid.NET.Constants;
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
            templateContext.DefineLocalVariable(varname, new StringValue("HELLO"));

            // Act
            var result = templateContext.SymbolTableStack.Reference(varname);

            // Assert
            Assert.That(result.SuccessValue<StringValue>().StringVal, Is.EqualTo("HELLO"));

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



    }
}
