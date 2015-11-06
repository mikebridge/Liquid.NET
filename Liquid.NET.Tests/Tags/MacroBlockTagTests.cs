using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Tags
{
    [TestFixture]
    public class MacroBlockTagTests
    {
        [Test]
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
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : You said 'hello'."));

        }

        [Test]
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
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : You said hello world"));

        }


        [Test]
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
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : You said hello."));

        }

        /// test that state is maintained
        [Test]
        public void It_Should_Assign_A_Cycle_To_Within_A_Macro()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("array", CreateArrayValues());

            var template = LiquidTemplate.Create("Result : {% macro thecycle %}{% cycle 'odd', 'even' %}{% endmacro %}{% for item in array %}{% thecycle %}{% endfor %}");

            // Act
            try
            {
                String result = template.Render(ctx);
                Logger.Log(result);

                // Assert
                Assert.That(result.TrimEnd(), Is.EqualTo("Result : oddevenoddeven"));
            }
            catch (LiquidRendererException ex)
            {
                Logger.Log(ex.Message);
                throw;
            }

        }

        [Test]
        public void It_Should_Parse_A_Nested_Error()
        {
            // Act
            var templateContext = new TemplateContext().WithAllFilters();
            const string templateString = @"Result : {% macro mymacro arg1 %}"
                              + @"{{ 1 | divided_by: 0}}"
                              + @"{% endmacro %}"
                              + @"{% mymacro ""hello"" ""world""%}";
            var result = RenderingHelper.RenderTemplate(templateString, templateContext);

            // Assert
            Assert.That(result, Is.EqualTo("Result : Liquid error: divided by 0"));

        }
        [Test]
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
            Assert.That(result, Is.EqualTo("Result : "));

        }

        [Test]
        [Ignore("When erroring-args is implemented, this should print an error.")]
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
            Assert.That(result, Is.EqualTo("Result : "));

        }

        private LiquidCollection CreateArrayValues()
        {
            return new LiquidCollection
            {
                new LiquidString("a string"),
                LiquidNumeric.Create(123),
                LiquidNumeric.Create(456m),
                new LiquidBoolean(false)
            };
        }

    }
}
