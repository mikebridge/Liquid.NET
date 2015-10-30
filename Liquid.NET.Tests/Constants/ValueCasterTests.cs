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
            var num = NumericValue.Create(123.45m);

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
            var num = NumericValue.Create(123.45m);

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
            var num = NumericValue.Create(123.45m);

            // Act
            MethodInfo method = typeof(ValueCaster).GetMethod("Cast");
            MethodInfo generic = method.MakeGenericMethod(typeof(NumericValue), typeof(StringValue));
            var castResult = (LiquidExpressionResult) generic.Invoke(null, new object[] {num});
            var stringLiteral = castResult.SuccessValue<StringValue>();

            // Assert
            Assert.That(stringLiteral.Value, Is.EqualTo("123.45"));

        }

//        [Test]
//        public void It_Should_Format_An_Array_Like_Json()
//        {
//            // Arrange
//            var num = new ArrayValue(new List<IExpressionConstant>{new NumericValue(123.4m), new NumericValue(5)});
//
//            // Act
//            var stringliteral = ValueCaster.Cast<ArrayValue, StringValue>(num)
//                .SuccessValue<StringValue>()
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
            var num = new ArrayValue(new List<IExpressionConstant> { new StringValue("a"), new StringValue("b"), new StringValue("c") });

            // Act
            var result1 = ValueCaster.Cast<ArrayValue, StringValue>(num);
            var stringliteral = result1.SuccessValue<StringValue>()
                .StringVal;

            // Assert
            Assert.That(stringliteral, Is.EqualTo("abc"));

        }

        [Test]
        public void It_Should_Return_The_Same_Object_If_Src_And_Dest_Are_Arrays()
        {
            // Arrange
            var original = new ArrayValue(new List<IExpressionConstant>{NumericValue.Create(123.4m), NumericValue.Create(5)});

            // Act
            var result = ValueCaster.Cast<ArrayValue, ArrayValue>(original).SuccessValue<ArrayValue>();

            // Assert
            Assert.That(result, Is.EqualTo(original));
        }

        [Test]
        public void It_Should_Return_The_Same_Object_If_Src_And_Dest_Are_Strings()
        {
            // Arrange
            var original = new StringValue("Test");

            // Act
            var result = ValueCaster.Cast<StringValue, StringValue>(original).SuccessValue<StringValue>();

            // Assert
            Assert.That(result, Is.EqualTo(original));
        }

        [Test]
        public void It_Should_Return_The_Same_Object_If_Dest_Is_An_ExpressionConstant()
        {
            // Arrange
            var original = new ArrayValue(new List<IExpressionConstant> { NumericValue.Create(123.4m), NumericValue.Create(5) });

            // Act
            var result = ValueCaster.Cast<ArrayValue, ExpressionConstant>(original).SuccessValue<ArrayValue>();

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

//        [Test]
//        public void It_Should_Format_A_Number_Without_Extra_Zeroes()
//        {
//            // Arrange
//            var num = new NumericValue(123.0000m);
//
//            // Act
//            var stringliteral = ValueCaster.RenderAsString((IExpressionConstant) num);
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
        public void It_Should_Convert_Null_To_Numeric_Zero()
        {

            // Act
            var result = ValueCaster.ConvertFromNull<NumericValue>();
            var numericResult = result.SuccessValue<NumericValue>();

            // Assert
            Assert.That(numericResult.IsInt, Is.True);
            Assert.That(numericResult.IntValue, Is.EqualTo(0));

        }

        [Test]
        public void It_Should_Convert_Null_To_EmptyString()
        {

            // Act
            var result = ValueCaster.ConvertFromNull<StringValue>();
            var stringResult = result.SuccessValue<StringValue>();

            // Assert
            Assert.That(stringResult.StringVal, Is.EqualTo(""));

        }


//        [Test]
//        public void It_Should_Cast_A_String_To_An_Array_Of_Strings()
//        {
//            // Arrange
//            var str = new StringValue("Hello");
//
//            // Act
//            var arrayResult = ValueCaster.Cast<StringValue, ArrayValue>(str);
//            Assert.That(arrayResult.IsError, Is.False);
//
//            // Assert
//            var arrValue = arrayResult.SuccessValue<ArrayValue>().ArrValue;
//            Assert.That(arrValue.Count, Is.EqualTo(5));
//            Assert.That(String.Join(",", arrValue.Select(x =>  ((StringValue) x.Value).StringVal)), Is.EqualTo("H,e,l,l,o"));
//
//        }

        [Test]
        public void It_Should_Cast_A_String_To_An_Array_Of_One_String()
        {
            // Arrange
            var str = new StringValue("Hello");

            // Act
            var arrayResult = ValueCaster.Cast<StringValue, ArrayValue>(str);
            Assert.That(arrayResult.IsError, Is.False);

            // Assert
            var arrValue = arrayResult.SuccessValue<ArrayValue>().ArrValue;
            Assert.That(arrValue.Count, Is.EqualTo(1));
            Assert.That(arrValue[0].Value, Is.EqualTo("Hello"));

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


        [Test]
        public void It_Should_Not_Quote_Numerics_In_Json_Dict()
        {
            // Arrange
            var dictValue = new DictionaryValue(
                new Dictionary<string, IExpressionConstant>
                {
                    {"one", NumericValue.Create(1)},
                    {"two", NumericValue.Create(2L)},
                    {"three", NumericValue.Create(3m)},
                    {"four", NumericValue.Create(new BigInteger(4))}

                });

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
            var subDictValue = new DictionaryValue(
                new Dictionary<string, IExpressionConstant>
                {
                    {"abc", new StringValue("def")}
                });
            var dictValue = new DictionaryValue(
                new Dictionary<string, IExpressionConstant>
                {
                    {"one", NumericValue.Create(1)},
                    {"two", subDictValue},
                });

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
            var subDictValue = new DictionaryValue(
                new Dictionary<string, IExpressionConstant>
                {
                    {"abc", new StringValue("def")},
                    {"ghi", null}
                });
            var dictValue = new DictionaryValue(
                new Dictionary<string, IExpressionConstant>
                {
                    {"one", null},
                    {"two", subDictValue},
                });

            // Act
            ITemplateContext ctx = new TemplateContext().DefineLocalVariable("dict1", dictValue);

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ dict1 }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : { \"one\" : null, \"two\" : { \"abc\" : \"def\", \"ghi\" : null } }"));

        }

        [Test]
        public void It_Should_Cast_Null_To_None()
        {
            // Arrange
            var result = ValueCaster.Cast<StringValue, NumericValue>(null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.SuccessResult.HasValue, Is.False);
        }

        [Test]
        public void It_Should_ConvertFromNull()
        {
            // Arrange
            var result = ValueCaster.ConvertFromNull<ArrayValue>();
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.SuccessResult.HasValue, Is.False);
        }

        [Test]
        public void It_Should_Convert_Num()
        {
            // Arrange
            var result = ValueCaster.Cast<NumericValue, StringValue>(NumericValue.Create(1));
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.SuccessResult.HasValue, Is.True);
        }

        [Test]
        public void It_Should_Convert_An_Array()
        {
            // Arrange
            var result = ValueCaster.Cast<ArrayValue, StringValue>(new ArrayValue(new List<IExpressionConstant>{new StringValue("test")}));
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.SuccessResult.HasValue, Is.True);
            Assert.That(result.SuccessValue<StringValue>().StringVal, Is.EqualTo("test"));
        }

        [Test]
        public void It_Should_Convert_A_Date()
        {
            // Arrange
            var result = ValueCaster.Cast<DateValue, StringValue>(new DateValue(DateTime.UtcNow));
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.SuccessResult.HasValue, Is.True);
        }

        [Test]
        public void It_Should_Cast_Numeric_To_Numeric()
        {
            // Arrange
            var result = ValueCaster.Cast<NumericValue, NumericValue>(NumericValue.Create(1));
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.SuccessResult.HasValue, Is.True);
        }

        [Test]
        public void It_Should_Not_Cast_Dict_To_Numeric()
        {
            // Arrange
            var result = ValueCaster.Cast<DictionaryValue, NumericValue>(new DictionaryValue(new Dictionary<string, Option<IExpressionConstant>>()));
            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public void It_Should_Not_Cast_Array_To_Numeric()
        {
            // Arrange
            var result = ValueCaster.Cast<ArrayValue, NumericValue>(new ArrayValue(new List<IExpressionConstant> { new StringValue("test") }));
            Assert.That(result.IsSuccess, Is.False);
        }


    }
}
