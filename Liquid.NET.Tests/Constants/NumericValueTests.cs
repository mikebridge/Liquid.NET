using System;
using System.Collections.Generic;
using System.Numerics;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests.Constants
{
    [TestFixture]
    public class NumericValueTests
    {
        [Test]
        public void It_Should_Evaluate_To_Itself()
        {
            // Arrange
            var number = NumericValue.Create(123m);

            // Act
            var result = number.Eval(new TemplateContext(), new List<Option<IExpressionConstant>>()).SuccessValue<NumericValue>();

            // Assert
            Assert.That(result.Value, Is.EqualTo(123m));

        }

        [Test]
        [TestCase(123.4, 123)]
        [TestCase(123.5, 124)]
        [TestCase(124.4, 124)]
        [TestCase(124.5, 125)] // Convert.ToInt32 rounds to nearest even #.
        [TestCase(-1.99, -2)]
        [TestCase(-1.4, -1)]
        [TestCase(0, 0)]
        public void It_Should_Round_A_Decimal_To_Nearest_Int(decimal input, int expected)
        {
            // Arrange
            var number = NumericValue.Create(input);

            // Assert
            Assert.That(number.IntValue, Is.EqualTo(expected));

        }

        [Test]
        public void It_Should_Format_Without_Trailing_Zeroes()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 0.0075| times: 100 }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 0.75"));
        }


        [Test]
        public void It_Should_Cast_A_Non_Numeric_String_To_Zero()
        {
            // Act
            NumericValue number = NumericValue.Parse("z").SuccessValue<NumericValue>();

            // Assert
            Assert.That(number.IntValue, Is.EqualTo(0));

        }

        [Test]        
        public void It_Should_Remember_If_It_Is_An_Int_Or_Double()
        {

            // Assert
            Assert.That(NumericValue.Create(123.0m).IsInt, Is.False);

            // Assert
            Assert.That( NumericValue.Create(123).IsInt, Is.True);
        }

        [Test]
        [TestCase("123", true)]
        [TestCase("123.0", false)]
        [TestCase("123.1", false)]
        public void It_Should_Remember_If_It_Is_An_Int_Or_Double_When_Parsed(String input, bool isInt)
        {

            // Assert
            Assert.That(NumericValue.Parse(input).SuccessValue<NumericValue>().IsInt, Is.EqualTo(isInt));

        }

        [Test]
        public void It_Should_Cast_An_Int_To_Decimal()
        {
            Assert.That(NumericValue.Create(12345).DecimalValue, Is.EqualTo(12345m));
        }

        [Test]
        public void It_Should_Cast_A_Long_To_Decimal()
        {
            Assert.That(NumericValue.Create(long.MaxValue).DecimalValue, Is.EqualTo((decimal) long.MaxValue));
        }

        [Test]
        public void It_Should_Cast_A_BigInt_To_Decimal()
        {
            Assert.That(NumericValue.Create(new BigInteger(12345678901234567890)).DecimalValue, Is.EqualTo(12345678901234567890m));            
        }

        [Test]
        public void It_Should_Cast_An_Int_To_An_Int()
        {
            Assert.That(NumericValue.Create(12345).IntValue, Is.EqualTo(12345));
        }

        [Test]
        public void It_Should_Cast_A_Decimal_To_An_Int()
        {
            Assert.That(NumericValue.Create(12345.6m).IntValue, Is.EqualTo(12346));
        }

        [Test]
        public void It_Should_Cast_A_Long_To_An_Int()
        {
            Assert.That(NumericValue.Create(12345L).IntValue, Is.EqualTo(12345));
        }

        [Test]
        public void It_Should_Cast_A_BigInt_To_Int()
        {
            Assert.That(NumericValue.Create(new BigInteger(123456789)).IntValue, Is.EqualTo(123456789));
        }


        [Test]
        public void It_Should_Cast_An_Int_To_A_BigInt()
        {
            Assert.That(NumericValue.Create(12345).BigIntValue, Is.EqualTo(new BigInteger(12345)));
        }

        [Test]
        public void It_Should_Cast_A_Decimal_To_An_BigInt()
        {
            Assert.That(NumericValue.Create(12345.6m).BigIntValue, Is.EqualTo(new BigInteger(12346)));
        }

        [Test]
        public void It_Should_Cast_A_Long_To_A_BigInteger()
        {
            Assert.That(NumericValue.Create(12345L).BigIntValue, Is.EqualTo(new BigInteger(12345)));
        }

        [Test]
        public void It_Should_Cast_A_BigInt_To_BigintInt()
        {
            Assert.That(NumericValue.Create(new BigInteger(123456789)).BigIntValue, Is.EqualTo(new BigInteger(123456789)));
        }

        [Test]
        public void An_Bigint_Should_Equal_A_Decimal()
        {
            Assert.That(NumericValue.Create(new BigInteger(123)), Is.EqualTo(NumericValue.Create(123m)));
        }

        [Test]
        public void An_Int_Should_Equal_A_Decimal()
        {
            Assert.That(NumericValue.Create(123), Is.EqualTo(NumericValue.Create(123m)));
        }

        [Test]
        public void An_Int_Should_Equal_A_BigInt()
        {
            Assert.That(NumericValue.Create(new BigInteger(123)), Is.EqualTo(NumericValue.Create(123)));
        }


    }
}
