using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using NUnit.Framework;

namespace Liquid.NET.Tests.Symbols
{
    [TestFixture]
    public class UndefinedTests
    {
        [Test]
        public void Undefined_Evaluates_To_False()
        {
            // Arrange
            var undefined = new Undefined("zoop");
            
            // Assert
            Assert.That(undefined.IsTrue, Is.False);

        }
    }
}
