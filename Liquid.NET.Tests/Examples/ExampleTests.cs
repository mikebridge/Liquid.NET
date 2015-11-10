using System;
using System.Linq;
using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Examples
{
    /// <summary>
    /// Examples in the Wiki
    /// </summary>
    [TestFixture]
    public class ExampleTests
    {
        [Test]
        public void Test_Simple_Template()
        {
            ITemplateContext ctx = new TemplateContext();

            ctx.DefineLocalVariable("myvariable", LiquidString.Create("Hello World"));

            var parsingResult = LiquidTemplate.Create("<div>{{myvariable}}</div>");

            Assert.That(parsingResult.LiquidTemplate.Render(ctx).Result, Is.EqualTo("<div>Hello World</div>"));

        }

        [Test]
        public void Test_Simple_Collection()
        {
            ITemplateContext ctx = new TemplateContext();

            ctx.DefineLocalVariable("items", new LiquidCollection
            {
                LiquidNumeric.Create(2), 
                LiquidNumeric.Create(4),
                LiquidNumeric.Create(6)
            });

            var parsingResult = LiquidTemplate.Create("<ul>{% for item in items %}<li>{{item}}</li>{% endfor %}</ul>");

            Assert.That(parsingResult.LiquidTemplate.Render(ctx).Result, Is.EqualTo("<ul><li>2</li><li>4</li><li>6</li></ul>"));

        }

        [Test]
        public void Test_Simple_Hash()
        {
            ITemplateContext ctx = new TemplateContext();
            var nameHash = new LiquidHash
            {
                {"first", LiquidString.Create("Tobias")},
                {"last", LiquidString.Create("Lütke")}
            };

            ctx.DefineLocalVariable("greeting", new LiquidHash
            {
                {"address", LiquidString.Create("Hello")},
                {"name", nameHash}
            });

            var parsingResult = LiquidTemplate.Create("You said '{{ greeting.address }} {{ greeting.name.first }} {{ greeting.name.last }}'");

            Assert.That(parsingResult.LiquidTemplate.Render(ctx).Result, Is.EqualTo("You said 'Hello Tobias Lütke'"));

        }

        [Test]
        public void Test_Filter()
        {
            ITemplateContext ctx = new TemplateContext()
                .WithAllFilters()
                .DefineLocalVariable("resultcount", LiquidNumeric.Create(42))
                .DefineLocalVariable("searchterm", LiquidString.Create("MiXeDcAsE"));

            var parsingResult = LiquidTemplate.Create("{{ resultcount }} {{ resultcount | pluralize: 'item', 'items' }} were found for '{{searchterm | downcase}}'.");

            Assert.That(parsingResult.LiquidTemplate.Render(ctx).Result, Is.EqualTo("42 items were found for 'mixedcase'."));

        }

        [Test]
        public void Test_Parsing_Error()
        {
            var parsingResult = LiquidTemplate.Create("This filter delimiter is not terminated: {{ myfilter");            
            String error = String.Join(",", parsingResult.ParsingErrors.Select(x => x.ToString()));
            Assert.That(error, Is.StringContaining("line 1:52 at <EOF>: Missing '}}'"));            
        }


        [Test]
        public void Test_Rendering_Error()
        {
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            var parsingResult = LiquidTemplate.Create("Divide by zero result in: {{ 1 | divided_by: 0}}");
            var renderingResult = parsingResult.LiquidTemplate.Render(ctx);
            String error = String.Join(",", renderingResult.RenderingErrors.Select(x => x.Message));
            //Console.WriteLine("The ERROR was : " + error);
            //Console.WriteLine("The RESULT was : " + renderingResult.Result);
            Assert.That(error, Is.StringContaining("Liquid error: divided by 0"));
        }
    }
}
