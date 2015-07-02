using System;
using Antlr4.Runtime;
using Liquid.NET.Tests.Ruby;
using NUnit.Framework;

namespace Liquid.NET.Tests.Tags
{
    [TestFixture]
    public class TableRowBlockTagTests
    {
        [Test]
        public void It_Should_Render_A_Table_Row()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.Define("numbers", DictionaryFactory.CreateArrayFromJson("[1, 2, 3, 4, 5, 6]"));
            var template = LiquidTemplate.Create(@"{% tablerow n in numbers cols:2%}ITER{{n}}{% endtablerow %}");
            
            // Act
            String result = template.Render(ctx);
            Console.WriteLine(result);
            String expected =
                @"<tr class=""row1""><td class=""col1"">ITER1</td><td class=""col2"">ITER2</td></tr><tr class=""row2"">" +
                @"<td class=""col1"">ITER3</td><td class=""col2"">ITER4</td></tr><tr class=""row3"">" +
                @"<td class=""col1"">ITER5</td><td class=""col2"">ITER6</td></tr>";
            // Act

            // Assert
            Assert.That(result, Is.EqualTo(expected));

        }
    }
}
