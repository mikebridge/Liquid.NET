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

    }
}
