using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Filters.Strings;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class UniqFilterTests
    {
        [Test]
        public void It_Should_Filter_Out_Unique_StringValues()
        {
            // Arrange
            const string tmpl = @"{% assign fruits = ""orange apple banana apple orange"" %}"
                    + "{{ fruits | split: ' ' | uniq | join: ' ' }}";

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl);
            Logger.Log(result);

            // Assert
            Assert.That(result, Is.EqualTo("orange apple banana"));

        }

        [Test]
        public void It_Should_Filter_Out_Unique_Simple_Objects()
        {
            // Arrange
            ArrayValue arrayValue = new ArrayValue{
                new StringValue("123"), 
                NumericValue.Create(456m),
                NumericValue.Create(123), 
                NumericValue.Create(123), 
                new BooleanValue(false)
            };
            var filter = new UniqFilter();

            // Act
            var result = filter.Apply(new TemplateContext(), arrayValue).SuccessValue<ArrayValue>();

            // Assert
            Assert.That(result.Select(ValueCaster.RenderAsString), Is.EquivalentTo(new List<String>{"123", "456.0", "123", "false"}));

        }

    }
}
