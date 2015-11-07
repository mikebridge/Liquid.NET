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


    }
}
