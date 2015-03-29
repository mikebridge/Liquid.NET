using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Grammar;
using Liquid.NET.Symbols;
using Liquid.NET.Tags;
using NUnit.Framework;

namespace Liquid.NET.Tests
{
    [TestFixture]
    public class RenderingVisitorTests
    {
        [Test]
        public void It_Should_Render_A_Raw_Text_Node()
        {
            // Arrange
            const string blocktext = "HELLO";
            var renderingVisitor = CreateRenderingVisitor();
            var rawTextNode = new RawBlockTag(blocktext);

            // Act
            renderingVisitor.Visit(rawTextNode);

            // Assert
            Assert.That(renderingVisitor.Text, Is.EqualTo(blocktext));

        }


        private static RenderingVisitor CreateRenderingVisitor()
        {
            return new RenderingVisitor(new LiquidEvaluator(), new SymbolTableStack(new TemplateContext()));
        }
    }
}
