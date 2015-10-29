using System;
using System.Runtime.Remoting.Messaging;
using Liquid.NET.Constants;
using Liquid.NET.Filters.Math;
using NUnit.Framework;

namespace Liquid.NET.Tests.Constants
{
    [TestFixture]
    public class ExpressionConstantTests
    {

        [Test]
        [TestCase(1, 2, false)]
        [TestCase(2, 2, true)]
        public void NumericValues_Should_Equal(decimal decvar1, decimal decvar2, bool expected)
        {
            // Arrange
            var var1 = NumericValue.Create(decvar1);
            var var2 = NumericValue.Create(decvar2);

            // Assert
            Assert.That(var1.Equals(var2), Is.EqualTo(expected));

        }

        [Test]
        public void NumericValues_Should_Not_Equal_Null()
        {
            // Arrange
            var var1 = NumericValue.Create(1);            

            // Assert
            Assert.That(var1.Equals(null), Is.EqualTo(false));

        }

        [Test]
        public void ToString_Should_Render_As_String()
        {
            // Arrange
            var var1 = NumericValue.Create(1);

            // Assert
            Assert.That(var1.ToString(), Is.EqualTo("1"));

        }


//        [Test]
//        public void It_Should_Chain_A_Function()
//        {
//            // Arrange
//            var var1 = new NumericValue(123);
//            var var2 = new NumericValue(100);
//
//            // Act
//            //var result = var1.Bind(x => Add(x, var2));
//            
//            var result = var1.Bind(x => new NumericValue((decimal)var2.Value + (decimal) x.Value));
//            //var result= fn(var2);
//            // Assert
//            Assert.That(result.Value, Is.EqualTo(223m));
//
//        }

//        [Test]
//        public void It_Should_Pass_An_Error_Back()
//        {
//            // Arrange
//            var var1 = new NumericValue(123);
//            var var2 = new NumericValue(100);
//
//            // Act
//            var result = var1.Bind(x => new NumericValue((decimal)var2.Value + (decimal) x.Value));
//            var result2 = result.Bind(x =>
//            {
//                var y = new NumericValue(0) { ErrorMessage = "An error has occurred" };
//                return y;
//            });
//            //var result= fn(var2);
//            // Assert
//            Logger.Log(result2);
//            Assert.That(result2.HasError, Is.True);
//
//        }

//        [Test]
//        public void It_Should_Return_Error_If_Passed_Unknown()
//        {
//            // Arrange
//            var undefinedNumber = ConstantFactory.CreateNilValueOfType<NumericValue>("Undefined test");
//                  
//            // Act
//            var result = undefinedNumber.Bind(x => _testToString((NumericValue) x));
//
//            // Assert
//            Assert.That(result.IsUndefined, Is.True);
//
//        }

          //private Func<NumericValue, NumericValue, NumericValue> Add = (x, y) => new NumericValue((decimal)x.Value + (Decimal)y.Value);

          //private readonly Func<NumericValue, StringValue> _testToString = num => new StringValue(num.Value.ToString());


    }
}
