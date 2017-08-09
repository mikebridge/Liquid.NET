using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Xunit;

namespace Liquid.NET.Tests.Expressions
{
    
    public class ContainsExpressionTests
    {
        [Theory]
        [InlineData("\"hello\"", "'el'", "TRUE")]
        [InlineData("\"hello\"", "'e'", "TRUE")]
        [InlineData("\"hello\"", "'X'", "FALSE")]
        public void It_Should_Determine_If_A_String_Contains_A_Substring(String val, String contains, String expected)
        {
            // Act
            var result = RenderingHelper.RenderTemplate("{%if "+val+" contains "+contains+" %}TRUE{% else %}FALSE{% endif %}");

            // Assert
            Assert.Equal(expected, result);

        }

        [Theory]
        [InlineData("1", "FALSE")]
        [InlineData("\'1\'", "TRUE")]
        [InlineData("\'X\'", "FALSE")]
        [InlineData("2", "TRUE")]
        [InlineData("\'2\'", "FALSE")]
        [InlineData("true", "TRUE")]
        [InlineData("ZZZ", "FALSE")]
        public void It_Should_Determine_If_An_Array_Contains_An_Element(String contains, String expected)
        {
            // Arrange
            var ctx = new TemplateContext();
            ctx.DefineLocalVariable("array", CreateArray());

            // Act
            var result = RenderingHelper.RenderTemplate("{%if array contains " + contains + " %}TRUE{% else %}FALSE{% endif %}", ctx);

            // Assert
            Assert.Equal(expected, result);

        }

        [Theory]
        [InlineData("1", "FALSE")]
        [InlineData("\"one\"", "TRUE")]
        [InlineData("2", "FALSE")]
        [InlineData("\'two\'", "TRUE")]
        public void It_Should_Determine_If_An_Dictionary_Contains_A_KEy(String contains, String expected)
        {
            // Arrange
            var ctx = new TemplateContext();
            ctx.DefineLocalVariable("dict", CreateDictionary());

            // Act
            var result = RenderingHelper.RenderTemplate("{%if dict contains " + contains + " %}TRUE{% else %}FALSE{% endif %}", ctx);

            // Assert
            Assert.Equal(expected, result);

        }


        [Fact]
        public void It_Should_Not_Work_For_A_Numeric_Value()
        {
            // Arrange
            var ctx = new TemplateContext();
            ctx.DefineLocalVariable("array", CreateArray());

            // Act
            var result = RenderingHelper.RenderTemplate("{%if 3 contains 3 %}TRUE{% else %}FALSE{% endif %}");

            // Assert
            Assert.Contains("FALSE", result); // TODO: SHould this be an error?
        }

        public LiquidCollection CreateArray()
        {
            // Arrange
            return new LiquidCollection{
                LiquidString.Create("1"), 
                LiquidNumeric.Create(2), 
                LiquidString.Create("Three"),
                new LiquidBoolean(true)
            };;

        }

        public LiquidHash CreateDictionary()
        {
            return new LiquidHash
            {
                {"one", LiquidString.Create("1")},
                {"two", LiquidNumeric.Create(2)},
                {"three", LiquidString.Create("Three")},
                {"four", new LiquidBoolean(true)}
            };

        }


    }
}
