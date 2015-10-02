using System;
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
            String result = "";
            const string blocktext = "HELLO";
            var ctx = new TemplateContext();
            var renderingVisitor = new RenderingVisitor(ctx, str => result += str);
            var rawTextNode = new RawBlockTag(blocktext);

            // Act
            renderingVisitor.Visit(rawTextNode);

            // Assert
            Assert.That(result, Is.EqualTo(blocktext));

        }
    }
}
