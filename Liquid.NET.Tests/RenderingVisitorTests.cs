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

            var renderingVisitor = new RenderingVisitor(new TemplateContext());
            renderingVisitor.PushTextAccumulator(str => result += str);

            var rawTextNode = new RawBlockTag(blocktext);
           
            // Act
            renderingVisitor.Visit(rawTextNode);

            // Assert
            Assert.That(result, Is.EqualTo(blocktext));

        }


        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void It_Should_Throw_An_Exception_When_No_Accumulator()
        {
            // Arrange
            var renderingVisitor = new RenderingVisitor(new TemplateContext());
            var rawTextNode = new RawBlockTag("HELLO");

            // Act
            renderingVisitor.Visit(rawTextNode);
          
        }

    }
}
