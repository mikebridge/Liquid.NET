using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Tags;
using Liquid.NET.Tests.Ruby;
using Liquid.NET.Utils;
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
            cycleTag.CycleList.Add(new TreeNode<LiquidExpression>(new LiquidExpression { Expression = LiquidString.Create("A")}));
            cycleTag.CycleList.Add(new TreeNode<LiquidExpression>(new LiquidExpression { Expression = LiquidString.Create("B") }));
            cycleTag.CycleList.Add(new TreeNode<LiquidExpression>(new LiquidExpression { Expression = LiquidString.Create("C") }));

            // Assert
            Assert.That(((LiquidString) cycleTag.ElementAt(0).Data.Expression).StringVal, Is.EqualTo("A"));
            Assert.That(((LiquidString) cycleTag.ElementAt(1).Data.Expression).StringVal, Is.EqualTo("B"));
      }

        [Test]
        public void It_Should_Cycle_Through_Strings()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("array", CreateArrayValues());

            var template = LiquidTemplate.Create("Result : {% for item in array %}ITEM:{% cycle 'odd', 'even' %} {% endfor %}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result.TrimEnd(), Is.EqualTo("Result : ITEM:odd ITEM:even ITEM:odd ITEM:even"));

        }

        [Test]
        public void It_Should_Cycle_Through_Booleans()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("array", CreateArrayValues());

            var template = LiquidTemplate.Create("Result : {% for item in array %}ITEM:{% cycle true, false %} {% endfor %}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result.TrimEnd(), Is.EqualTo("Result : ITEM:true ITEM:false ITEM:true ITEM:false"));

        }

        [Test]
        public void It_Should_Cycle_Through_Nil()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("array", CreateArrayValues());

            var template = LiquidTemplate.Create("Result : {% for item in array %}ITEM:{% cycle nil %} {% endfor %}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result.TrimEnd(), Is.EqualTo("Result : ITEM: ITEM: ITEM: ITEM:"));

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
            ctx.DefineLocalVariable("array", CreateArrayValues());

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
            ctx.DefineLocalVariable("array", CreateArrayValues());

            var template = LiquidTemplate.Create("Result : {% for item in array %}{% cycle 'group1': '1', '2', '3' %}{% endfor %}{% for item in array %}{% cycle 'group2': '1', '2', '3' %}{% endfor %}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result.TrimEnd(), Is.EqualTo("Result : 12311231"));

        }

        [Test]
        public void It_Should_Cycle_Through_Variables()
        {
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("var1", LiquidString.Create("ONE"));
            ctx.DefineLocalVariable("var2", LiquidString.Create("TWO"));
            ctx.DefineLocalVariable("var3", new LiquidBoolean(false));
            ctx.DefineLocalVariable("var4", LiquidNumeric.Create(9));

            var template = LiquidTemplate.Create("Result : {% for item in (1..4) %}{% cycle var1, var2, var3, var4 %}{% endfor %}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result.TrimEnd(), Is.EqualTo("Result : ONETWOfalse9"));

        }

        [Test]
        public void It_Should_Cycle_Through_Vars_And_Non_Vars()
        {
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("var1", LiquidString.Create("ONE"));
            ctx.DefineLocalVariable("var2", new LiquidCollection{LiquidString.Create("TWO")});
            ctx.DefineLocalVariable("var3", new LiquidBoolean(false));
            ctx.DefineLocalVariable("var4", LiquidNumeric.Create(9));

            var template = LiquidTemplate.Create("Result : {% for item in (1..4) %}{% cycle \"ONE\", var2[0], var3, var4 %}{% endfor %}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result.TrimEnd(), Is.EqualTo("Result : ONETWOfalse9"));

        }

        [Test]
        public void It_Should_Allow_A_Var_In_Cycle_Group()
        {
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("var1", LiquidString.Create("ONE"));
            ctx.DefineLocalVariable("var2", new LiquidCollection{ LiquidString.Create("TWO") });
            ctx.DefineLocalVariable("var3", new LiquidBoolean(false));
            ctx.DefineLocalVariable("var4", LiquidNumeric.Create(9));

            var template = LiquidTemplate.Create("Result : {% for item in (1..4) %}{% cycle var1: \"ONE\", var2[0], var3, var4 %}{% endfor %}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result.TrimEnd(), Is.EqualTo("Result : ONETWOfalse9"));

        }


        [Test]
        [TestCase(@"{%cycle var1: ""one"", ""two"" %} {%cycle var2: ""one"", ""two"" %} {%cycle var1: ""one"", ""two"" %} {%cycle var2: ""one"", ""two"" %} {%cycle var1: ""one"", ""two"" %} {%cycle var2: ""one"", ""two"" %}", @"{""var1"":1,""var2"":2}", @"one one two two one one")]
        [TestCase(@"{%cycle 1,2%} {%cycle 1,2%} {%cycle 1,2%} {%cycle 1,2,3%} {%cycle 1,2,3%} {%cycle 1,2,3%} {%cycle 1,2,3%}", @"{}", @"1 2 1 1 2 3 1")]
        [TestCase(@"{%cycle 1: ""one"", ""two"" %} {%cycle 2: ""one"", ""two"" %} {%cycle 1: ""one"", ""two"" %} {%cycle 2: ""one"", ""two"" %} {%cycle 1: ""one"", ""two"" %} {%cycle 2: ""one"", ""two"" %}", @"{}", @"one one two two one one")]
        public void It_Should_Cycle_Through_Groups(String input, String assigns, String expected) {

            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            
            foreach (var tuple in DictionaryFactory.CreateStringMapFromJson(assigns))
            {
                ctx.DefineLocalVariable(tuple.Item1, tuple.Item2);
            }

            
            var template = LiquidTemplate.Create(input);
            
            // Act
            String result = template.Render(ctx);
        
            // Assert
            Assert.That(result.Trim(), Is.EqualTo(expected));
        }


        private LiquidCollection CreateArrayValues()
        {
            return new LiquidCollection{
                LiquidString.Create("a string"),
                LiquidNumeric.Create(123),
                LiquidNumeric.Create(456m),
                new LiquidBoolean(false)
            };
        }
    }
}
