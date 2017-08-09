using System;
using System.Collections.Generic;
using System.Numerics;
using Liquid.NET.Constants;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests.Constants
{
    
    public class LiquidNumericTests
    {
        [Fact]
        public void It_Should_Evaluate_To_Itself()
        {
            // Arrange
            var number = LiquidNumeric.Create(123m);

            // Act
            var result = number.Eval(new TemplateContext(), new List<Option<ILiquidValue>>()).SuccessValue<LiquidNumeric>();

            // Assert
            Assert.Equal(123m, result.Value);

        }

        [Theory]
        [InlineData(123.4, 123)]
        [InlineData(123.5, 124)]
        [InlineData(124.4, 124)]
        [InlineData(124.5, 125)] // Convert.ToInt32 rounds to nearest even #.
        [InlineData(-1.99, -2)]
        [InlineData(-1.4, -1)]
        [InlineData(0, 0)]
        public void It_Should_Round_A_Decimal_To_Nearest_Int(decimal input, int expected)
        {
            // Arrange
            var number = LiquidNumeric.Create(input);

            // Assert
            Assert.Equal(expected, number.IntValue);

        }

        [Fact]
        public void It_Should_Format_Without_Trailing_Zeroes()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ 0.0075| times: 100 }}");

            // Assert
            Assert.Equal("Result : 0.75", result);
        }


        [Fact]
        public void It_Should_Cast_A_Non_Numeric_String_To_Zero()
        {
            // Act
            LiquidNumeric number = LiquidNumeric.Parse("z").SuccessValue<LiquidNumeric>();

            // Assert
            Assert.Equal(0, number.IntValue);

        }

        [Fact]        
        public void It_Should_Remember_If_It_Is_An_Int_Or_Double()
        {

            // Assert
            Assert.False(LiquidNumeric.Create(123.0m).IsInt);

            // Assert
            Assert.True( LiquidNumeric.Create(123).IsInt);
        }

        [Theory]
        [InlineData("123", true)]
        [InlineData("123.0", false)]
        [InlineData("123.1", false)]
        public void It_Should_Remember_If_It_Is_An_Int_Or_Double_When_Parsed(String input, bool isInt)
        {

            // Assert
            Assert.Equal(isInt, LiquidNumeric.Parse(input).SuccessValue<LiquidNumeric>().IsInt);

        }

        [Fact]
        public void It_Should_Cast_An_Int_To_Decimal()
        {
            Assert.Equal(12345m, LiquidNumeric.Create(12345).DecimalValue);
        }

        [Fact]
        public void It_Should_Cast_A_Long_To_Decimal()
        {
            Assert.Equal(long.MaxValue, LiquidNumeric.Create(long.MaxValue).DecimalValue);
        }

        [Fact]
        public void It_Should_Cast_A_BigInt_To_Decimal()
        {
            Assert.Equal(12345678901234567890m, LiquidNumeric.Create(new BigInteger(12345678901234567890)).DecimalValue);            
        }

        [Fact]
        public void It_Should_Cast_A_BigInt_To_Long()
        {
            Assert.Equal(123456789012345678L, LiquidNumeric.Create(new BigInteger(123456789012345678)).LongValue);
        }


        [Fact]
        public void It_Should_Cast_An_Int_To_An_Int()
        {
            Assert.Equal(12345, LiquidNumeric.Create(12345).IntValue);
        }

        [Fact]
        public void It_Should_Cast_A_Decimal_To_An_Int()
        {
            Assert.Equal(12346, LiquidNumeric.Create(12345.6m).IntValue);
        }

        [Fact]
        public void It_Should_Cast_A_Long_To_An_Int()
        {
            Assert.Equal(12345, LiquidNumeric.Create(12345L).IntValue);
        }

        [Fact]
        public void It_Should_Cast_A_BigInt_To_Int()
        {
            Assert.Equal(123456789, LiquidNumeric.Create(new BigInteger(123456789)).IntValue);
        }


        [Fact]
        public void It_Should_Cast_An_Int_To_A_BigInt()
        {
            Assert.Equal(new BigInteger(12345), LiquidNumeric.Create(12345).BigIntValue);
        }

        [Fact]
        public void It_Should_Cast_A_Decimal_To_An_BigInt()
        {
            Assert.Equal(new BigInteger(12346), LiquidNumeric.Create(12345.6m).BigIntValue);
        }

        [Fact]
        public void It_Should_Cast_A_Long_To_A_BigInteger()
        {
            Assert.Equal(new BigInteger(12345), LiquidNumeric.Create(12345L).BigIntValue);
        }

        [Fact]
        public void It_Should_Cast_A_Decimal_To_A_Long()
        {
            Assert.Equal(12346L, LiquidNumeric.Create(12345.6m).LongValue);
        }

        [Fact]
        public void It_Should_Cast_An_Int_To_A_Long()
        {
            Assert.Equal(12345L, LiquidNumeric.Create(12345).LongValue);
        }

        [Fact]
        public void It_Should_Cast_A_BigInt_To_BigintInt()
        {
            Assert.Equal(new BigInteger(123456789), LiquidNumeric.Create(new BigInteger(123456789)).BigIntValue);
        }

        [Fact]
        public void An_Bigint_Should_Equal_A_Decimal()
        {
            Assert.Equal(LiquidNumeric.Create(123m), LiquidNumeric.Create(new BigInteger(123)));
        }

        [Fact]
        public void An_Int_Should_Equal_A_Decimal()
        {
            Assert.Equal(LiquidNumeric.Create(123m), LiquidNumeric.Create(123));
        }

        [Fact]
        public void An_Int_Should_Equal_A_BigInt()
        {
            Assert.Equal(LiquidNumeric.Create(123), LiquidNumeric.Create(new BigInteger(123)));
        }

        [Fact]
        public void It_Should_Return_The_BigInt_Value()
        {
            Assert.Equal(new BigInteger(123), LiquidNumeric.Create(new BigInteger(123)).Value);
        }

        [Fact]
        public void An_Int_Should_Create_A_Hash_Code()
        {
            var num = LiquidNumeric.Create(123);
            Assert.NotEqual(123.GetHashCode(), num.GetHashCode());
        }

        [Fact]
        public void A_BigInt_Is_An_Int()
        {
            Assert.True(LiquidNumeric.Create(new BigInteger(123)).IsInt);
        }

        [Fact]
        public void A_Long_Is_An_Int()
        {
            Assert.True(LiquidNumeric.Create(123L).IsInt);
        }


    }
}
