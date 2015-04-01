using System;

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
                                          +@"{% endmymacro %}"
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
                                          + @"{% endmymacro %}"
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
                                          + @"{% endmymacro %}"
                                          + @"{% mymacro ""hello"" ""world""%}";

            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create(templateString);

            // Act
            String result = template.Render(ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : You said hello."));

        }

    }
}
