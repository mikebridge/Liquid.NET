using Xunit;

namespace Liquid.NET.Tests.Grammar
{
    
    public class GrammarTests
    {

        [Fact]
        public void It_Should_Echo_RawText()
        {
            // Act
            var result = RenderTemplate("HELLO");

            // Assert
            Assert.Equal("HELLO", result);

        }

        [Fact]
        public void It_Should_Echo_A_String()
        {
            // Act
            var result = RenderTemplate("Result : {{  \"Hello\"  }}");

            // Assert
            Assert.Equal("Result : Hello", result);

        }

        [Fact]
        public void It_Should_Echo_A_Boolean_True()
        {
            // Act
            var result = RenderTemplate("Result : {{ true}}");

            // Assert
            Assert.Equal("Result : true", result);

        }

        [Fact]
        public void It_Should_Echo_A_Boolean_False()
        {
            // Act
            var result = RenderTemplate("Result : {{ false }}");

            // Assert
            Assert.Equal("Result : false", result);

        }

        [Fact]
        public void It_Should_Echo_A_Number()
        {
            // Act
            var result = RenderTemplate("Result : {{ 1 }}");

            // Assert
            Assert.Equal("Result : 1", result);

        }

        [Fact]
        public void It_Should_Extract_A_Filter_Chain()
        {
            // Act
            var result = RenderTemplate("Result : {{ \"test awesome\" | upcase | remove: \"AWESOME\" }}");
            
            // Assert
            Assert.Equal("Result : TEST ", result);

        }

        [Fact]
        public void It_Should_Extract_A_Filter()
        {
            // Act
            var result = RenderTemplate("Result : {{ \"test AWESOME\" | remove: \"AWESOME\" }}");

            // Assert
            Assert.Equal("Result : test ", result); // note: it doesn't remove that extra space.
        }

        [Fact]
        public void It_Should_Apply_A_Filter_With_No_Args()
        {
            // Act
            var result = RenderTemplate("Result : {{ \"test it\" | upcase }}");

            // Assert
            Assert.Equal("Result : TEST IT", result);
        }

        [Fact]
        public void It_Should_Cast_From_Numeric_To_String_When_Filters_Mismatched()
        {
            // Act
            var result = RenderTemplate("Result : {{ 33 | upcase }}");

            // Assert
            Assert.Equal("Result : 33", result);
        }

        [Fact]
        public void It_Should_Cast_From_String_To_Numeric_When_Filters_Mismatched()
        {
            // Act
            var result = RenderTemplate("Result : {{ \"33\" | plus: 3 }}");

            // Assert
            Assert.Equal("Result : 36", result);
        }

        [Fact]
        public void It_Should_Pass_A_Numeric_Arg()
        {
            // Act
            var result = RenderTemplate("Result : {{ 33 | plus: 3 }}");

            // Assert
            Assert.Equal("Result : 36", result);
        }

        [Fact]
        public void It_Should_Render_An_Error_When_Conversion_From_Object_Fails()
        {
            // Act
            var template = LiquidTemplate.Create("Result : {{ true | plus: 3 }}");
            var result = template.LiquidTemplate.Render(new TemplateContext().WithAllFilters());

            // Assert
            Assert.Contains("Can't convert", result.Result); // note: it doesn't remove that extra space.
        }

        [Fact]
        public void It_Should_Render_An_Error_When_Cant_Pass_Argument()
        {
            // Act
            //var result = RenderTemplate("Result : {{ true | plus }}");
            //Logger.Log(result);
            var template = LiquidTemplate.Create("Result : {{ true | plus }}");
            var result = template.LiquidTemplate.Render(new TemplateContext().WithAllFilters());

            // Assert
            Assert.Contains("Can't convert", result.Result); // note: it doesn't remove that extra space.
        }



        private static string RenderTemplate(string resultHello)
        {
            return RenderingHelper.RenderTemplate(resultHello);
        }

    }
}
