using System;
using System.Numerics;
using System.Reflection;
using Liquid.NET.Constants;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests.Constants
{
    
    public class ValueCasterTests
    {
        [Fact]
        public void It_Should_Cast_A_Number_To_A_String()
        {
            // Arrange
            var num = LiquidNumeric.Create(123.45m);

            // Act
            var stringliteral = ValueCaster.Cast<LiquidNumeric, LiquidString>(num)
                .SuccessValue<LiquidString>()
                .StringVal;

            // Assert
            Assert.Equal("123.45", stringliteral);

        }

        [Fact]
        public void It_Can_Cast_With_Generics()
        {
            // Arrange
            var num = LiquidNumeric.Create(123.45m);

            // Act
            var stringliteral = ValueCaster.Cast<LiquidNumeric, LiquidString>(num)
                .SuccessValue<LiquidString>()
                .StringVal;

            // Assert
            Assert.Equal("123.45", stringliteral);

        }

        [Fact]
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
            Assert.Equal("123.45", stringLiteral.Value);

        }

//        [Fact]
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
//            Assert.Equal("[ 123.4, 5 ]", stringliteral);
//
//        }

        [Fact]
        public void It_Should_Format_An_Array_By_Concatenating_Each_Elements_STring_Value()
        {
            // Arrange
            var num = new LiquidCollection{ LiquidString.Create("a"), LiquidString.Create("b"), LiquidString.Create("c") };

            // Act
            var result1 = ValueCaster.Cast<LiquidCollection, LiquidString>(num);
            var stringliteral = result1.SuccessValue<LiquidString>()
                .StringVal;

            // Assert
            Assert.Equal("abc", stringliteral);

        }

        [Fact]
        public void It_Should_Return_The_Same_Object_If_Src_And_Dest_Are_Arrays()
        {
            // Arrange
            var original = new LiquidCollection{LiquidNumeric.Create(123.4m), LiquidNumeric.Create(5)};

            // Act
            var result = ValueCaster.Cast<LiquidCollection, LiquidCollection>(original).SuccessValue<LiquidCollection>();

            // Assert
            Assert.Equal(original, result);
        }

        [Fact]
        public void It_Should_Return_The_Same_Object_If_Src_And_Dest_Are_Strings()
        {
            // Arrange
            var original = LiquidString.Create("Test");

            // Act
            var result = ValueCaster.Cast<LiquidString, LiquidString>(original).SuccessValue<LiquidString>();

            // Assert
            Assert.Equal(original, result);
        }

        [Fact]
        public void It_Should_Return_The_Same_Object_If_Dest_Is_An_ExpressionConstant()
        {
            // Arrange
            var original = new LiquidCollection { LiquidNumeric.Create(123.4m), LiquidNumeric.Create(5) };

            // Act
            var result = ValueCaster.Cast<LiquidCollection, LiquidValue>(original).SuccessValue<LiquidCollection>();

            // Assert
            Assert.Equal(original, result);
        }


        [Fact]
        public void It_Should_Handle_Casting_A_Null_Value()
        {
            // Arrange
            var original = LiquidString.Create(null);

            // Act
            var result = ValueCaster.Cast<LiquidString, LiquidNumeric>(original);

            Assert.True(result.IsSuccess);

            // Assert
            //Assert.Equal(0, result.SuccessValue<LiquidNumeric>().IntValue);
            Assert.True(result.IsSuccess);
            Assert.False(result.SuccessOption<ILiquidValue>().HasValue);

        }

//        [Fact]
//        public void It_Should_Format_A_Number_Without_Extra_Zeroes()
//        {
//            // Arrange
//            var num = new LiquidNumeric(123.0000m);
//
//            // Act
//            var stringliteral = ValueCaster.RenderAsString((ILiquidValue) num);
//               
//            // Assert
//            Assert.Equal("123.00", stringliteral);
//
//        }


        [Theory]
        [InlineData(123.0, 123)]
        [InlineData(123.45, 123)]
        [InlineData(123.5, 124)]
        [InlineData(124.5, 125)]
        [InlineData(-124.5, -125)]
        [InlineData(-123.5, -124)]
        [InlineData(0, 0)]
        public void It_Should_Convert_To_An_Int_Using_Away_From_Zero(decimal input, int expected)
        {
            
            // Act
            var result = ValueCaster.ConvertToInt(input);

            // Assert
            Assert.Equal(expected, result);

        }

        [Theory]
        [InlineData(123.0, 123L)]
        [InlineData(123.45, 123L)]
        [InlineData(123.5, 124L)]
        [InlineData(124.5, 125L)]
        [InlineData(-124.5, -125L)]
        [InlineData(-123.5, -124L)]
        [InlineData(0, 0L)]
        public void It_Should_Convert_To_A_Long_Using_Away_From_Zero(decimal input, long expected)
        {

            // Act
            var result = ValueCaster.ConvertToLong(input);

            // Assert
            Assert.Equal(expected, result);

        }

        [Fact]
        public void It_Should_Convert_Null_To_Numeric_Zero()
        {

            // Act
            var result = ValueCaster.ConvertFromNull<LiquidNumeric>();
            var numericResult = result.SuccessValue<LiquidNumeric>();

            // Assert
            Assert.True(numericResult.IsInt);
            Assert.Equal(0, numericResult.IntValue);

        }

        [Fact]
        public void It_Should_Convert_Null_To_EmptyString()
        {

            // Act
            var result = ValueCaster.ConvertFromNull<LiquidString>();
            var stringResult = result.SuccessValue<LiquidString>();

            // Assert
            Assert.Equal("", stringResult.StringVal);

        }


//        [Fact]
//        public void It_Should_Cast_A_String_To_An_Array_Of_Strings()
//        {
//            // Arrange
//            var str = LiquidString.Create("Hello");
//
//            // Act
//            var arrayResult = ValueCaster.Cast<LiquidString, LiquidCollection>(str);
//            Assert.False(arrayResult.IsError);
//
//            // Assert
//            var arrValue = arrayResult.SuccessValue<LiquidCollection>().ArrValue;
//            Assert.Equal(5, arrValue.Count);
//            Assert.Equal("H,e,l,l,o", String.Join(",", arrValue.Select(x =>  ((LiquidString) x.Value).StringVal)));
//
//        }

        [Fact]
        public void It_Should_Cast_A_String_To_An_Array_Of_One_String()
        {
            // Arrange
            var str = LiquidString.Create("Hello");

            // Act
            var arrayResult = ValueCaster.Cast<LiquidString, LiquidCollection>(str);
            Assert.False(arrayResult.IsError);

            // Assert
            var arrValue = arrayResult.SuccessValue<LiquidCollection>();
            Assert.Equal(1, arrValue.Count);
            Assert.Equal("Hello", arrValue[0].Value.ToString());

        }

        [Fact]
        // SEE: https://github.com/Shopify/liquid/wiki/Liquid-for-Designers
        public void It_Should_Cast_KV_Pairs_In_A_Dictionary_To_An_Array_Of_Arrays_with_Two_Elements()
        {
            // Arrange
            var dictValue = new LiquidHash {
                    {"one", LiquidString.Create("ONE")},
                    {"two", LiquidString.Create("TWO")},
                    {"three", LiquidString.Create("THREE")},
                    {"four", LiquidString.Create("FOUR")}

                };

            // Act
            var result = ValueCaster.Cast<LiquidHash, LiquidCollection>(dictValue).SuccessValue<LiquidCollection>();

            // Assert

            Assert.Equal(4, result.Count);

        }


        [Fact]
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
            Assert.Equal("Result : { \"one\" : 1, \"two\" : 2, \"three\" : 3.0, \"four\" : 4 }", result);

        }

        [Fact]
        public void It_Should_Recursively_Render_Dictionaries_in_Json()
        {
            // Arrange     
            var subDictValue = new LiquidHash
            {
                {"abc", LiquidString.Create("def")}
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
            Assert.Equal("Result : { \"one\" : 1, \"two\" : { \"abc\" : \"def\" } }", result);

        }

        [Fact]
        public void It_Should_Render_A_Null_In_A_Dictionary()
        {
            // Arrange     
            var subDictValue = new LiquidHash
            {
                {"abc", LiquidString.Create("def")},
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
            Assert.Equal("Result : { \"one\" : null, \"two\" : { \"abc\" : \"def\", \"ghi\" : null } }", result);

        }

        [Fact]
        public void It_Should_Cast_Null_To_None()
        {
            var result = ValueCaster.Cast<LiquidString, LiquidNumeric>(null);
            Assert.True(result.IsSuccess);
            Assert.False(result.SuccessResult.HasValue);
        }

        [Fact]
        public void It_Should_ConvertFromNull()
        {
            var result = ValueCaster.ConvertFromNull<LiquidCollection>();
            Assert.True(result.IsSuccess);
            Assert.False(result.SuccessResult.HasValue);
        }

        [Fact]
        public void It_Should_Convert_Num()
        {
            var result = ValueCaster.Cast<LiquidNumeric, LiquidString>(LiquidNumeric.Create(1));
            Assert.True(result.IsSuccess);
            Assert.True(result.SuccessResult.HasValue);
        }

        [Fact]
        public void It_Should_Convert_An_Array()
        {
            var result = ValueCaster.Cast<LiquidCollection, LiquidString>(new LiquidCollection{LiquidString.Create("test")});
            Assert.True(result.IsSuccess);
            Assert.True(result.SuccessResult.HasValue);
            Assert.Equal("test", result.SuccessValue<LiquidString>().StringVal);
        }

        [Fact]
        public void It_Should_Convert_A_Date_To_A_Numeric()
        {
            var date = new DateTime(2015, 10, 29, 10, 11, 12);
            var result = ValueCaster.Cast<LiquidDate, LiquidNumeric>(new LiquidDate(date));
            Assert.True(result.IsSuccess);
            Assert.True(result.SuccessResult.HasValue);
            // ReSharper disable once PossibleInvalidOperationException
            Assert.Equal(date.Ticks, result.SuccessValue<LiquidNumeric>().LongValue);
        }

        [Fact]
        public void It_Should_Convert_A_Null_Date_To_0L()
        {
            var result = ValueCaster.Cast<LiquidDate, LiquidNumeric>(new LiquidDate(null));
            Assert.True(result.IsSuccess);
            Assert.True(result.SuccessResult.HasValue);
            // ReSharper disable once PossibleInvalidOperationException
            Assert.Equal(0L, result.SuccessValue<LiquidNumeric>().LongValue);
        }

        [Fact]
        public void It_Should_Convert_A_Numeric_To_A_Date()
        {
            var date = new DateTime(2015,10,29,10,11,12);
            var result = ValueCaster.Cast<LiquidNumeric, LiquidDate>(LiquidNumeric.Create(date.Ticks));
            Assert.True(result.IsSuccess);
            Assert.True(result.SuccessResult.HasValue);
            // ReSharper disable once PossibleInvalidOperationException
            Assert.Equal(date, result.SuccessValue<LiquidDate>().DateTimeValue.Value);
        }

        [Fact]
        public void It_Should_Cast_Numeric_To_Numeric()
        {
            var result = ValueCaster.Cast<LiquidNumeric, LiquidNumeric>(LiquidNumeric.Create(1));
            Assert.True(result.IsSuccess);
            Assert.True(result.SuccessResult.HasValue);
        }

        [Fact]
        public void It_Should_Not_Cast_Dict_To_Numeric()
        {
            var result = ValueCaster.Cast<LiquidHash, LiquidNumeric>(new LiquidHash());
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void It_Should_Not_Cast_Array_To_Numeric()
        {
            // Arrange
            var result = ValueCaster.Cast<LiquidCollection, LiquidNumeric>(new LiquidCollection { LiquidString.Create("test") });
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void It_Renders_Null_As_Empty()
        {
            // Arrange
            var result = ValueCaster.RenderAsString((ILiquidValue) null);
            Assert.Equal("", result);
        }

        [Fact]
        public void It_Should_Cast_Nil_To_0_In_A_Filter() // integration
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ x | plus: 1 }}");

            // Assert
            Assert.Equal("Result : 1", result);

        }

        [Fact]
        public void It_Should_Cast_Undefined_To_0_In_A_Filter_Arg() // integration
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ 3 | plus: x }}");

            // Assert
            Assert.Equal("Result : 3", result);

        }

        [Fact]
        public void It_Should_Cast_Nil_To_0_In_A_Filter_Arg() // integration
        {
            // Act

            var result = RenderingHelper.RenderTemplate("{% assign x = nil %}Result : {{ 3 | plus: x }}");

            // Assert
            Assert.Equal("Result : 3", result);

        }

    }
}
