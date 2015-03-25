using System;
using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Constants
{
    [TestFixture]
    public class ExpressionConstantTests
    {

        [Test]
        public void It_Should_Chain_A_Function()
        {
            // Arrange
            var var1 = new NumericValue(123);
            var var2 = new NumericValue(100);

            // Act
            //var result = var1.Bind(x => Add(x, var2));
            var result = var1.Bind(x => new NumericValue((decimal)var2.Value + (decimal) x.Value));
            //var result= fn(var2);
            // Assert
            Assert.That(result.Value, Is.EqualTo(223m));

        }

        [Test]
        public void It_Should_Pass_An_Error_Back()
        {
            // Arrange
            var var1 = new NumericValue(123);
            var var2 = new NumericValue(100);

            // Act
            var result = var1.Bind(x => new NumericValue((decimal)var2.Value + (decimal) x.Value));
            var result2 = result.Bind(x =>
            {
                var y = new NumericValue(0) { ErrorMessage = "An error has occurred" };
                return y;
            });
            //var result= fn(var2);
            // Assert
            Console.WriteLine(result2);
            Assert.That(result2.HasError, Is.True);

        }

          private Func<NumericValue, NumericValue, NumericValue> Add = (x, y) => new NumericValue((decimal)x.Value + (Decimal)y.Value);


    }
}
