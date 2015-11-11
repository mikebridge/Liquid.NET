using System;
using System.Collections.Generic;
using System.Numerics;
using Liquid.NET.Constants;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests.Utils
{
    [TestFixture]
    public class LiquidValueConverterTests
    {
        LiquidValueConverter _converter;

        [SetUp]
        public void SetUp()
        {
            _converter = new LiquidValueConverter();
        }

        [Test]
        public void It_Should_Convert_Null_To_None()
        {
            // Act
            var none = _converter.Convert(null);

            // Assert
            Assert.That(none, Is.EqualTo(Option<ILiquidValue>.None()));
        }

        [Test]
        public void It_Should_Convert_String_To_LiquidString()
        {
            // Act
            var testString = "Test STring";
            var val = _converter.Convert(testString);

            // Assert
            Assert.That(val.HasValue, Is.True);
            Assert.That(val.Value, Is.TypeOf<LiquidString>());
            Assert.That(val.Value, Is.EqualTo(LiquidString.Create(testString)));
        }

        [Test]
        [TestCase(123, typeof(IntLiquidNumeric), 123)]
        [TestCase(123U, typeof(IntLiquidNumeric), 123)]
        [TestCase(123L, typeof(LongLiquidNumeric), 123)]
        [TestCase(123UL, typeof(LongLiquidNumeric), 123)]
        public void It_Should_Convert_Int_To_LiquidNumeric_Int(object origValue, Type type, object expected)
        {
            // Act
            var val = _converter.Convert(origValue);

            // Assert
            Assert.That(val.HasValue, Is.True);
            Assert.That(val.Value.GetType().IsAssignableFrom(type), Is.True);
            Assert.That(val.Value.Value, Is.EqualTo(expected));
        }

        [Test]
        public void It_Should_Convert_To_Decimal()
        {
            var val = _converter.Convert(123.2m);
            Assert.That(val.HasValue, Is.True);
            Assert.That(val.Value, Is.TypeOf<DecimalLiquidNumeric>());
        }

        [Test]
        public void It_Should_Convert_Double_To_Decimal()
        {
            var val = _converter.Convert(123.2d);
            Assert.That(val.HasValue, Is.True);
            Assert.That(val.Value, Is.TypeOf<DecimalLiquidNumeric>());
            Assert.That(val.Value.Value, Is.EqualTo(123.2m));
        }

        [Test]
        public void It_Should_Convert_Float_To_Decimal()
        {
            var val = _converter.Convert(123.2f);
            Assert.That(val.HasValue, Is.True);
            Assert.That(val.Value, Is.TypeOf<DecimalLiquidNumeric>());
            Assert.That(val.Value.Value, Is.EqualTo(123.2m));
        }

        [Test]
        public void It_Should_Convert_Nullable_Int_To_NumericValue()
        {
            int? nullableInt = 123;
            var val = _converter.Convert(nullableInt);
            Assert.That(val.HasValue, Is.True);
            Assert.That(val.Value, Is.TypeOf<IntLiquidNumeric>());
            Assert.That(val.Value.Value, Is.EqualTo(123));
        }


        [Test]
        public void It_Should_Covert_BigInteger()
        {
            var val = _converter.Convert(new BigInteger(999));
            Assert.That(val.HasValue, Is.True);
            Assert.That(val.Value, Is.TypeOf<BigIntegerLiquidNumeric>());
            Assert.That(val.Value.Value, Is.EqualTo(new BigInteger(999)));
        }

        [Test]
        public void It_Should_Covert_A_Boolean()
        {
            var val = _converter.Convert(true);
            Assert.That(val.HasValue, Is.True);
            Assert.That(val.Value, Is.TypeOf<LiquidBoolean>());
            Assert.That(val.Value.Value, Is.True);
        }


        [Test]
        public void It_Should_Covert_A_DateTime()
        {
            var dateTime = new DateTime(2015,11,10,15,34,10);
            var val = _converter.Convert(dateTime);
            Assert.That(val.HasValue, Is.True);
            Assert.That(val.Value, Is.TypeOf<LiquidDate>());
            Assert.That(val.Value, Is.EqualTo(new LiquidDate(dateTime)));
        }

        [Test]
        public void It_Should_Convert_Generic_List_To_Collection()
        {
            var list = new List<Object>();
            var val = _converter.Convert(list);
            Assert.That(val.HasValue, Is.True);
            Assert.That(val.Value, Is.TypeOf<LiquidCollection>());
        }

        [Test]
        public void It_Should_Convert_IDictionary_To_Hash()
        {
            var dict = new Dictionary<String, Object>();
            var val = _converter.Convert(dict);
            Assert.That(val.HasValue, Is.True);
            Assert.That(val.Value, Is.TypeOf<LiquidHash>());
        }

        [Test]
        public void It_Should_Convert_IDictionary_Values_To_Hash_When_String_Key()
        {
            var dict = new Dictionary<String, Object>{{"test", 1}, {"test2", "TEST"}};
            var val = _converter.Convert(dict);
            Assert.That(val.HasValue, Is.True);
            var hash = (LiquidHash)val.Value;
            Assert.That(hash.ContainsKey("test"), Is.True);
            Assert.That(hash.ContainsKey("test2"), Is.True);
            Assert.That(hash["test"].Value, Is.EqualTo(LiquidNumeric.Create(1)));
            Assert.That(hash["test2"].Value, Is.EqualTo(LiquidString.Create("TEST")));
        }

        [Test]
        public void It_Should_Convert_IDictionary_Values_To_Hash_Via_Key_ToString_WHen_Conflicting()
        {
            var dict = new Dictionary<Object, Object> { { "3", "three" }, {3, "THREE" } };
            var val = _converter.Convert(dict);
            Assert.That(val.HasValue, Is.True);
            var hash = (LiquidHash)val.Value;
            Assert.That(hash.Keys.Count, Is.EqualTo(1));
            Assert.That(hash.ContainsKey("3"), Is.True);
            Assert.That(hash["3"].Value, Is.EqualTo(LiquidString.Create("THREE")));
        }

        [Test]
        public void It_Should_Convert_IList_Values()
        {
            var arr = new List<Object> {"Test", 123, null };
            var val = _converter.Convert(arr);
            Assert.That(val.HasValue, Is.True);
            var coll = (LiquidCollection) val.Value;
            Assert.That(coll.Count, Is.EqualTo(3));
            Assert.That(coll[0].Value, Is.EqualTo(LiquidString.Create("Test")));
            Assert.That(coll[1].Value, Is.EqualTo(LiquidNumeric.Create(123)));
            Assert.That(coll[2].HasValue, Is.False);
        }

        [Test]
        public void It_Should_Convert_An_Object()
        {
            // Act
            var testClass = new TestClass{FieLd1="aaa", Field1 = "bbb", IntField1=3, ObjField1="my obj"};
            var result = _converter.Convert(testClass);

            Assert.That(result.HasValue);
            var objAsHash = (LiquidHash) result.Value;

            // Assert
            Assert.That(objAsHash.ContainsKey("field1"));
            Assert.That(objAsHash.Keys.Count, Is.EqualTo(3));
            Assert.That(objAsHash["field1"].Value, Is.EqualTo(LiquidString.Create(testClass.FieLd1)));
            Assert.That(objAsHash["intfield1"].Value, Is.EqualTo(LiquidNumeric.Create(testClass.IntField1)));
            Assert.That(objAsHash["objfield1"].Value, Is.EqualTo(LiquidString.Create((String) testClass.ObjField1)));
        }

        [Test]
        public void It_Should_Convert_A_Nested_Object()
        {
            // Act
            var nestedClass = new TestClass { IntField1= 33};
            var testClass = new TestClass { FieLd1 = "aaa", Field1 = "bbb", IntField1 = 3, ObjField1 = nestedClass };

            var result = _converter.Convert(testClass);

            Assert.That(result.HasValue);
            var objAsHash = (LiquidHash)result.Value;

            // Assert
            Assert.That(objAsHash["objfield1"].Value, Is.InstanceOf<LiquidHash>());
            var foundNestedObject = (LiquidHash) objAsHash["objfield1"].Value;
            Assert.That(foundNestedObject.ContainsKey("intfield1"));
            Assert.That(foundNestedObject.ContainsKey("intfield1"));

        }

        [Test]
        public void It_Should_Convert_An_Object_With_Null_Field()
        {
            // Act
            var testClass = new TestClass { FieLd1 = "aaa", Field1 = "bbb", IntField1 = 3, ObjField1 = null };

            var result = _converter.Convert(testClass);

            Assert.That(result.HasValue);
            var objAsHash = (LiquidHash)result.Value;

            // Assert
            Assert.That(objAsHash["objfield1"].HasValue, Is.False);
        }

        [Test]
        public void It_Should_Rename_A_Field()
        {
            // Act
            var testClass = new ClassWithAttributes { Ignored = "ignored", Ok = "ok", Renamed="renamed"};

            var result = _converter.Convert(testClass);

            Assert.That(result.HasValue);
            var objAsHash = (LiquidHash)result.Value;

            // Assert
            Assert.That(objAsHash["somethingelse"].HasValue);
            Assert.That(objAsHash["somethingelse"].Value, Is.EqualTo(LiquidString.Create("renamed")));
        }

        [Test]
        public void It_Should_Ignore_A_Field()
        {
            // Act
            var testClass = new ClassWithAttributes { Ignored = "ignored", Ok = "ok", Renamed = "renamed" };

            var result = _converter.Convert(testClass);

            Assert.That(result.HasValue);
            var objAsHash = (LiquidHash)result.Value;

            // Assert
            Assert.That(objAsHash.ContainsKey("ignored"), Is.False);
            Assert.That(objAsHash.ContainsKey("ok"), Is.True);
        }

        [Test]
        [TestCase(null)]
        [TestCase("test")]
        public void It_Should_Ignore_A_Null_Field_When_IgnoreIfNull(String fieldValue)
        {
            // Act
            var testClass = new ClassWithAttributes { SomeField = fieldValue};

            var result = _converter.Convert(testClass);

            Assert.That(result.HasValue);
            var objAsHash = (LiquidHash)result.Value;

            // Assert
            Assert.That(objAsHash.ContainsKey("notnull"), Is.EqualTo(fieldValue != null));
            if (objAsHash.ContainsKey("notnull"))
            {
                Assert.That(objAsHash.Value, Is.EqualTo(fieldValue.ToLiquid()));
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
