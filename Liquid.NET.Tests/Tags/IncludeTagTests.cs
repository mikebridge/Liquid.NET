using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Tests.Helpers;
using Xunit;

namespace Liquid.NET.Tests.Tags
{
    
    public class IncludeTagTests
    {
        [Fact]
        public void It_Should_Include_A_Virtual_File()
        {
            // Arrange
            var ctx = CreateContext(new Dictionary<String,String>{{"test", "Test Snippet"}});
            
            //ctx.Define("payments", new LiquidCollection(new List<ILiquidValue>()));

            const String str = "{% include 'test' %}";

            // Act
            var result = RenderingHelper.RenderTemplate(str, ctx);

            // Assert
            Assert.Equal("Test Snippet", result);

        }

        [Fact]
        public void It_Should_Include_Name_Of_VirtualFile_With_ParsingErrors()
        {
            // Arrange
            var ctx = CreateContext(new Dictionary<String, String> { { "test", "{% if .wefiouhwef %}" } });

            const String str = "{% include 'test' %}";
            //IList<LiquidError> renderingErrors = new List<LiquidError>();
            //IList<LiquidError> parsingErrors = new List<LiquidError>();
            // Act

            var template = LiquidTemplate.Create(str);

            var result = template.LiquidTemplate.Render(ctx);
            //RenderingHelper.RenderTemplate(str, ctx, renderingErrors.Add, parsingErrors.Add);

            Assert.False(result.RenderingErrors.Any());
            Assert.True(result.ParsingErrors.Any());
            Assert.Equal("test", result.ParsingErrors[0].TokenSource);    

        }

        [Fact]
        public void It_Should_Include_A_Virtual_File_With_With()
        {
            // Arrange
            var ctx = CreateContext(new Dictionary<String, String> { { "test", "Test Snippet: {{ test }}" } });
            //ctx.Define("payments", new LiquidCollection(new List<ILiquidValue>()));

            const String str = "{% include 'test' with 'green' %}";

            // Act
            var result = RenderingHelper.RenderTemplate(str, ctx);

            // Assert
            Assert.Equal("Test Snippet: green", result);

        }


        [Fact]
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
            Assert.Equal("Test Snippet: 1 Test Snippet: 2 Test Snippet: 3 Test Snippet: 4 ", result);

        }


        [Fact]
        public void It_Should_Include_A_Virtual_File_With_A_Variable_Value()
        {
            // Arrange
            var ctx = CreateContext(new Dictionary<String, String>{{"test", "Test Snippet: {{ colour }}"}});
            //ctx.Define("payments", new LiquidCollection(new List<ILiquidValue>()));

            const String str = "{%assign colour = 'green' %}{% include 'test' with colour %}";

            // Act
            var result = RenderingHelper.RenderTemplate(str, ctx);

            // Assert
            Assert.Equal("Test Snippet: green", result);

        }

        [Fact]
        public void It_Should_Define_Variables_In_Include()
        {
            // Arrange
            var ctx = CreateContext(new Dictionary<String, String> { { "test", "Colour: {{ colour }}, Width: {{ width }}" } });
            //ctx.Define("payments", new LiquidCollection(new List<ILiquidValue>()));

            const String str = "{% include 'test' colour: 'Green', width: 10 %}";

            // Act
            var result = RenderingHelper.RenderTemplate(str, ctx);

            // Assert
            Assert.Equal("Colour: Green, Width: 10", result);

        }

        [Fact]
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
            Assert.True(called);

        }

        [Fact]
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
            Assert.True(renderingVisitor.HasErrors);

        }

        [Fact]
        public void It_Should_Accumulate_Errors_When_Include_Contains_Parsing_Errors()
        {
            // Arrange
            var ctx = CreateContext(new Dictionary<String, String>
            {
                { "test", "problem: {% unterminated" }
            }).WithAllFilters();

            const String template = "{% include 'test' %}";

            // Act

            var liquidParsingResult = new LiquidASTGenerator().Generate(template);
            Assert.False(liquidParsingResult.HasParsingErrors);

            var renderingVisitor = new RenderingVisitor(ctx);
            String result = "";
            renderingVisitor.StartWalking(liquidParsingResult.LiquidAST.RootNode, x => result += x);
            Console.WriteLine(result);
            Assert.True(renderingVisitor.HasErrors);

        }


        [Fact]
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
            Assert.Contains("missing TAGEND", result);

        }

        [Fact]
        public void It_Should_Register_A_Parsing_Error_At_Rendering_Time_When_Invalid_Syntax()
        {
            // Arrange
            var ctx = CreateContext(new Dictionary<String, String>
            {
                { "test", "problem: {% unterminated" }
            }).WithAllFilters();

            const String template = "{% include 'test' %}";

            // Act

            //var parsingResult = new LiquidASTGenerator().Generate(template);
            //Assert.False(parsingResult.HasParsingErrors);
            var liquidTemplate = LiquidTemplate.Create(template);

            var result = liquidTemplate.LiquidTemplate.Render(ctx);
            Console.WriteLine(result.Result);

            Assert.True(result.HasParsingErrors);
            Assert.Contains("missing TAGEND", result.ParsingErrors[0].Message);

        }

        [Fact]
        public void It_Should_Register_A_Rendering_Error()
        {
            // Arrange
            var ctx = CreateContext(new Dictionary<String, String>
            {
                { "test", "problem: {{ 1 | divided_by: 0}}" }
            }).WithAllFilters();

            const String template = "{% include 'test' %}";

            // Act

            //var parsingResult = new LiquidASTGenerator().Generate(template);

            var liquidTemplate = LiquidTemplate.Create(template);

            var result = liquidTemplate.LiquidTemplate.Render(ctx);

            Assert.True(result.HasRenderingErrors);
            Assert.Contains("divided by 0", result.RenderingErrors[0].Message);

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
