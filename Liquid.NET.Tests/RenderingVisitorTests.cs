using System;
using Liquid.NET.Tags;
using Xunit;

namespace Liquid.NET.Tests
{
    
    public class RenderingVisitorTests
    {
        [Fact]
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
            Assert.Equal(blocktext, result);

        }


        [Fact]
        //[ExpectedException(typeof(InvalidOperationException))]
        public void It_Should_Throw_An_Exception_When_No_Accumulator()
        {
            // Arrange
            var renderingVisitor = new RenderingVisitor(new TemplateContext());
            var rawTextNode = new RawBlockTag("HELLO");

            // Act
            Assert.Throws<InvalidOperationException>(() => renderingVisitor.Visit(rawTextNode));
          
        }

    }
}
