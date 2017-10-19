using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using Liquid.NET.Constants;
using Liquid.NET.Utils;
using Xunit;

namespace Liquid.NET.Tests.Utils
{
    
    public class LiquidValueConverterTests
    {
        readonly LiquidValueConverter _converter;

        public LiquidValueConverterTests()
        {
            _converter = new LiquidValueConverter();
        }

        [Fact]
        public void It_Should_Convert_Null_To_None()
        {
            // Act
            var none = _converter.Convert(null);

            // Assert
            Assert.Equal(Option<ILiquidValue>.None(), none);
        }

        [Fact]
        public void It_Should_Convert_String_To_LiquidString()
        {
            // Act
            var testString = "Test STring";
            var val = _converter.Convert(testString);

            // Assert
            Assert.True(val.HasValue);
            Assert.IsType<LiquidString>(val.Value);
            Assert.Equal(LiquidString.Create(testString), val.Value);
        }

        [Theory]
        [InlineData(123, typeof(IntLiquidNumeric), 123)]
        [InlineData(123U, typeof(IntLiquidNumeric), 123)]
        [InlineData(123L, typeof(LongLiquidNumeric), 123)]
        [InlineData(123UL, typeof(LongLiquidNumeric), 123)]
        public void It_Should_Convert_Int_To_LiquidNumeric_Int(object origValue, Type type, object expected)
        {
            // Act
            var val = _converter.Convert(origValue);

            // Assert
            Assert.True(val.HasValue);
            Assert.True(val.Value.GetType().IsAssignableFrom(type));
            Assert.Equal(expected, Convert.ToInt32(val.Value.Value));
        }

        [Fact]
        public void It_Should_Convert_To_Decimal()
        {
            var val = _converter.Convert(123.2m);
            Assert.True(val.HasValue);
            Assert.IsType<DecimalLiquidNumeric>(val.Value);
        }

        [Fact]
        public void It_Should_Convert_Double_To_Decimal()
        {
            var val = _converter.Convert(123.2d);
            Assert.True(val.HasValue);
            Assert.IsType<DecimalLiquidNumeric>(val.Value);
            Assert.Equal(123.2m, val.Value.Value);
        }

        [Fact]
        public void It_Should_Convert_Float_To_Decimal()
        {
            var val = _converter.Convert(123.2f);
            Assert.True(val.HasValue);
            Assert.IsType<DecimalLiquidNumeric>(val.Value);
            Assert.Equal(123.2m, val.Value.Value);
        }

        [Fact]
        public void It_Should_Convert_Nullable_Int_To_NumericValue()
        {
            int? nullableInt = 123;
            var val = _converter.Convert(nullableInt);
            Assert.True(val.HasValue);
            Assert.IsType<IntLiquidNumeric>(val.Value);
            Assert.Equal(123, val.Value.Value);
        }


        [Fact]
        public void It_Should_Covert_BigInteger()
        {
            var val = _converter.Convert(new BigInteger(999));
            Assert.True(val.HasValue);
            Assert.IsType<BigIntegerLiquidNumeric>(val.Value);
            Assert.Equal(new BigInteger(999), val.Value.Value);
        }

        [Fact]
        public void It_Should_Covert_A_Boolean()
        {
            var val = _converter.Convert(true);
            Assert.True(val.HasValue);
            Assert.IsType<LiquidBoolean>(val.Value);
            Assert.True((bool) val.Value.Value);
        }


        [Fact]
        public void It_Should_Covert_A_DateTime()
        {
            var dateTime = new DateTime(2015,11,10,15,34,10);
            var val = _converter.Convert(dateTime);
            Assert.True(val.HasValue);
            Assert.IsType<LiquidDate>(val.Value);
            Assert.Equal(new LiquidDate(dateTime), val.Value);
        }

        [Fact]
        public void It_Should_Convert_Guid_To_LiquidString()
        {
            var guid = Guid.NewGuid();
            var val = _converter.Convert(guid);

            Assert.True(val.HasValue);
            Assert.IsType<LiquidString>(val.Value);
            Assert.Equal(LiquidString.Create(guid.ToString("D")), val.Value);
        }

        enum TestEnum { One, Two, Three }
        [Fact]
        public void It_Should_Convert_Enum_To_LiquidString()
        {
            var testEnum = TestEnum.One;
            var val = _converter.Convert(testEnum);

            Assert.True(val.HasValue);
            Assert.IsType<LiquidString>(val.Value);
            Assert.Equal(LiquidString.Create(Enum.GetName(testEnum.GetType(), testEnum)), val.Value);
        }

        [Fact]
        public void It_Should_Convert_Generic_List_To_Collection()
        {
            var list = new List<Object>();
            var val = _converter.Convert(list);
            Assert.True(val.HasValue);
            Assert.IsType<LiquidCollection>(val.Value);
        }

        [Fact]
        public void It_Should_Convert_IDictionary_To_Hash()
        {
            var dict = new Dictionary<String, Object>();
            var val = _converter.Convert(dict);
            Assert.True(val.HasValue);
            Assert.IsType<LiquidHash>(val.Value);
        }

        [Fact]
        public void It_Should_Convert_IDictionary_Values_To_Hash_When_String_Key()
        {
            var dict = new Dictionary<String, Object>{{"test", 1}, {"test2", "TEST"}};
            var val = _converter.Convert(dict);
            Assert.True(val.HasValue);
            var hash = (LiquidHash)val.Value;
            Assert.True(hash.ContainsKey("test"));
            Assert.True(hash.ContainsKey("test2"));
            Assert.Equal(LiquidNumeric.Create(1), hash["test"].Value);
            Assert.Equal(LiquidString.Create("TEST"), hash["test2"].Value);
        }

        [Fact]
        public void It_Should_Convert_IDictionary_Values_To_Hash_Via_Key_ToString_WHen_Conflicting()
        {
            var dict = new Dictionary<Object, Object> { { "3", "three" }, {3, "THREE" } };
            var val = _converter.Convert(dict);
            Assert.True(val.HasValue);
            var hash = (LiquidHash)val.Value;
            Assert.Equal(1, hash.Keys.Count);
            Assert.True(hash.ContainsKey("3"));
            Assert.Equal(LiquidString.Create("THREE"), hash["3"].Value);
        }

        [Fact]
        public void It_Should_Convert_IList_Values()
        {
            var arr = new List<Object> {"Test", 123, null };
            var val = _converter.Convert(arr);
            Assert.True(val.HasValue);
            var coll = (LiquidCollection) val.Value;
            Assert.Equal(3, coll.Count);
            Assert.Equal(LiquidString.Create("Test"), coll[0].Value);
            Assert.Equal(LiquidNumeric.Create(123), coll[1].Value);
            Assert.False(coll[2].HasValue);
        }

        [Fact]
        public void It_Should_Convert_An_Object()
        {
            // Act
            var testClass = new TestClass{FieLd1="aaa", Field1 = "bbb", IntField1=3, ObjField1="my obj"};
            var result = _converter.Convert(testClass);

            Assert.True(result.HasValue);
            var objAsHash = (LiquidHash) result.Value;

            // Assert
            Assert.True(objAsHash.ContainsKey("field1"));
            Assert.Equal(3, objAsHash.Keys.Count);
            Assert.Equal(LiquidString.Create(testClass.FieLd1), objAsHash["field1"].Value);
            Assert.Equal(LiquidNumeric.Create(testClass.IntField1), objAsHash["intfield1"].Value);
            Assert.Equal(LiquidString.Create((String) testClass.ObjField1), objAsHash["objfield1"].Value);
        }

        [Fact]
        public void It_Should_Convert_A_Nested_Object()
        {
            // Act
            var nestedClass = new TestClass { IntField1= 33};
            var testClass = new TestClass { FieLd1 = "aaa", Field1 = "bbb", IntField1 = 3, ObjField1 = nestedClass };

            var result = _converter.Convert(testClass);

            Assert.True(result.HasValue);
            var objAsHash = (LiquidHash)result.Value;

            // Assert
            Assert.IsType<LiquidHash>(objAsHash["objfield1"].Value);
            var foundNestedObject = (LiquidHash) objAsHash["objfield1"].Value;
            Assert.True(foundNestedObject.ContainsKey("intfield1"));
            Assert.True(foundNestedObject.ContainsKey("intfield1"));

        }

        [Fact]
        public void It_Should_Convert_An_Object_With_Null_Field()
        {
            // Act
            var testClass = new TestClass { FieLd1 = "aaa", Field1 = "bbb", IntField1 = 3, ObjField1 = null };

            var result = _converter.Convert(testClass);

            Assert.True(result.HasValue);
            var objAsHash = (LiquidHash)result.Value;

            // Assert
            Assert.False(objAsHash["objfield1"].HasValue);
        }

        [Fact]
        public void It_Should_Rename_A_Field()
        {
            // Act
            var testClass = new ClassWithAttributes { Ignored = "ignored", Ok = "ok", Renamed="renamed"};

            var result = _converter.Convert(testClass);

            Assert.True(result.HasValue);
            var objAsHash = (LiquidHash)result.Value;

            // Assert
            Assert.True(objAsHash["somethingelse"].HasValue);
            Assert.Equal(LiquidString.Create("renamed"), objAsHash["somethingelse"].Value);
        }

        [Fact]
        public void It_Should_Ignore_A_Field()
        {
            // Act
            var testClass = new ClassWithAttributes { Ignored = "ignored", Ok = "ok", Renamed = "renamed" };

            var result = _converter.Convert(testClass);

            Assert.True(result.HasValue);
            var objAsHash = (LiquidHash)result.Value;

            // Assert
            Assert.False(objAsHash.ContainsKey("ignored"));
            Assert.True(objAsHash.ContainsKey("ok"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("test")]
        public void It_Should_Ignore_A_Null_Field_When_IgnoreIfNull(String fieldValue)
        {
            // Act
            var testClass = new ClassWithAttributes { SomeField = fieldValue};

            var result = _converter.Convert(testClass);

            Assert.True(result.HasValue);
            var objAsHash = (LiquidHash)result.Value;

            // Assert
            Assert.Equal(fieldValue != null, objAsHash.ContainsKey("notnull"));
            if (objAsHash.ContainsKey("notnull"))
            {
                Assert.Equal(fieldValue.ToLiquid(), objAsHash.Value);
            }
        }

        public class TestClass
        {
            public String Field1 {get; set;}
            public String FieLd1 {get; set;} // different capitalization
            public int IntField1 {get; set;}
            public Object ObjField1 {get; set;}
            
        }

        public class ClassWithAttributes
        {
            [LiquidIgnore]
            public String Ignored { get; set; }

            [LiquidName("somethingelse")]
            public String Renamed { get; set; }

            [LiquidName("notnull")]
            [LiquidIgnoreIfNull]
            public String SomeField { get; set; }

            public String Ok { get; set; }

        }

    }
}
