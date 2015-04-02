using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Parser
{
    [TestFixture]
    public class ErrorTests
    {
        [Test]
        [TestCase(@"TEST: {{ ""test,test"" | split: }} DONE", @"Liquid error: missing arguments after colon in filter 'split'")]
        public void It_Should_Handle_Invalid_Filters(String input, String expected)
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            IList<LiquidError> errors = new List<LiquidError>();
            var template = CreateRenderer(errors, input);

            // Act
            String result = template.Render(ctx);
            //Console.WriteLine("ERROR: " + String.Join(",", errors.Select(x => x.ToString())));
            //Console.WriteLine("ERRORS: " + errors);
            Console.WriteLine("RESULT IS ");
            Console.WriteLine(result);

            // Assert
            Assert.That(result, Is.StringContaining(expected));
            Assert.That(result, Is.Not.StringContaining(input));
        }

        [Test]
        public void It_Should_Save_Errors()
        {
            // Arrange
            const string erroneousTemplate = "{{";
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            IList<LiquidError> errors = new List<LiquidError>();            
            var template = CreateRenderer(errors, erroneousTemplate);


            // Act
            String result = template.Render(ctx);
            Console.WriteLine(result);
            Console.WriteLine("ERROR: "+String.Join(",", errors.Select(x => x.ToString())));
            
            //Assert.That(result, Is.EqualTo(expected));
            Assert.That(errors.Count, Is.EqualTo(1));
        }

        private static LiquidTemplate CreateRenderer(IList<LiquidError> errors, string erroneousTemplate)
        {
            var liquidAstGenerator = new LiquidASTGenerator();
            OnParsingErrorEventHandler liquidAstGeneratorParsingErrorEventHandler = errors.Add;

            liquidAstGenerator.ParsingErrorEventHandler += liquidAstGeneratorParsingErrorEventHandler;
            var liquidAst = liquidAstGenerator.Generate(erroneousTemplate);

            var template = new LiquidTemplate(liquidAst);
            return template;
        }
    }
}
