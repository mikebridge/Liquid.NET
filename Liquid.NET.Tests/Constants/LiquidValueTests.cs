using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Constants
{
    [TestFixture]
    public class LiquidValueTests
    {

        [Test]
        [TestCase(1, 2, false)]
        [TestCase(2, 2, true)]
        public void NumericValues_Should_Equal(decimal decvar1, decimal decvar2, bool expected)
        {
            // Arrange
            var var1 = LiquidNumeric.Create(decvar1);
            var var2 = LiquidNumeric.Create(decvar2);

            // Assert
            Assert.That(var1.Equals(var2), Is.EqualTo(expected));

        }

        [Test]
        public void NumericValues_Should_Not_Equal_Null()
        {
            // Arrange
            var var1 = LiquidNumeric.Create(1);            

            // Assert
            Assert.That(var1.Equals(null), Is.EqualTo(false));

        }

        [Test]
        public void ToString_Should_Render_As_String()
        {
            // Arrange
            var var1 = LiquidNumeric.Create(1);

            // Assert
            Assert.That(var1.ToString(), Is.EqualTo("1"));

        }


//        [Test]
//        public void It_Should_Chain_A_Function()
//        {
//            // Arrange
//            var var1 = new LiquidNumeric(123);
//            var var2 = new LiquidNumeric(100);
//
//            // Act
//            //var result = var1.Bind(x => Add(x, var2));
//            
//            var result = var1.Bind(x => new LiquidNumeric((decimal)var2.Value + (decimal) x.Value));
//            //var result= fn(var2);
//            // Assert
//            Assert.That(result.Value, Is.EqualTo(223m));
//
//        }

//        [Test]
//        public void It_Should_Pass_An_Error_Back()
//        {
//            // Arrange
//            var var1 = new LiquidNumeric(123);
//            var var2 = new LiquidNumeric(100);
//
//            // Act
//            var result = var1.Bind(x => new LiquidNumeric((decimal)var2.Value + (decimal) x.Value));
//            var result2 = result.Bind(x =>
//            {
//                var y = new LiquidNumeric(0) { ErrorMessage = "An error has occurred" };
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
//            var undefinedNumber = ConstantFactory.CreateNilValueOfType<LiquidNumeric>("Undefined test");
//                  
//            // Act
//            var result = undefinedNumber.Bind(x => _testToString((LiquidNumeric) x));
//
//            // Assert
//            Assert.That(result.IsUndefined, Is.True);
//
//        }

          //private Func<LiquidNumeric, LiquidNumeric, LiquidNumeric> Add = (x, y) => new LiquidNumeric((decimal)x.Value + (Decimal)y.Value);

          //private readonly Func<LiquidNumeric, LiquidString> _testToString = num => new LiquidString(num.Value.ToString());


    }
}
