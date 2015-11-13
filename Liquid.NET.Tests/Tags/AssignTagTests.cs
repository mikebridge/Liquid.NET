using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Tags
{
    [TestFixture]
    public class AssignTagTests
    {
        [Test]
        public void It_Should_Store_A_Variable()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create("{% assign foo = \"bar\" %}{{ foo }}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.That(result, Is.EqualTo("bar"));

        }

        [Test]
        public void It_Should_Store_A_Boolean()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithDebuggingFilters();
            var template = LiquidTemplate.Create("{% assign foo = false %}{{ foo | type_of}}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.That(result, Is.EqualTo("bool"));

        }

        [Test]
        public void It_Should_Assign_Null()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create("{% assign foo = null %}{{ foo }}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.That(result, Is.EqualTo(""));

        }

        [Test]
        public void It_Refer_To_Another_Variable()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("array", CreateArrayValues());
            var template = LiquidTemplate.Create("{% assign foo = array %}{{ foo[1] }}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.That(result, Is.EqualTo("123"));

        }

       
        [Test]
        public void It_Should_Evaluate_An_Expresson()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            ctx.DefineLocalVariable("array", CreateArrayValues());
            var template = LiquidTemplate.Create("{% assign foo = \"test\" | upcase %}{{ foo }}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.That(result, Is.EqualTo("TEST"));

        }

        [Test]
        public void It_Should_Assign_Empty_Values()
        {
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            var template = LiquidTemplate.Create("{% assign content_column_width = content_column_width | minus: image_column_width | minus: 10 -%}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.That(result, Is.EqualTo(""));
            
        }


        [Test]
        public void It_Should_Assign_A_Bool_Value()
        {
            ITemplateContext ctx = new TemplateContext().WithDebuggingFilters();
            ctx.DefineLocalVariable("show", new LiquidBoolean(true));

            var template = LiquidTemplate.Create("{% assign show1 = show %}{{ show1 }}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.That(result, Is.EqualTo("true"));

        }

        [Test]
        public void It_Should_Add_A_Field_To_An_Existing_Hash()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext()
                .DefineLocalVariable("foo", new LiquidHash{{"oldfield", LiquidString.Create("OLD")}});
            var template = LiquidTemplate.Create("{% assign foo.newfield = \"NEW\" %}{{ foo.oldfield }} {{ foo.newfield }}")
                .OnParsingError( err => Assert.Fail("ERROR "+err.Message));

            // Act
            var result = template.LiquidTemplate.Render(ctx)
                .OnAnyError(err => Assert.Fail("ERROR " + err.Message))
                .Result;

            // Assert
            Assert.That(result, Is.EqualTo("OLD NEW"));

        }

        [Test]
        public void It_Should_Add_A_Defined_Field_To_An_Existing_Hash_Using_Array_Syntax()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext()
                .DefineLocalVariable("foo", new LiquidHash { { "oldfield", LiquidString.Create("OLD") } });

            var template = LiquidTemplate.Create("{% assign foo[\"newfield\"] = \"NEW\" %}{{ foo.oldfield }} {{ foo.newfield }}")
                .OnParsingError(err => Assert.Fail("ERROR " + err.Message));

            // Act
            var result = template.LiquidTemplate.Render(ctx)
                .OnAnyError(err => Assert.Fail("ERROR " + err.Message))
                .Result;

            // Assert
            Assert.That(result, Is.EqualTo("OLD NEW"));

        }

        [Test]
        public void It_Should_Add_A_Dynamically_Defined_Field_To_An_Existing_Hash()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext()
                .DefineLocalVariable("bar", LiquidString.Create("newfield"))
                .DefineLocalVariable("foo", new LiquidHash { { "oldfield", LiquidString.Create("OLD") } });

            var template = LiquidTemplate.Create("{% assign foo[bar] = \"NEW\" %}{{ foo.oldfield }} {{ foo.newfield }}")
                .OnParsingError(err => Assert.Fail("ERROR " + err.Message));

            // Act
            var result = template.LiquidTemplate.Render(ctx)
                .OnAnyError(err => Assert.Fail("ERROR " + err.Message))
                .Result;

            // Assert
            Assert.That(result, Is.EqualTo("OLD NEW"));

        }


        [Test]
        public void It_Should_Keep_Accuracy_In_A_Filter()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {% assign x = 1 | plus: 12.0 %}{{ x }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 13.0"));
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
