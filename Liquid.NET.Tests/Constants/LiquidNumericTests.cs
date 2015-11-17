using System;
using System.Collections.Generic;
using System.Numerics;
using Liquid.NET.Constants;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests.Constants
{
    [TestFixture]
    public class LiquidNumericTests
    {
        [Test]
        public void It_Should_Evaluate_To_Itself()
        {
            // Arrange
            var number = LiquidNumeric.Create(123m);

            // Act
            //var result = number.Accept(new TemplateContext(), new List<Option<ILiquidValue>>()).SuccessValue<LiquidNumeric>();
            Assert.Fail("FIx this");
            // Assert
            //Assert.That(result.Value, Is.EqualTo(123m));

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
            var number = LiquidNumeric.Create(input);

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
            LiquidNumeric number = LiquidNumeric.Parse("z").SuccessValue<LiquidNumeric>();

            // Assert
            Assert.That(number.IntValue, Is.EqualTo(0));

        }

        [Test]        
        public void It_Should_Remember_If_It_Is_An_Int_Or_Double()
        {

            // Assert
            Assert.That(LiquidNumeric.Create(123.0m).IsInt, Is.False);

            // Assert
            Assert.That( LiquidNumeric.Create(123).IsInt, Is.True);
        }

        [Test]
        [TestCase("123", true)]
        [TestCase("123.0", false)]
        [TestCase("123.1", false)]
        public void It_Should_Remember_If_It_Is_An_Int_Or_Double_When_Parsed(String input, bool isInt)
        {

            // Assert
            Assert.That(LiquidNumeric.Parse(input).SuccessValue<LiquidNumeric>().IsInt, Is.EqualTo(isInt));

        }

        [Test]
        public void It_Should_Cast_An_Int_To_Decimal()
        {
            Assert.That(LiquidNumeric.Create(12345).DecimalValue, Is.EqualTo(12345m));
        }

        [Test]
        public void It_Should_Cast_A_Long_To_Decimal()
        {
            Assert.That(LiquidNumeric.Create(long.MaxValue).DecimalValue, Is.EqualTo((decimal) long.MaxValue));
        }

        [Test]
        public void It_Should_Cast_A_BigInt_To_Decimal()
        {
            Assert.That(LiquidNumeric.Create(new BigInteger(12345678901234567890)).DecimalValue, Is.EqualTo(12345678901234567890m));            
        }

        [Test]
        public void It_Should_Cast_A_BigInt_To_Long()
        {
            Assert.That(LiquidNumeric.Create(new BigInteger(123456789012345678)).LongValue, Is.EqualTo(123456789012345678L));
        }


        [Test]
        public void It_Should_Cast_An_Int_To_An_Int()
        {
            Assert.That(LiquidNumeric.Create(12345).IntValue, Is.EqualTo(12345));
        }

        [Test]
        public void It_Should_Cast_A_Decimal_To_An_Int()
        {
            Assert.That(LiquidNumeric.Create(12345.6m).IntValue, Is.EqualTo(12346));
        }

        [Test]
        public void It_Should_Cast_A_Long_To_An_Int()
        {
            Assert.That(LiquidNumeric.Create(12345L).IntValue, Is.EqualTo(12345));
        }

        [Test]
        public void It_Should_Cast_A_BigInt_To_Int()
        {
            Assert.That(LiquidNumeric.Create(new BigInteger(123456789)).IntValue, Is.EqualTo(123456789));
        }


        [Test]
        public void It_Should_Cast_An_Int_To_A_BigInt()
        {
            Assert.That(LiquidNumeric.Create(12345).BigIntValue, Is.EqualTo(new BigInteger(12345)));
        }

        [Test]
        public void It_Should_Cast_A_Decimal_To_An_BigInt()
        {
            Assert.That(LiquidNumeric.Create(12345.6m).BigIntValue, Is.EqualTo(new BigInteger(12346)));
        }

        [Test]
        public void It_Should_Cast_A_Long_To_A_BigInteger()
        {
            Assert.That(LiquidNumeric.Create(12345L).BigIntValue, Is.EqualTo(new BigInteger(12345)));
        }

        [Test]
        public void It_Should_Cast_A_Decimal_To_A_Long()
        {
            Assert.That(LiquidNumeric.Create(12345.6m).LongValue, Is.EqualTo(12346L));
        }

        [Test]
        public void It_Should_Cast_An_Int_To_A_Long()
        {
            Assert.That(LiquidNumeric.Create(12345).LongValue, Is.EqualTo(12345L));
        }

        [Test]
        public void It_Should_Cast_A_BigInt_To_BigintInt()
        {
            Assert.That(LiquidNumeric.Create(new BigInteger(123456789)).BigIntValue, Is.EqualTo(new BigInteger(123456789)));
        }

        [Test]
        public void An_Bigint_Should_Equal_A_Decimal()
        {
            Assert.That(LiquidNumeric.Create(new BigInteger(123)), Is.EqualTo(LiquidNumeric.Create(123m)));
        }

        [Test]
        public void An_Int_Should_Equal_A_Decimal()
        {
            Assert.That(LiquidNumeric.Create(123), Is.EqualTo(LiquidNumeric.Create(123m)));
        }

        [Test]
        public void An_Int_Should_Equal_A_BigInt()
        {
            Assert.That(LiquidNumeric.Create(new BigInteger(123)), Is.EqualTo(LiquidNumeric.Create(123)));
        }

        [Test]
        public void It_Should_Return_The_BigInt_Value()
        {
            Assert.That(LiquidNumeric.Create(new BigInteger(123)).Value, Is.EqualTo(new BigInteger(123)));
        }

        [Test]
        public void An_Int_Should_Create_A_Hash_Code()
        {
            var num = LiquidNumeric.Create(123);
            Assert.That(num.GetHashCode(), Is.Not.EqualTo(123.GetHashCode()));
        }

        [Test]
        public void A_BigInt_Is_An_Int()
        {
            Assert.That(LiquidNumeric.Create(new BigInteger(123)).IsInt, Is.True);
        }

        [Test]
        public void A_Long_Is_An_Int()
        {
            Assert.That(LiquidNumeric.Create(123L).IsInt, Is.True);
        }


    }
}
