using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Xunit;

namespace Liquid.NET.Tests.Tags
{
    
    public class IfThenElseBlockTagTests
    {
        [Fact]
        public void It_Should_Render_If_True()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% if true %}OK{% endif %}");

            // Assert
            Assert.Equal("Result : OK", result);
        }

        [Fact]
        public void It_Should_Render_If_False()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% if false %}OK{% endif %}");

            // Assert
            Assert.Equal("Result : ", result);
        }

        [Fact]
        public void It_Should_Render_Else()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% if false %}OK{% else %}Else{% endif %}");

            // Assert
            Assert.Equal("Result : Else", result);
        }

        [Fact]
        public void It_Should_Render_Elsif()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% if false %}OK{% elsif true %}Else If{% else %}Else{% endif %}");

            // Assert
            Assert.Equal("Result : Else If", result);
        }

        [Fact]
        public void It_Should_Render_Second_Elsif()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% if false %}OK{% elsif false %}Else If{% elsif true %}second else{% else %}Else{% endif %}");

            // Assert
            Assert.Equal("Result : second else", result);
        }

        [Fact]
        public void It_Should_Render_Nested_If_Statements()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% if true %}OK{% if false %}NOT OK{% endif %}{% if true %}OK{% endif %}{% endif %}");

            // Assert
            Assert.Equal("Result : OKOK", result);
        }

      
        /// <summary>
        /// https://github.com/mikebridge/Liquid.NET/wiki/Differences/
        /// </summary>
        [Fact]
        public void It_Should_Group_Expressions_With_Parentheses()
        {
            // Arrange
            const string str = @"{% if (false and true) or true %}Result #1 is true{% endif %}"
                             + @"{% if false and (true or true) %}Result #2 is true{% endif %}";
            // Act
            var result = RenderingHelper.RenderTemplate(str);

            // Assert
            Assert.Equal("Result #1 is true", result);

        }

        /// <summary>
        /// https://github.com/mikebridge/Liquid.NET/wiki/Differences/
        /// </summary>
        [Fact]
        public void It_Should_Allow_Not_To_Be_Used()
        {
            // Arrange
            const String txt = "not false is true!";
            const string str = @"{% if not false %}"+txt+"{% endif %}";

            // Act
            var result = RenderingHelper.RenderTemplate(str);

            // Assert
            Assert.Equal(txt, result);

        }


        /// <summary>
        /// https://github.com/Shopify/liquid/wiki/Liquid-for-Designers
        /// </summary>
        [Fact]
        public void It_Should_Test_An_Array_Against_NotEmpty()
        {
            // Arrange
            var ctx = CreateContextWithDictionary();


            const String str = "{% if user.payments == empty %}you never paid !{% endif %}";

            // Act
            var result = RenderingHelper.RenderTemplate(str, ctx );

            // Assert
            Assert.Equal("", result);

        }

        [Fact]
        public void It_Should_Test_An_Array_Against_Empty()
        {
            // Arrange
            var ctx = new TemplateContext();
            ctx.DefineLocalVariable("payments", new LiquidCollection());

            const String str = "{% if payments == empty %}This is empty{% endif %}";

            // Act
            var result = RenderingHelper.RenderTemplate(str, ctx);

            // Assert
            Assert.Equal("This is empty", result);

        }

        [Fact]
        public void It_Should_Test_Value_Against_Blank()
        {
            // Arrange
            var ctx = new TemplateContext();
            ctx.DefineLocalVariable("payments", new LiquidCollection());

            const String str = "{% if payments.blank? %}This is empty{% endif %}";

            // Act
            var result = RenderingHelper.RenderTemplate(str, ctx);

            // Assert
            Assert.Equal("This is empty", result);

        }

        private static TemplateContext CreateContextWithDictionary()
        {
            var ctx = new TemplateContext();
            var payments = new LiquidCollection
            {
                LiquidNumeric.Create(12.34m),
                LiquidNumeric.Create(33.45m),
            };

            ctx.DefineLocalVariable("user", new LiquidHash{
                {"payments", payments}
            });
            return ctx;
        }
    }
}
