using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using Liquid.NET.Constants;
using Liquid.NET.Utils;
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
            var num = LiquidNumeric.Create(123.45m);

            // Act
            var stringliteral = ValueCaster.Cast<LiquidNumeric, LiquidString>(num)
                .SuccessValue<LiquidString>()
                .StringVal;

            // Assert
            Assert.That(stringliteral, Is.EqualTo("123.45"));

        }

        [Test]
        public void It_Can_Cast_With_Generics()
        {
            // Arrange
            var num = LiquidNumeric.Create(123.45m);

            // Act
            var stringliteral = ValueCaster.Cast<LiquidNumeric, LiquidString>(num)
                .SuccessValue<LiquidString>()
                .StringVal;

            // Assert
            Assert.That(stringliteral, Is.EqualTo("123.45"));

        }

        [Test]
        public void It_Can_Cast_With_Generics_Via_Reflection()
        {
            // Arrange
            var num = LiquidNumeric.Create(123.45m);

            // Act
            MethodInfo method = typeof(ValueCaster).GetMethod("Cast");
            MethodInfo generic = method.MakeGenericMethod(typeof(LiquidNumeric), typeof(LiquidString));
            var castResult = (LiquidExpressionResult) generic.Invoke(null, new object[] {num});
            var stringLiteral = castResult.SuccessValue<LiquidString>();

            // Assert
            Assert.That(stringLiteral.Value, Is.EqualTo("123.45"));

        }

//        [Test]
//        public void It_Should_Format_An_Array_Like_Json()
//        {
//            // Arrange
//            var num = new LiquidCollection(new List<ILiquidValue>{new LiquidNumeric(123.4m), new LiquidNumeric(5)});
//
//            // Act
//            var stringliteral = ValueCaster.Cast<LiquidCollection, LiquidString>(num)
//                .SuccessValue<LiquidString>()
//                .StringVal;
//
//            // Assert
//            Assert.That(stringliteral, Is.EqualTo("[ 123.4, 5 ]"));
//
//        }

        [Test]
        public void It_Should_Format_An_Array_By_Concatenating_Each_Elements_STring_Value()
        {
            // Arrange
            var num = new LiquidCollection{ new LiquidString("a"), new LiquidString("b"), new LiquidString("c") };

            // Act
            var result1 = ValueCaster.Cast<LiquidCollection, LiquidString>(num);
            var stringliteral = result1.SuccessValue<LiquidString>()
                .StringVal;

            // Assert
            Assert.That(stringliteral, Is.EqualTo("abc"));

        }

        [Test]
        public void It_Should_Return_The_Same_Object_If_Src_And_Dest_Are_Arrays()
        {
            // Arrange
            var original = new LiquidCollection{LiquidNumeric.Create(123.4m), LiquidNumeric.Create(5)};

            // Act
            var result = ValueCaster.Cast<LiquidCollection, LiquidCollection>(original).SuccessValue<LiquidCollection>();

            // Assert
            Assert.That(result, Is.EqualTo(original));
        }

        [Test]
        public void It_Should_Return_The_Same_Object_If_Src_And_Dest_Are_Strings()
        {
            // Arrange
            var original = new LiquidString("Test");

            // Act
            var result = ValueCaster.Cast<LiquidString, LiquidString>(original).SuccessValue<LiquidString>();

            // Assert
            Assert.That(result, Is.EqualTo(original));
        }

        [Test]
        public void It_Should_Return_The_Same_Object_If_Dest_Is_An_ExpressionConstant()
        {
            // Arrange
            var original = new LiquidCollection { LiquidNumeric.Create(123.4m), LiquidNumeric.Create(5) };

            // Act
            var result = ValueCaster.Cast<LiquidCollection, LiquidValue>(original).SuccessValue<LiquidCollection>();

            // Assert
            Assert.That(result, Is.EqualTo(original));
        }


        [Test]
        public void It_Should_Handle_Casting_A_Null_Value()
        {
            // Arrange
            var original = new LiquidString(null);

            // Act
            var result = ValueCaster.Cast<LiquidString, LiquidNumeric>(original);

            Assert.That(result.IsSuccess, Is.True);

            // Assert
            // is this what it should do?
            Assert.That(result.SuccessValue<LiquidNumeric>().IntValue, Is.EqualTo(0));

        }

//        [Test]
//        public void It_Should_Format_A_Number_Without_Extra_Zeroes()
//        {
//            // Arrange
//            var num = new LiquidNumeric(123.0000m);
//
//            // Act
//            var stringliteral = ValueCaster.RenderAsString((ILiquidValue) num);
//               
//            // Assert
//            Assert.That(stringliteral, Is.EqualTo("123.00"));
//
//        }


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
        [TestCase(123.0, 123L)]
        [TestCase(123.45, 123L)]
        [TestCase(123.5, 124L)]
        [TestCase(124.5, 125L)]
        [TestCase(-124.5, -125L)]
        [TestCase(-123.5, -124L)]
        [TestCase(0, 0L)]
        public void It_Should_Convert_To_A_Long_Using_Away_From_Zero(decimal input, long expected)
        {

            // Act
            var result = ValueCaster.ConvertToLong(input);

            // Assert
            Assert.That(result, Is.EqualTo(expected));

        }

        [Test]
        public void It_Should_Convert_Null_To_Numeric_Zero()
        {

            // Act
            var result = ValueCaster.ConvertFromNull<LiquidNumeric>();
            var numericResult = result.SuccessValue<LiquidNumeric>();

            // Assert
            Assert.That(numericResult.IsInt, Is.True);
            Assert.That(numericResult.IntValue, Is.EqualTo(0));

        }

        [Test]
        public void It_Should_Convert_Null_To_EmptyString()
        {

            // Act
            var result = ValueCaster.ConvertFromNull<LiquidString>();
            var stringResult = result.SuccessValue<LiquidString>();

            // Assert
            Assert.That(stringResult.StringVal, Is.EqualTo(""));

        }


//        [Test]
//        public void It_Should_Cast_A_String_To_An_Array_Of_Strings()
//        {
//            // Arrange
//            var str = new LiquidString("Hello");
//
//            // Act
//            var arrayResult = ValueCaster.Cast<LiquidString, LiquidCollection>(str);
//            Assert.That(arrayResult.IsError, Is.False);
//
//            // Assert
//            var arrValue = arrayResult.SuccessValue<LiquidCollection>().ArrValue;
//            Assert.That(arrValue.Count, Is.EqualTo(5));
//            Assert.That(String.Join(",", arrValue.Select(x =>  ((LiquidString) x.Value).StringVal)), Is.EqualTo("H,e,l,l,o"));
//
//        }

        [Test]
        public void It_Should_Cast_A_String_To_An_Array_Of_One_String()
        {
            // Arrange
            var str = new LiquidString("Hello");

            // Act
            var arrayResult = ValueCaster.Cast<LiquidString, LiquidCollection>(str);
            Assert.That(arrayResult.IsError, Is.False);

            // Assert
            var arrValue = arrayResult.SuccessValue<LiquidCollection>();
            Assert.That(arrValue.Count, Is.EqualTo(1));
            Assert.That(arrValue[0].Value, Is.EqualTo("Hello"));

        }

        [Test]
        // SEE: https://github.com/Shopify/liquid/wiki/Liquid-for-Designers
        public void It_Should_Cast_KV_Pairs_In_A_Dictionary_To_An_Array_Of_Arrays_with_Two_Elements()
        {
            // Arrange
            var dictValue = new LiquidHash {
                    {"one", new LiquidString("ONE")},
                    {"two", new LiquidString("TWO")},
                    {"three", new LiquidString("THREE")},
                    {"four", new LiquidString("FOUR")}

                };

            // Act
            var result = ValueCaster.Cast<LiquidHash, LiquidCollection>(dictValue).SuccessValue<LiquidCollection>();

            // Assert

            Assert.That(result.Count, Is.EqualTo(4));

        }


        [Test]
        public void It_Should_Not_Quote_Numerics_In_Json_Dict()
        {
            // Arrange
            var dictValue = new LiquidHash
            {
                {"one", LiquidNumeric.Create(1)},
                {"two", LiquidNumeric.Create(2L)},
                {"three", LiquidNumeric.Create(3m)},
                {"four", LiquidNumeric.Create(new BigInteger(4))}
            };

            // Act
            ITemplateContext ctx = new TemplateContext().DefineLocalVariable("dict1", dictValue);

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ dict1 }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : { \"one\" : 1, \"two\" : 2, \"three\" : 3.0, \"four\" : 4 }"));

        }

        [Test]
        public void It_Should_Recursively_Render_Dictionaries_in_Json()
        {
            // Arrange     
            var subDictValue = new LiquidHash
            {
                {"abc", new LiquidString("def")}
            };
            var dictValue = new LiquidHash {
                    {"one", LiquidNumeric.Create(1)},
                    {"two", subDictValue}
            };

            // Act
            ITemplateContext ctx = new TemplateContext().DefineLocalVariable("dict1", dictValue);

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ dict1 }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : { \"one\" : 1, \"two\" : { \"abc\" : \"def\" } }"));

        }

        [Test]
        public void It_Should_Render_A_Null_In_A_Dictionary()
        {
            // Arrange     
            var subDictValue = new LiquidHash
            {
                {"abc", new LiquidString("def")},
                {"ghi", null}
            };
            var dictValue = new LiquidHash
            {
                {"one", null},
                {"two", subDictValue},
            };

            // Act
            ITemplateContext ctx = new TemplateContext().DefineLocalVariable("dict1", dictValue);

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ dict1 }}", ctx);

            // Assert
            Assert.That(result,
                Is.EqualTo("Result : { \"one\" : null, \"two\" : { \"abc\" : \"def\", \"ghi\" : null } }"));

        }

        [Test]
        public void It_Should_Cast_Null_To_None()
        {
            var result = ValueCaster.Cast<LiquidString, LiquidNumeric>(null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.SuccessResult.HasValue, Is.False);
        }

        [Test]
        public void It_Should_ConvertFromNull()
        {
            var result = ValueCaster.ConvertFromNull<LiquidCollection>();
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.SuccessResult.HasValue, Is.False);
        }

        [Test]
        public void It_Should_Convert_Num()
        {
            var result = ValueCaster.Cast<LiquidNumeric, LiquidString>(LiquidNumeric.Create(1));
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.SuccessResult.HasValue, Is.True);
        }

        [Test]
        public void It_Should_Convert_An_Array()
        {
            var result = ValueCaster.Cast<LiquidCollection, LiquidString>(new LiquidCollection{new LiquidString("test")});
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.SuccessResult.HasValue, Is.True);
            Assert.That(result.SuccessValue<LiquidString>().StringVal, Is.EqualTo("test"));
        }

        [Test]
        public void It_Should_Convert_A_Date_To_A_Numeric()
        {
            var date = new DateTime(2015, 10, 29, 10, 11, 12);
            var result = ValueCaster.Cast<LiquidDate, LiquidNumeric>(new LiquidDate(date));
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.SuccessResult.HasValue, Is.True);
            // ReSharper disable once PossibleInvalidOperationException
            Assert.That(result.SuccessValue<LiquidNumeric>().LongValue, Is.EqualTo(date.Ticks));
        }

        [Test]
        public void It_Should_Convert_A_Null_Date_To_0L()
        {
            var result = ValueCaster.Cast<LiquidDate, LiquidNumeric>(new LiquidDate(null));
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.SuccessResult.HasValue, Is.True);
            // ReSharper disable once PossibleInvalidOperationException
            Assert.That(result.SuccessValue<LiquidNumeric>().LongValue, Is.EqualTo(0L));
        }

        [Test]
        public void It_Should_Convert_A_Numeric_To_A_Date()
        {
            var date = new DateTime(2015,10,29,10,11,12);
            var result = ValueCaster.Cast<LiquidNumeric, LiquidDate>(LiquidNumeric.Create(date.Ticks));
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.SuccessResult.HasValue, Is.True);
            // ReSharper disable once PossibleInvalidOperationException
            Assert.That(result.SuccessValue<LiquidDate>().DateTimeValue.Value, Is.EqualTo(date));
        }

        [Test]
        public void It_Should_Cast_Numeric_To_Numeric()
        {
            var result = ValueCaster.Cast<LiquidNumeric, LiquidNumeric>(LiquidNumeric.Create(1));
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.SuccessResult.HasValue, Is.True);
        }

        [Test]
        public void It_Should_Not_Cast_Dict_To_Numeric()
        {
            var result = ValueCaster.Cast<LiquidHash, LiquidNumeric>(new LiquidHash());
            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public void It_Should_Not_Cast_Array_To_Numeric()
        {
            // Arrange
            var result = ValueCaster.Cast<LiquidCollection, LiquidNumeric>(new LiquidCollection { new LiquidString("test") });
            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public void It_Renders_Null_As_Empty()
        {
            // Arrange
            var result = ValueCaster.RenderAsString((ILiquidValue) null);
            Assert.That(result, Is.EqualTo(""));
        }


    }
}
