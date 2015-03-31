using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Tree.Pattern;
using Liquid.NET.Constants;

using NUnit.Framework;

namespace Liquid.NET.Tests.Tags
{
    [TestFixture]
    public class ForBlockTagTests
    {
        [Test]
        public void It_Should_Iterate_Through_A_Collection()
        {
            // Arrange
            const string templateString = "Result : {% for item in array %}<li>{{ item }}</li>{% endfor %}";            
            TemplateContext ctx = new TemplateContext();
            ctx.Define("array", CreateArrayValues());
            var template = LiquidTemplate.Create(templateString);

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : <li>a string</li><li>123</li><li>456</li><li>false</li>"));
        }

        [Test]
        public void It_Should_Iterate_Through_A_Collection_Backward()
        {
            // Arrange
            const string templateString = "Result : {% for item in array reversed %}<li>{{ item }}</li>{% endfor %}";
            TemplateContext ctx = new TemplateContext();
            ctx.Define("array", CreateArrayValues());
            var template = LiquidTemplate.Create(templateString);

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : <li>false</li><li>456</li><li>123</li><li>a string</li>"));
        }

        [Test]
        public void It_Should_Iterate_Through_A_Collection_Offset()
        {
            // Arrange
            const string templateString = "Result : {% for item in array offset: 2%}<li>{{ item }}</li>{% endfor %}";
            TemplateContext ctx = new TemplateContext();
            ctx.Define("array", CreateArrayValues());
            var template = LiquidTemplate.Create(templateString);

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : <li>456</li><li>false</li>"));
        }

        [Test]
        public void It_Should_Iterate_Through_A_Collection_Limited()
        {
            // Arrange
            const string templateString = "Result : {% for item in array limit: 2%}<li>{{ item }}</li>{% endfor %}";
            TemplateContext ctx = new TemplateContext();
            ctx.Define("array", CreateArrayValues());
            var template = LiquidTemplate.Create(templateString);

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : <li>a string</li><li>123</li>"));
        }


        [Test]
        public void It_Should_Iterate_Through_A_Collection_Limit_Offset()
        {
            // Arrange
            const string templateString = "Result : {% for item in array limit: 1 offset: 2 %}<li>{{ item }}</li>{% endfor %}";
            TemplateContext ctx = new TemplateContext();
            ctx.Define("array", CreateArrayValues());
            var template = LiquidTemplate.Create(templateString);

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : <li>456</li>"));
        }

        [Test]
        public void It_Should_Not_Fail_If_Length_Off_End_Of_Array()
        {
            // Arrange
            const string templateString = "Result : {% for item in array limit: 6 offset: 2 %}<li>{{ item }}</li>{% endfor %}";
            TemplateContext ctx = new TemplateContext();
            ctx.Define("array", CreateArrayValues());
            var template = LiquidTemplate.Create(templateString);

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : <li>456</li><li>false</li>"));
        }

        [Test]
        public void It_Should_Not_Fail_If_Offset_Off_End_Of_Array()
        {
            // Arrange
            const string templateString = "Result : {% for item in array offset: 10 %}<li>{{ item }}</li>{% endfor %}";
            TemplateContext ctx = new TemplateContext();
            ctx.Define("array", CreateArrayValues());
            var template = LiquidTemplate.Create(templateString);

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : "));
        }



        [Test]
        public void It_Should_Not_Let_Local_Variable_Outside_Scope()
        {
            // Arrange
            const string templateString = "Result :{% for item in array %} Inside: {{ item }}{% endfor %} Outside: {{ item }}";
            TemplateContext ctx = new TemplateContext();
            ctx.Define("array", CreateArrayValues());
            var template = LiquidTemplate.Create(templateString);

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : Inside: a string Inside: 123 Inside: 456 Inside: false Outside: " + Undefined.CreateUndefinedMessage("item")));
        }

        [Test]
        public void It_Should_Iterate_Through_A_Nested_Collection()
        {
            // Arrange
            const string templateString = "Result : {% for subarray in array1 %}"
                                        + "<tr>{% for item in subarray %}"
                                        + "<td>{{item}}</td>"
                                        + "{% endfor %}"
                                        + "</tr>{% endfor %}";
            TemplateContext ctx = new TemplateContext();
            var numericValues = Enumerable.Range(0, 3).Select(x => (IExpressionConstant) new NumericValue(x)).ToList();
            var array2 = new ArrayValue(numericValues);
            //var array1 = Enumerable.Range(0, 3).Select(x => new ArrayValue(array2);
            var array1 = new ArrayValue(new List<IExpressionConstant> { array2, array2, array2 });
            ctx.Define("array1", array1);
            
            var template = LiquidTemplate.Create(templateString);

            // Act
            String result = template.Render(ctx);

            // Assert
            String row = "<tr><td>0</td><td>1</td><td>2</td></tr>";
            Assert.That(result, Is.EqualTo("Result : "+row + row + row));
        }

        [Test]
        public void It_Should_Iterate_Through_An_Array_Inside_An_Array()
        {
            // Arrange
            const string templateString = "Result : {% for item in array[1] %}<li>{{ item }}</li>{% endfor %}";
            Console.WriteLine(templateString);
            TemplateContext ctx = new TemplateContext();
            ctx.Define("array", new ArrayValue(new List<IExpressionConstant> {new StringValue("HELLO"), CreateArrayValues()}));
            var template = LiquidTemplate.Create(templateString);

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : <li>a string</li><li>123</li><li>456</li><li>false</li>"));

        }

        [Test]
        public void It_Should_Iterate_Through_A_String()
        {
            // Arrange

            var template = LiquidTemplate.Create("Result : {% for item in \"Hello\" %}<li>{{ item }}</li>{% endfor %}");

            // Act
            String result = template.Render(new TemplateContext());

            // Assert
            Assert.That(result, Is.EqualTo("Result : <li>H</li><li>e</li><li>l</li><li>l</li><li>o</li>"));

        }



        [Test]
        [TestCase("(0..3)","0123")]
        public void It_Should_Iterate_Through_A_Generator(String generator, String expected)
        {
            // Arrange
            var template = LiquidTemplate.Create("Result : {% for item in "+generator+" %}{{ item }}{% endfor %}");

            // Act
            String result = template.Render(new TemplateContext());

            // Assert
            Assert.That(result, Is.EqualTo("Result : " + expected));

        }


        [Test]
        public void It_Should_Iterate_Through_A_Generator_With_Vars()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.Define("array", new ArrayValue(new List<IExpressionConstant> { new NumericValue(10), new NumericValue(13) }));

            var template = LiquidTemplate.Create("Result : {% for item in (array[0]..array[1]) %}{{ item }}{% endfor %}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : 10111213"));

        }



        [Test]
        public void It_Should_Iterate_Through_A_Generator_Backwards_With_Vars()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.Define("array", new ArrayValue(new List<IExpressionConstant> { new NumericValue(5), new NumericValue(3) }));

            var template = LiquidTemplate.Create("Result : {% for item in (array[0]..array[1]) %}{{ item }}{% endfor %}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : 543"));

        }


        /// <summary>
        /// forloop.length      # => length of the entire for loop
        /// forloop.index       # => index of the current iteration
        /// forloop.index0      # => index of the current iteration (zero based)
        /// forloop.rindex      # => how many items are still left?
        /// forloop.rindex0     # => how many items are still left? (zero based)
        /// forloop.first       # => is this the first iteration?
        /// forloop.last        # => is this the last iteration?
        /// </summary>
        [Test]
        [TestCase("forloop.first", "", "true false false false")]
        [TestCase("forloop.index", "", "1 2 3 4")]
        [TestCase("forloop.index0", "", "0 1 2 3")]
        [TestCase("forloop.rindex", "", "4 3 2 1")]
        [TestCase("forloop.rindex0", "", "3 2 1 0")]
        [TestCase("forloop.last", "", "false false false true")]
        [TestCase("forloop.length", "", "4 4 4 4")]

        public void It_Should_Insert_ForLoop_First(String varname, String parms, String expected)
        {
            TemplateContext ctx = new TemplateContext();
            ctx.Define("array", CreateArrayValues());

            var template = LiquidTemplate.Create("Result : {% for item in array "+parms +"%}{{ "+varname+" }} {% endfor %}");

            // Act
            String result = template.Render(ctx);
            Console.WriteLine(result);

            // Assert
            Assert.That(result.Trim(), Is.EqualTo("Result : " + expected));

        }

        [Test]
        public void It_Should_Iterate_Through_A_Generator_From_A_Dictionary()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.Define("dict", new DictionaryValue(
                new Dictionary<string, IExpressionConstant>
                {
                    {"start", new NumericValue(1)},
                    {"end", new NumericValue(5)}
                }));
            var template = LiquidTemplate.Create("Result : {% for item in (dict.start..dict.end) %}{{ item }}{% endfor %}");

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : 12345"));

        }


        [Test]
        [TestCase("one : ONE")]
        [TestCase("two : TWO")]
        [TestCase("three : THREE")]
        [TestCase("four : FOUR")]
        public void It_Should_Iterate_Through_A_Dictionary(String expected)
        {
            // SEE: https://github.com/Shopify/liquid/wiki/Liquid-for-Designers

            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.Define("dict", new DictionaryValue(
                new Dictionary<string, IExpressionConstant>
                {
                    {"one", new StringValue("ONE")},
                    {"two", new StringValue("TWO")},
                    {"three", new StringValue("THREE")},
                    {"four", new StringValue("FOUR")}

                }));
            var template = LiquidTemplate.Create("Result : {% for item in dict %}<li>{{ item[0] }} : {{ item[1] }}</li>{% endfor %}");


            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.StringContaining(expected));

        }

        [Test]
        public void It_Should_Break_Out_Of_A_Loop()
        {
            // Arrange
            var tmpl = GetForLoop("{% break %}");

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl);

            // Assert
            Assert.That(result, Is.EqualTo("Result : loop1loop2loop"));
        }

        [Test]
        public void It_Should_Skip_Part_Of_A_Loop()
        {
            // Arrange
            var tmpl = GetForLoop("{% continue %}");

            // Act 
            var result = RenderingHelper.RenderTemplate(tmpl);

            // Assert
            Assert.That(result, Is.EqualTo("Result : loop1loop2looploop"));
        }

        private static string GetForLoop(string txt)
        {
            return @"Result : {%assign coll = ""1,2,3,4"" | split: ','%}"
                   + "{% for item in coll %}"
                   + "loop"
                   + "{% if item > 2 %}"
                   + txt
                   + "{% endif %}"
                   + "{{item}}"
                   + "{% endfor %}";
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
