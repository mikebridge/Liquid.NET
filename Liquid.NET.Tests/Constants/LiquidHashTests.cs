using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests.Constants
{
    
    public class LiquidHashTests
    {
        [Fact]        
        public void It_Should_Dereference_A_LiquidHash()
        {

            // Arrange
            var dictValue = new LiquidHash
            {
                {"string1", LiquidString.Create("a string")},
                {"string2", LiquidNumeric.Create(123)},
                {"string3", LiquidNumeric.Create(456m)}
            };

            // Assert
            Assert.Equal("a string", dictValue.ValueAt("string1").Value.ToString());
        }

        [Fact]
        public void It_Should_Dereference_A_Nested_Hash()
        {

            // Arrange
            LiquidHash dictValue3 = new LiquidHash  {
                {"str", LiquidString.Create("Dict 3")},
            };

            LiquidHash dictValue2 = new LiquidHash{
                {"dict3", dictValue3}
            };

            LiquidHash dictValue = new LiquidHash{
                {"dict2", dictValue2}        
            };

            ITemplateContext ctx = new TemplateContext()
                .DefineLocalVariable("dict1", dictValue);
            
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{dict1.dict2.dict3.str}}->{% if dict1.dict2.dict3.str == \"Dict 2\" %}Dict2{% elseif dict1.dict2.dict3.str == \"Dict 3\" %}Dict3{%endif%}", ctx);

            // Assert
            Assert.Equal("Result : Dict 3->Dict3", result);
        }

        [Fact]
        public void It_Should_Fail_When_Dereferencing_A_Missing_Property()
        {
            // Arrange
            LiquidHash dictValue = new LiquidHash();

            // Assert
            Assert.False(dictValue.ValueAt("string1").HasValue);

        }

        [Fact]
        public void It_Should_Initialize_The_LiquidHash_With_OptionSyntax()
        {
            // Arrange
            var dict = new LiquidHash
            {
                {"key1", Option<ILiquidValue>.Create(LiquidString.Create("test 1"))},
                {"key2", Option<ILiquidValue>.Create(LiquidString.Create("test 2"))},
            };

            // Assert
            Assert.Equal(LiquidString.Create("test 1"), dict.ValueAt("key1").Value);

        }

        [Fact]
        public void It_Should_Initialize_The_LiquidHash_With_IExpressionConstant()
        {
            // Arrange
            var dict = new LiquidHash
            {
                {"key1", LiquidString.Create("test 1")},
                {"key2", LiquidString.Create("test 2")},
            };


            // Assert
            Assert.Equal(LiquidString.Create("test 1"), dict.ValueAt("key1").Value);

        }

        [Fact]
        public void It_Should_Implement_IDictionaryFunctions()
        {
            var dict = new LiquidHash();
            Assert.False(dict.IsReadOnly);
            Assert.NotNull(dict.GetEnumerator());
        }

        [Fact]
        public void It_Should_Set_A_Value()
        {
            var dict = new LiquidHash();
            dict["key"] = new Some<ILiquidValue>(LiquidString.Create("test"));
            Assert.Equal(LiquidString.Create("test"), dict["key"].Value);
        }


        [Fact]
        public void It_Should_Set_A_Value_Via_Key_Value_Pair()
        {
            var val = new Some<ILiquidValue>(LiquidString.Create("test"));           
            var kvp = new KeyValuePair<String,Option<ILiquidValue>>("key", val);
            var dict = new LiquidHash {kvp};
            Assert.Equal(LiquidString.Create("test"), dict["key"].Value);
        }

        [Fact]
        public void It_Should_Convert_Null_To_None()
        {
            var kvp = new KeyValuePair<String, Option<ILiquidValue>>("key", null);
            var dict = new LiquidHash {kvp};
            Assert.False(dict["key"].HasValue);
        }

        [Fact]
        public void It_Should_Add_A_Value()
        {
            var dict = new LiquidHash();
            dict.Add("key", LiquidString.Create("test"));
            Assert.Equal(LiquidString.Create("test"), dict["key"].Value);
        }

        [Fact]
        public void It_Should_Clear_Values()
        {
            var dict = new LiquidHash {{"key", LiquidString.Create("test")}};
            dict.Clear();
            Assert.Empty(dict);
        }

        [Fact]
        public void It_Should_Remove_A_Value()
        {
            var dict = new LiquidHash { { "key", LiquidString.Create("test") } };
            dict.Remove("key");
            Assert.Empty(dict);
        }

        [Fact]
        public void It_Should_Copy_Values()
        {
            var dict = new LiquidHash { { "key", LiquidString.Create("test") } };
            var x = new KeyValuePair<string, Option<ILiquidValue>>[1];
            dict.CopyTo(x, 0);
            Assert.Single(x);
        }

        [Fact]
        public void It_Should_Know_If_A_Value_Is_Contained()
        {
            var val = new Some<ILiquidValue>(LiquidString.Create("test"));
            var kvp = new KeyValuePair<String, Option<ILiquidValue>>("key", val);
            var dict = new LiquidHash { kvp };

            Assert.Contains(kvp, dict);
        }

        [Fact]
        public void It_Should_Retrieve_Values()
        {
            var dict = new LiquidHash {{"key", LiquidString.Create("test")}};
            Assert.Equal(1, dict.Values.Count);
        }

        [Fact]
        public void It_Should_Remove_Values()
        {

            var dict = new LiquidHash { { "key", LiquidString.Create("test") } };
            dict.Remove("key");
            Assert.False(dict.Values.Any());
        }

        [Fact]
        public void It_Should_Try_To_Retrieve_Values()
        {
            var dict = new LiquidHash { { "key", LiquidString.Create("test") } };
            Option<ILiquidValue> opt;
            bool success = dict.TryGetValue("key", out opt);
            Assert.True(success);
            // ReSharper disable once PossibleNullReferenceException
            Assert.Equal("test", ((LiquidString) opt.Value).StringVal);
        }

    }
}
