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
                Console.WriteLine(result);

                // Assert
                Assert.That(result.TrimEnd(), Is.EqualTo("Result : oddevenoddeven"));
            }
            catch (LiquidRendererException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        private ArrayValue CreateArrayValues()
        {
            var list = new List<IExpressionConstant>
            {
                new StringValue("a string"),
                NumericValue.Create(123),
                NumericValue.Create(456m),
                new BooleanValue(false)
            };
            return new ArrayValue(list);
        }

    }
}
