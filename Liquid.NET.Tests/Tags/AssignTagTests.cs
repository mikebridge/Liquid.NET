using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Xunit;

namespace Liquid.NET.Tests.Tags
{
    
    public class AssignTagTests
    {
        [Fact]
        public void It_Should_Store_A_Variable()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create("{% assign foo = \"bar\" %}{{ foo }}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.Equal("bar", result);

        }

        [Fact]
        public void It_Should_Store_A_Boolean()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithDebuggingFilters();
            var template = LiquidTemplate.Create("{% assign foo = false %}{{ foo | type_of}}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.Equal("bool", result);

        }

        [Fact]
        public void It_Should_Assign_Null()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create("{% assign foo = null %}{{ foo }}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.Equal("", result);

        }

        [Fact]
        public void It_Refer_To_Another_Variable()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("array", CreateArrayValues());
            var template = LiquidTemplate.Create("{% assign foo = array %}{{ foo[1] }}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.Equal("123", result);

        }

       
        [Fact]
        public void It_Should_Evaluate_An_Expresson()
        {
            // Arrange
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            ctx.DefineLocalVariable("array", CreateArrayValues());
            var template = LiquidTemplate.Create("{% assign foo = \"test\" | upcase %}{{ foo }}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.Equal("TEST", result);

        }

        [Fact]
        public void It_Should_Assign_Empty_Values()
        {
            ITemplateContext ctx = new TemplateContext().WithAllFilters();
            var template = LiquidTemplate.Create("{% assign content_column_width = content_column_width | minus: image_column_width | minus: 10 -%}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.Equal("", result);
            
        }


        [Fact]
        public void It_Should_Assign_A_Bool_Value()
        {
            ITemplateContext ctx = new TemplateContext().WithDebuggingFilters();
            ctx.DefineLocalVariable("show", new LiquidBoolean(true));

            var template = LiquidTemplate.Create("{% assign show1 = show %}{{ show1 }}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;

            // Assert
            Assert.Equal("true", result);

        }


        [Fact]
        public void It_Should_Keep_Accuracy_In_A_Filter()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {% assign x = 1 | plus: 12.0 %}{{ x }}");

            // Assert
            Assert.Equal("Result : 13.0", result);
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
