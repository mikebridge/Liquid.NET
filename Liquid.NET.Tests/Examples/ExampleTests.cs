using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
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

            var template = LiquidTemplate.Create("<div>{{myvariable}}</div>");

            Assert.That(template.Render(ctx), Is.EqualTo("<div>Hello World</div>"));

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

            var template = LiquidTemplate.Create("<ul>{% for item in items %}<li>{{item}}</li>{% endfor %}</ul>");

            Assert.That(template.Render(ctx), Is.EqualTo("<ul><li>2</li><li>4</li><li>6</li></ul>"));

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

            var template = LiquidTemplate.Create("You said '{{ greeting.address }} {{ greeting.name.first }} {{ greeting.name.last }}'");

            Assert.That(template.Render(ctx), Is.EqualTo("You said 'Hello Tobias Lütke'"));

        }

        [Test]
        public void Test_Filter()
        {
            ITemplateContext ctx = new TemplateContext()
                .WithAllFilters()
                .DefineLocalVariable("resultcount", LiquidNumeric.Create(42))
                .DefineLocalVariable("searchterm", LiquidString.Create("MiXeDcAsE"));

            var template = LiquidTemplate.Create("{{ resultcount }} {{ resultcount | pluralize: 'item', 'items' }} were found for '{{searchterm | downcase}}'.");

            Assert.That(template.Render(ctx), Is.EqualTo("42 items were found for 'mixedcase'."));

        }

    }
}
