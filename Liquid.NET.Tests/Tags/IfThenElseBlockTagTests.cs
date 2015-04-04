using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Tags
{
    [TestFixture]
    public class IfThenElseBlockTagTests
    {
        [Test]
        public void It_Should_Render_If_True()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% if true %}OK{% endif %}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : OK"));
        }

        [Test]
        public void It_Should_Render_If_False()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% if false %}OK{% endif %}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : "));
        }

        [Test]
        public void It_Should_Render_Else()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% if false %}OK{% else %}Else{% endif %}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : Else"));
        }

        [Test]
        public void It_Should_Render_Elsif()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% if false %}OK{% elsif true %}Else If{% else %}Else{% endif %}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : Else If"));
        }

        [Test]
        public void It_Should_Render_Second_Elsif()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% if false %}OK{% elsif false %}Else If{% elsif true %}second else{% else %}Else{% endif %}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : second else"));
        }

        [Test]
        public void It_Should_Render_Nested_If_Statements()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% if true %}OK{% if false %}NOT OK{% endif %}{% if true %}OK{% endif %}{% endif %}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : OKOK"));
        }

      
        /// <summary>
        /// https://github.com/mikebridge/Liquid.NET/wiki/Differences/
        /// </summary>
        [Test]
        public void It_Should_Group_Expressions_With_Parentheses()
        {
            // Arrange
            const string str = @"{% if (false and true) or true %}Result #1 is true{% endif %}"
                             + @"{% if false and (true or true) %}Result #2 is true{% endif %}";
            // Act
            var result = RenderingHelper.RenderTemplate(str);

            // Assert
            Assert.That(result, Is.EqualTo("Result #1 is true"));

        }

        /// <summary>
        /// https://github.com/mikebridge/Liquid.NET/wiki/Differences/
        /// </summary>
        [Test]
        public void It_Should_Allow_Not_To_Be_Used()
        {
            // Arrange
            const String txt = "not false is true!";
            const string str = @"{% if not false %}"+txt+"{% endif %}";

            // Act
            var result = RenderingHelper.RenderTemplate(str);

            // Assert
            Assert.That(result, Is.EqualTo(txt));

        }


        /// <summary>
        /// https://github.com/Shopify/liquid/wiki/Liquid-for-Designers
        /// </summary>
        [Test]
        public void It_Should_Test_An_Array_Against_Empty()
        {
            // Arrange
            var ctx = CreateContextWithDictionary();


            const String str = "{% if user.payments == empty %}you never paid !{% endif %}";

            // Act
            var result = RenderingHelper.RenderTemplate(str, ctx );

            // Assert
            Assert.That(result, Is.EqualTo("you never paid !"));

        }

        /// <summary>
        /// https://github.com/Shopify/liquid/wiki/Liquid-for-Designers
        /// </summary>
        [Test]
        public void It_Should_Throw_An_Error_If_Invalid_Key()
        {
            // Arrange
            var ctx = CreateContextWithDictionary();


            const String str = "{% if user.payments == wfwefewf %}you never paid !{% endif %}";

            // Act
            var result = RenderingHelper.RenderTemplate(str, ctx);

            // Assert
            Assert.That(result, Is.EqualTo("you never paid !"));

        }

        private static TemplateContext CreateContextWithDictionary()
        {
            var ctx = new TemplateContext();
            var payments = new List<IExpressionConstant>
            {
                new NumericValue(12.34m),
                new NumericValue(33.45m),
            };

            var dict = new Dictionary<String, IExpressionConstant>
            {
                {"payments", new ArrayValue(payments)}
            };
            ctx.Define("user", new DictionaryValue(dict));
            return ctx;
        }
    }
}
