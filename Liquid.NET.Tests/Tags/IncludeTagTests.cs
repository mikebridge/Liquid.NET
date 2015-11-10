using System;
using System.Collections.Generic;
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
            
            //ctx.Define("payments", new LiquidCollection(new List<ILiquidValue>()));

            const String str = "{% include 'test' %}";

            // Act
            var result = RenderingHelper.RenderTemplate(str, ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Test Snippet"));

        }

        [Test]
        public void It_Should_Include_Name_Of_VirtualFile_With_Errors()
        {
            // Arrange
            var ctx = CreateContext(new Dictionary<String, String> { { "test", "{% if .wefiouhwef %}" } });

            //ctx.Define("payments", new LiquidCollection(new List<ILiquidValue>()));

            const String str = "{% include 'test' %}";
            IList<LiquidError> errors = new List<LiquidError>();
            // Act
            //try
            //{
                RenderingHelper.RenderTemplate(str, ctx, errors.Add);
                //Assert.Fail("Expected exception");
            //}
            //catch (LiquidParserException ex)
            //{
                //Assert.That(ex.LiquidErrors[0].TokenSource, Is.EqualTo("test"));    
            Assert.That(errors[0].TokenSource, Is.EqualTo("test"));    
            //}
            // Assert
            

        }

        [Test]
        public void It_Should_Include_A_Virtual_File_With_With()
        {
            // Arrange
            var ctx = CreateContext(new Dictionary<String, String> { { "test", "Test Snippet: {{ test }}" } });
            //ctx.Define("payments", new LiquidCollection(new List<ILiquidValue>()));

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
            //ctx.Define("payments", new LiquidCollection(new List<ILiquidValue>()));

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
            //ctx.Define("payments", new LiquidCollection(new List<ILiquidValue>()));

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
            //ctx.Define("payments", new LiquidCollection(new List<ILiquidValue>()));

            const String str = "{% include 'test' colour: 'Green', width: 10 %}";

            // Act
            var result = RenderingHelper.RenderTemplate(str, ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Colour: Green, Width: 10"));

        }

        [Test]
        public void It_Should_Use_Contexts_Caching_Strategy()
        {
            // Arrange
            bool called=false;
            var ctx = CreateContext(new Dictionary<String, String>
            {
                { "test", "Colour: {{ colour }}, Width: {{ width }}" }
            });
            var defaultAstGenerator = ctx.ASTGenerator;
            ctx.WithASTGenerator((str, errFn) => { called = true; return defaultAstGenerator(str, errFn); });

            const String tmpl = "{% include 'test' colour: 'Green', width: 10 %}";

            // Act
            RenderingHelper.RenderTemplate(tmpl, ctx);

            // Assert
            Assert.That(called, Is.True);

        }

        [Test]
        public void It_Should_Accumulate_Errors_When_Include_Contains_Rendering_Errors()
        {
            // Arrange
            var ctx = CreateContext(new Dictionary<String, String>
            {
                { "test", "problem: {{ 1 | divided_by: 0 }}" }
            }).WithAllFilters();
            //var defaultAstGenerator = ctx.ASTGenerator;
            //ctx.WithASTGenerator(str => { called = true; return defaultAstGenerator(str); });

            const String template = "{% include 'test' %}";

            // Act

            var ast = new LiquidASTGenerator().Generate(template);
            var renderingVisitor = new RenderingVisitor(ctx);
            String result = "";
            renderingVisitor.StartWalking(ast.LiquidAST.RootNode, x => result += x);
            Console.WriteLine(result);
            Assert.That(renderingVisitor.HasErrors);

        }

        [Test]
        public void It_Should_Accumulate_Errors_When_Include_Contains_Parsing_Errors()
        {
            // Arrange
            var ctx = CreateContext(new Dictionary<String, String>
            {
                { "test", "problem: {% unterminated" }
            }).WithAllFilters();
            //var defaultAstGenerator = ctx.ASTGenerator;
            //ctx.WithASTGenerator(str => { called = true; return defaultAstGenerator(str); });

            const String template = "{% include 'test' %}";

            // Act

            var ast = new LiquidASTGenerator().Generate(template);
            var renderingVisitor = new RenderingVisitor(ctx);
            String result = "";
            renderingVisitor.StartWalking(ast.LiquidAST.RootNode, x => result += x);
            Console.WriteLine(result);
            Assert.That(renderingVisitor.HasErrors);

        }


        [Test]
        public void It_Should_Render_An_Error_When_Include_Contains_Parsing_Errors()
        {
            // Arrange
            var ctx = CreateContext(new Dictionary<String, String>
            {
                { "test", "problem: {% unterminated" }
            }).WithAllFilters();

            const String template = "{% include 'test' %}";

            // Act

            var ast = new LiquidASTGenerator().Generate(template);
            var renderingVisitor = new RenderingVisitor(ctx);
            String result = "";
            renderingVisitor.StartWalking(ast.LiquidAST.RootNode, x => result += x);
            Console.WriteLine("RESULT: " + result);
            Assert.That(result, Is.StringContaining("missing TAGEND"));

        }



        private static ITemplateContext CreateContext(Dictionary<String, String> dict) 
        {
            return new TemplateContext().WithFileSystem(new TestFileSystem(dict));
        }

        private LiquidCollection CreateArrayValues()
        {
            return new LiquidCollection
            {
                LiquidNumeric.Create(1),
                LiquidNumeric.Create(2),
                LiquidNumeric.Create(3),
                LiquidNumeric.Create(4)
            };

        }

    }
}
