using System;
using System.Collections.Generic;

using Liquid.NET.Constants;
using Liquid.NET.Tags;
using NUnit.Framework;

namespace Liquid.NET.Tests.Tags
{
    [TestFixture]
    public class CycleTagTests
    {

        [Test]
        public void It_Should_Find_Elements_In_Its_Array()
        {
            // Arrange
            var cycleTag = new CycleTag();
            cycleTag.CycleList.Add(new StringValue("A"));
            cycleTag.CycleList.Add(new StringValue("B"));
            cycleTag.CycleList.Add(new StringValue("C"));

            // Assert
            Assert.That(cycleTag.ElementAt(0).Value.ToString(), Is.EqualTo("A"));
            Assert.That(cycleTag.ElementAt(1).Value.ToString(), Is.EqualTo("B"));
      }

        [Test]
        public void It_Should_Cycle_Through_Strings()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.Define("array", CreateArrayValues());

            var template = LiquidTemplate.Create("Result : {% for item in array %}ITEM:{% cycle 'odd', 'even' %} {% endfor %}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result.TrimEnd(), Is.EqualTo("Result : ITEM:odd ITEM:even ITEM:odd ITEM:even"));

        }

        /// <summary>
        /// Test of group behaviour described at 
        /// https://docs.shopify.com/themes/liquid-documentation/tags/iteration-tags#cycle
        /// </summary>
        [Test]
        public void It_Should_Maintain_State_For_Cycle_Groups_With_The_Same_Name_And_Keys()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.Define("array", CreateArrayValues());

            var template = LiquidTemplate.Create("Result : {% for item in array %}{% cycle '1', '2', '3' %}{% endfor %}{% for item in array %}{% cycle '1', '2', '3' %}{% endfor %}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result.TrimEnd(), Is.EqualTo("Result : 12312312"));

        }

        [Test]
        public void It_Should_Cycle_Through_Strings_In_Different_Groups()
        {
            TemplateContext ctx = new TemplateContext();
            ctx.Define("array", CreateArrayValues());

            var template = LiquidTemplate.Create("Result : {% for item in array %}{% cycle 'group1': '1', '2', '3' %}{% endfor %}{% for item in array %}{% cycle 'group2': '1', '2', '3' %}{% endfor %}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result.TrimEnd(), Is.EqualTo("Result : 12311231"));

        }

        private ArrayValue CreateArrayValues()
        {
            var list = new List<IExpressionConstant>
            {
                new StringValue("a string"),
                new NumericValue(123),
                new NumericValue(456m),
                new BooleanValue(false)
            };
            return new ArrayValue(list);
        }
    }
}
