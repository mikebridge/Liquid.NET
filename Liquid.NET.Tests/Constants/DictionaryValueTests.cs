using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Utils;
using NUnit.Framework;

namespace Liquid.NET.Tests.Constants
{
    [TestFixture]
    public class DictionaryValueTests
    {
        [Test]        
        public void It_Should_Dereference_A_Dictionary()
        {

            // Arrange
            var dictValue = new DictionaryValue
            {
                {"string1", new StringValue("a string")},
                {"string2", NumericValue.Create(123)},
                {"string3", NumericValue.Create(456m)}
            };

            // Assert
            Assert.That(dictValue.ValueAt("string1").Value, Is.EqualTo("a string"));
        }

        [Test]
        public void It_Should_Dereference_A_Nested_Dictionary()
        {

            // Arrange
            DictionaryValue dictValue3 = new DictionaryValue  {
                {"str", new StringValue("Dict 3")},
            };

            DictionaryValue dictValue2 = new DictionaryValue{
                {"dict3", dictValue3}
            };

            DictionaryValue dictValue = new DictionaryValue{
                {"dict2", dictValue2}        
            };

            ITemplateContext ctx = new TemplateContext()
                .DefineLocalVariable("dict1", dictValue);
            
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{dict1.dict2.dict3.str}}->{% if dict1.dict2.dict3.str == \"Dict 2\" %}Dict2{% elseif dict1.dict2.dict3.str == \"Dict 3\" %}Dict3{%endif%}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : Dict 3->Dict3"));
        }

        [Test]
        public void It_Should_Fail_When_Dereferencing_A_Missing_Property()
        {
            // Arrange
            DictionaryValue dictValue = new DictionaryValue();

            // Assert
            Assert.That(dictValue.ValueAt("string1").HasValue, Is.False);

        }

        [Test]
        public void It_Should_Initialize_The_DictionaryValue_With_OptionSyntax()
        {
            // Arrange
            var dict = new DictionaryValue
            {
                {"key1", Option<IExpressionConstant>.Create(new StringValue("test 1"))},
                {"key2", Option<IExpressionConstant>.Create(new StringValue("test 2"))},
            };

            // Assert
            Assert.That(dict.ValueAt("key1").Value, Is.EqualTo(new StringValue("test 1")));

        }

        [Test]
        public void It_Should_Initialize_The_DictionaryValue_With_IExpressionConstant()
        {
            // Arrange
            var dict = new DictionaryValue
            {
                {"key1", new StringValue("test 1")},
                {"key2", new StringValue("test 2")},
            };


            // Assert
            Assert.That(dict.ValueAt("key1").Value, Is.EqualTo(new StringValue("test 1")));

        }
    }
}
