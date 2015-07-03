using Liquid.NET.Tags;
using Liquid.NET.Tests.Helpers;
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
            var ctx = new TemplateContext();
            return new RenderingVisitor(new LiquidASTRenderer(), ctx);
        }
    }
}
