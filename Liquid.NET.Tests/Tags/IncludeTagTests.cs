using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Tests.Helpers;
using NUnit.Framework;

namespace Liquid.NET.Tests.Tags
{
    [TestFixture]
    public class IncludeTagTests
    {
        [Test]
        public void It_Should_Include_A_Virtual_File()
        {
            // Arrange
            var ctx = CreateContext(new Dictionary<String,String>{{"test", "Test Snippet"}});
            
            //ctx.Define("payments", new ArrayValue(new List<IExpressionConstant>()));

            const String str = "{% include 'test' %}";

            // Act
            var result = RenderingHelper.RenderTemplate(str, ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Test Snippet"));

        }

        [Test]
        public void It_Should_Include_A_Virtual_File_With_With()
        {
            // Arrange
            var ctx = CreateContext(new Dictionary<String, String> { { "test", "Test Snippet: {{ test }}" } });
            //ctx.Define("payments", new ArrayValue(new List<IExpressionConstant>()));

            const String str = "{% include 'test' with 'green' %}";

            // Act
            var result = RenderingHelper.RenderTemplate(str, ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Test Snippet: green"));

        }


        [Test]
        public void It_Should_Include_A_Virtual_File_With_For()
        {
            // Arrange
            var ctx = CreateContext(new Dictionary<String, String> { { "test", "Test Snippet: {{ test }} " } });
            ctx.DefineLocalVariable("array",CreateArrayValues());
            //ctx.Define("payments", new ArrayValue(new List<IExpressionConstant>()));

            const String str = "{% include 'test' for array %}";

            // Act
            var result = RenderingHelper.RenderTemplate(str, ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Test Snippet: 1 Test Snippet: 2 Test Snippet: 3 Test Snippet: 4 "));

        }


        [Test]
        public void It_Should_Include_A_Virtual_File_With_A_Variable_Value()
        {
            // Arrange
            var ctx = CreateContext(new Dictionary<String, String>{{"test", "Test Snippet: {{ colour }}"}});
            //ctx.Define("payments", new ArrayValue(new List<IExpressionConstant>()));

            const String str = "{%assign colour = 'green' %}{% include 'test' with colour %}";

            // Act
            var result = RenderingHelper.RenderTemplate(str, ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Test Snippet: green"));

        }

        [Test]
        public void It_Should_Define_Variables_In_Include()
        {
            // Arrange
            var ctx = CreateContext(new Dictionary<String, String> { { "test", "Colour: {{ colour }}, Width: {{ width }}" } });
            //ctx.Define("payments", new ArrayValue(new List<IExpressionConstant>()));

            const String str = "{% include 'test' colour: 'Green', width: 10 %}";

            // Act
            var result = RenderingHelper.RenderTemplate(str, ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Colour: Green, Width: 10"));

        }

        private static ITemplateContext CreateContext(Dictionary<String, String> dict) 
        {
            return new TemplateContext().WithFileSystem(new TestFileSystem(dict));
        }

        private ArrayValue CreateArrayValues()
        {
            var list = new List<IExpressionConstant>
            {
                new NumericValue(1),
                new NumericValue(2),
                new NumericValue(3),
                new NumericValue(4)
            };
            return new ArrayValue(list);
        }

    }
}
