using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Constants
{
    [TestFixture]
    public class ValueCasterTests
    {
        [Test]
        public void It_Should_Cast_A_Number_To_A_String()
        {
            // Arrange
            var num = new NumericValue(123.45m);

            // Act
            var stringliteral = ValueCaster.Cast<NumericValue, StringValue>(num)
                .SuccessValue<StringValue>()
                .StringVal;

            // Assert
            Assert.That(stringliteral, Is.EqualTo("123.45"));

        }

        [Test]
        public void It_Can_Cast_With_Generics()
        {
            // Arrange
            var num = new NumericValue(123.45m);

            // Act
            var stringliteral = ValueCaster.Cast<NumericValue, StringValue>(num)
                .SuccessValue<StringValue>()
                .StringVal;

            // Assert
            Assert.That(stringliteral, Is.EqualTo("123.45"));

        }

        [Test]
        public void It_Can_Cast_With_Generics_Via_Reflection()
        {
            // Arrange
            var num = new NumericValue(123.45m);

            // Act
            MethodInfo method = typeof(ValueCaster).GetMethod("Cast");
            MethodInfo generic = method.MakeGenericMethod(typeof(NumericValue), typeof(StringValue));
            var stringLiteral = (StringValue) generic.Invoke(null, new object[] {num});

            // Assert
            Assert.That(stringLiteral.Value, Is.EqualTo("123.45"));

        }

        [Test]
        public void It_Should_Format_An_Array_Like_Json()
        {
            // Arrange
            var num = new ArrayValue(new List<IExpressionConstant>{new NumericValue(123.4m), new NumericValue(5)});

            // Act
            var stringliteral = ValueCaster.Cast<ArrayValue, StringValue>(num)
                .SuccessValue<StringValue>()
                .StringVal;

            // Assert
            Assert.That(stringliteral, Is.EqualTo("[ 123.4, 5 ]"));

        }

        [Test]
        public void It_Should_Return_The_Same_Object_If_Src_And_Dest_Are_The_Same()
        {
            // Arrange
            var original = new ArrayValue(new List<IExpressionConstant>{new NumericValue(123.4m), new NumericValue(5)});

            // Act
            var result = ValueCaster.Cast<ArrayValue, ArrayValue>(original);

            // Assert
            Assert.That(result, Is.EqualTo(original));
        }

        [Test]
        public void It_Should_Return_The_Same_Object_If_Dest_Is_An_ExpressionConstant()
        {
            // Arrange
            var original = new ArrayValue(new List<IExpressionConstant> { new NumericValue(123.4m), new NumericValue(5) });

            // Act
            var result = ValueCaster.Cast<ArrayValue, ExpressionConstant>(original);

            // Assert
            Assert.That(result, Is.EqualTo(original));
        }


        [Test]
        public void It_Should_Handle_Casting_A_Null_Value()
        {
            // Arrange
            var original = new StringValue(null);

            // Act
            var result = ValueCaster.Cast<StringValue, NumericValue>(original);

            Assert.That(result.IsSuccess, Is.True);

            // Assert
            // is this what it should do?
            Assert.That(result.SuccessValue<NumericValue>().IntValue, Is.EqualTo(0));

        }

        [Test]
        [TestCase(123.0, 123)]
        [TestCase(123.45, 123)]
        [TestCase(123.5, 124)]
        [TestCase(124.5, 125)]
        [TestCase(-124.5, -125)]
        [TestCase(-123.5, -124)]
        [TestCase(0, 0)]
        public void It_Should_Convert_To_An_Int_Using_Away_From_Zero(decimal input, int expected)
        {
            
            // Act
            var result = ValueCaster.ConvertToInt(input);

            // Assert
            Assert.That(result, Is.EqualTo(expected));

        }

        [Test]
        public void It_Should_Cast_A_String_To_An_Array_Of_Strings()
        {
            // Arrange
            var str = new StringValue("Hello");

            // Act
            var arrayResult = ValueCaster.Cast<StringValue, ArrayValue>(str);
            Assert.That(arrayResult.IsError, Is.False, arrayResult.ErrorResult.Message);

            // Assert
            var arrValue = arrayResult.SuccessValue<ArrayValue>().ArrValue;
            Assert.That(arrValue.Count, Is.EqualTo(5));
            Assert.That(String.Join(",", arrValue.Select(x =>  ((StringValue) x.Value).StringVal)), Is.EqualTo("H,e,l,l,o"));

        }

        [Test]
        // SEE: https://github.com/Shopify/liquid/wiki/Liquid-for-Designers
        public void It_Should_Cast_KV_Pairs_In_A_Dictionary_To_An_Array_Of_Arrays_with_Two_Elements()
        {
            // Arrange
            var dictValue = new DictionaryValue(
                new Dictionary<string, IExpressionConstant>
                {
                    {"one", new StringValue("ONE")},
                    {"two", new StringValue("TWO")},
                    {"three", new StringValue("THREE")},
                    {"four", new StringValue("FOUR")}

                });

            // Act
            var result = ValueCaster.Cast<DictionaryValue, ArrayValue>(dictValue).SuccessValue<ArrayValue>();

            // Assert

            Assert.That(result.ArrValue.Count, Is.EqualTo(4));

        }


    }
}
