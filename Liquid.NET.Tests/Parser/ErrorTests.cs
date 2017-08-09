using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Liquid.NET.Tests.Parser
{
    
    public class ErrorTests
    {
        [Theory]
        [InlineData(@"TEST: {{ ""test,test"" | split: }} DONE",
            @"Liquid error: missing arguments after colon in filter 'split'")]
        public void It_Should_Handle_Invalid_Filters(String input, String expected)
        {

            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            IList<LiquidError> errors = new List<LiquidError>();
            var template = CreateRenderer(errors, input);

            // Act
            template.Render(ctx);
            var err = errors.FirstOrDefault(x => x.ToString().Contains(expected));
            Assert.NotNull(err);

        }

        [Fact]
        public void It_Should_Save_Errors()
        {
            // Arrange
            const string erroneousTemplate = "{{";
            IList<LiquidError> errors = new List<LiquidError>();            
            
            // Act
            CreateRenderer(errors, erroneousTemplate);
            Assert.Equal(1, errors.Count);
        }


        [Fact]
        public void It_Should_Check_For_A_Missing_Colon()
        {
            // Act
            //try
            //{
            var templateResult = LiquidTemplate.Create("Result : {{ 2  | modulo 2 }}");
            //var result = template.LiquidTemplate.Render(ctx);
            //RenderingHelper.RenderTemplate("Result : {{ 2  | modulo 2 }}");
            //Assert.Fail("Expected exception");
            //}
            //catch (LiquidParserException ex)
            //{
            // Assert
            Assert.True(templateResult.HasParsingErrors);
            Assert.Contains("Liquid error: missing colon before args ", templateResult.ParsingErrors[0].Message);
            //}
        }

        private static LiquidTemplate CreateRenderer(IList<LiquidError> errors, string erroneousTemplate)
        {
            var liquidAstGenerator = new LiquidASTGenerator();
            //OnParsingErrorEventHandler liquidAstGeneratorParsingErrorEventHandler = errors.Add;

            //liquidAstGenerator.ParsingErrorEventHandler += liquidAstGeneratorParsingErrorEventHandler;
            var liquidAst = liquidAstGenerator.Generate(erroneousTemplate, errors.Add);

            var template = new LiquidTemplate(liquidAst);
            return template;
        }
    }
}
