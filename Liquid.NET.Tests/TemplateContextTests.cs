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
            templateContext.Define(varname, new StringValue("HELLO"));

            // Act
            var result = templateContext.Reference(varname);

            // Assert
            Assert.That(result.Value, Is.EqualTo("HELLO"));

        }

    }
}
