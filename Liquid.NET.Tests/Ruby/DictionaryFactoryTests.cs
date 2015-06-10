using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Ruby
{
    [TestFixture]
    public class DictionaryFactoryTests
    {
        [Test]
        public void It_Should_Convert_An_Array()
        {
            // Arrange
            String json = "[1,2,3]";
 
            // Act
            var result = ConstantFactory.CreateFromJson(json);

            // Assert
            Console.WriteLine(result);
            Assert.That(result, Is.TypeOf<ArrayValue>());
            var expected = new ArrayValue(new List<IExpressionConstant>
            {
                new NumericValue(1), 
                new NumericValue(2),
                new NumericValue(3)
            });
            Assert.That(((ArrayValue) result).ArrValue, Is.EquivalentTo(expected));
            Assert.Fail("Not Implemented Yet");


        }
    }
}

