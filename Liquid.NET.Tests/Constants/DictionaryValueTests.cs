using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
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
            var dict = new Dictionary<String, IExpressionConstant>
            {
                {"string1", new StringValue("a string")},
                {"string2", new NumericValue(123)},
                {"string3", new NumericValue(456m)}
            };
            DictionaryValue dictValue = new DictionaryValue(dict);

            // Assert
            Assert.That(dictValue.ValueAt("string1").Value, Is.EqualTo(dict["string1"].Value));
        }

        [Test]
        public void It_Should_Dereference_A_Nested_Dictionary()
        {

            // Arrange
            var dict3 = new Dictionary<String, IExpressionConstant>
            {
                {"str", new StringValue("Dict 3")},
            };
            DictionaryValue dictValue3 = new DictionaryValue(dict3);

            var dict2 = new Dictionary<String, IExpressionConstant>
            {
                {"dict3", dictValue3}
            };
            DictionaryValue dictValue2 = new DictionaryValue(dict2);

            var dict = new Dictionary<String, IExpressionConstant>
            {
                {"dict2", dictValue2}        
            };
            DictionaryValue dictValue = new DictionaryValue(dict);

            ITemplateContext ctx = new TemplateContext()
                .DefineLocalVariable("dict1", dictValue);
            
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{dict1.dict2.dict3.str}}->{% if dict1.dict2.dict3.str == \"Dict 2\" %}Dict2{% elseif dict1.dict2.dict3.str == \"Dict 3\" %}Dict3{%endif%}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : Dict 3->Dict3"));
        }

    }
}
