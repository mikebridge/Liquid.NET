using System;
using System.Linq;
using Liquid.NET.Constants;
using Xunit;

namespace Liquid.NET.Tests.Tags
{
    
    public class MacroBlockTagTests
    {
        [Fact]
        public void It_Should_Define_A_Macro()
        {
            // Arrange
            const string templateString =  @"Result : {% macro mymacro arg1 arg2 %}"
                                          +@"You said '{{ arg1 }}'."
                                          + @"{% endmacro %}"
                                          +@"{% mymacro ""hello"" %}";
           
            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create(templateString);

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.Equal("Result : You said 'hello'.", result);

        }

        [Fact]
        public void It_Should_Redefine_A_Macro()
        {
            // Arrange
            const string templateString = @"Result : {% macro mymacro arg1 %}"
                                          + @"You said '{{ arg1 }}'."
                                          + @"{% endmacro %}{% macro mymacro arg1 %}"
                                          + @"I heard '{{ arg1 }}'."
                                          + @"{% endmacro %}"
                                          + @"{% mymacro ""hello"" %}";

            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create(templateString);

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.Equal("Result : I heard 'hello'.", result);

        }


        [Fact]
        public void It_Should_Not_Overwrite_Global_Scope()
        {
            // Arrange
            const string templateString = @"{%assign arg1 = 'world' %}Result : {% macro mymacro arg1 arg2 %}"
                                          + @"You said {{ arg1 }} "
                                          + @"{% endmacro %}"
                                          + @"{% mymacro ""hello"" %}{{ arg1 }}";

            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create(templateString);

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.Equal("Result : You said hello world", result);

        }


        [Fact]
        public void It_Ignores_Extra_Variables()
        {
            // Arrange
            const string templateString = @"Result : {% macro mymacro arg1 %}"
                                          + @"You said {{ arg1 }}."
                                          + @"{% endmacro %}"
                                          + @"{% mymacro ""hello"" ""world""%}";

            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create(templateString);

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.Equal("Result : You said hello.", result);

        }

        /// test that state is maintained
        [Fact]
        public void It_Should_Assign_A_Cycle_To_Within_A_Macro()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("array", CreateArrayValues());

            var template = LiquidTemplate.Create("Result : {% macro thecycle %}{% cycle 'odd', 'even' %}{% endmacro %}{% for item in array %}{% thecycle %}{% endfor %}");

            // Act
//            try
//            {
            String result = template.LiquidTemplate.Render(ctx).Result;
                Logger.Log(result);

                // Assert
                Assert.Equal("Result : oddevenoddeven", result.TrimEnd());
//            }
//            catch (LiquidRendererException ex)
//            {
//                Logger.Log(ex.Message);
//                throw;
//            }

        }

        [Fact]
        public void It_Should_Render_A_Nested_Error_Inline()
        {
            // Act
            var templateContext = new TemplateContext().WithAllFilters();
            const string templateString = @"Result : {% macro mymacro arg1 %}"
                              + @"{{ 1 | divided_by: 0}}"
                              + @"{% endmacro %}"
                              + @"{% mymacro ""hello"" ""world""%}";

            //var result = RenderingHelper.RenderTemplate(templateString, templateContext);
            var templateResult = LiquidTemplate.Create(templateString);
            var renderingResult = templateResult.LiquidTemplate.Render(templateContext);

            // Assert
            Assert.Contains("Liquid error: divided by 0", renderingResult.RenderingErrors[0].Message);

        }

        [Fact]
        public void It_Should_Register_A_Rendering_Error()
        {
            // Act
            //IList<LiquidError> renderingErrors = new List<LiquidError>();
            var templateContext = new TemplateContext().WithAllFilters();
            const string templateString = @"Result : {% macro mymacro arg1 %}"
                              + @"{{ 1 | divided_by: 0}}"
                              + @"{% endmacro %}"
                              + @"{% mymacro ""hello"" ""world""%}";

            var template = LiquidTemplate.Create(templateString);

            var result = template.LiquidTemplate.Render(templateContext);

            //RenderingHelper.RenderTemplate(templateString, templateContext, renderingErrors.Add);

            // Assert
            Assert.Contains("Liquid error: divided by 0", result.RenderingErrors[0].Message);

        }

        [Fact]
        public void It_Should_Register_A_Parsing_Error()
        {
            // Act
            //IList<LiquidError> renderingErrors = new List<LiquidError>();
            //IList<LiquidError> parsingErrors = new List<LiquidError>();
            //var templateContext = new TemplateContext().WithAllFilters();
            const string templateString = @"Result : {% macro mymacro arg1 %}"
                              + @"{% dzserger "
                              + @"{% endmacro %}"
                              + @"{% mymacro ""hello"" ""world""%}";

            var template = LiquidTemplate.Create(templateString);

            //var result = template.LiquidTemplate.Render(templateContext);

            // Assert
            //Assert.False(result.RenderingErrors.Any(), Is.False);
            Assert.True(template.ParsingErrors.Any());
            Assert.Contains("mismatched input", template.ParsingErrors[0].Message);

        }

        [Fact]
        public void It_Should_Handle_Missing_Args()
        {
            // Act
            var templateContext = new TemplateContext().WithAllFilters();
            const string templateString = @"Result : {% macro mymacro arg1 %}"
                              + @"{{arg1}}"
                              + @"{% endmacro %}"
                              + @"{% mymacro x %}";
            var result = RenderingHelper.RenderTemplate(templateString, templateContext);

            // Assert
            Assert.Equal("Result : ", result);

        }


        [Fact(Skip = "When erroring-args is implemented, this should print an error.")]
        public void It_Should_Handle_Error_Args()
        {
            // Act
            var templateContext = new TemplateContext().WithAllFilters(); // .WithMissingArgsAsError();
            const string templateString = @"Result : {% macro mymacro arg1 %}"
                              + @"in macro:{{arg1}}"
                              + @"{% endmacro %}"
                              + @"{% mymacro x %}";
            var result = RenderingHelper.RenderTemplate(templateString, templateContext);

            // Assert
            Assert.Equal("Result : ", result);

        }

        private LiquidCollection CreateArrayValues()
        {
            return new LiquidCollection
            {
                LiquidString.Create("a string"),
                LiquidNumeric.Create(123),
                LiquidNumeric.Create(456m),
                new LiquidBoolean(false)
            };
        }

    }
}
