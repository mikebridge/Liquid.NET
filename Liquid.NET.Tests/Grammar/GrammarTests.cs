using System;
using NUnit.Framework;

namespace Liquid.NET.Tests.Grammar
{
    [TestFixture]
    //[Ignore("Not Implemented yet")]
    public class GrammarTests
    {

        [Test]
        public void It_Should_Echo_RawText()
        {
            // Act
            var result = RenderTemplate("HELLO");

            // Assert
            Assert.That(result, Is.EqualTo("HELLO"));

        }

        [Test]
        public void It_Should_Echo_A_String()
        {
            // Act
            var result = RenderTemplate("Result : {{  \"Hello\"  }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : Hello"));

        }

        [Test]
        public void It_Should_Echo_A_Boolean_True()
        {
            // Act
            var result = RenderTemplate("Result : {{ true}}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : true"));

        }

        [Test]
        public void It_Should_Echo_A_Boolean_False()
        {
            // Act
            var result = RenderTemplate("Result : {{ false }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : false"));

        }

        [Test]
        public void It_Should_Echo_A_Number()
        {
            // Act
            var result = RenderTemplate("Result : {{ 1 }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 1"));

        }

        [Test]
        public void It_Should_Extract_A_Filter_Chain()
        {
            // Act
            var result = RenderTemplate("Result : {{ \"test awesome\" | upcase | remove: \"AWESOME\" 123 }}");
            
            // Assert
            Assert.That(result, Is.EqualTo("Result : TEST "));

        }

        [Test]
        [Ignore("Need to write a thing for this")]
        public void RawArgs_Should_Contain_The_Original_Arg_String()
        {
            // Act
            var result = RenderTemplate("Result : {{ \"test awesome\" | echoargs: \"awesome\" 123 }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : TEST ")); // note: it doesn't remove that extra space.
        }

        [Test]
        public void It_Should_Extract_A_Filter()
        {
            // Act
            var result = RenderTemplate("Result : {{ \"test AWESOME\" | remove: \"AWESOME\" }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : test ")); // note: it doesn't remove that extra space.
        }

        [Test]
        public void It_Should_Apply_A_Filter_With_No_Args()
        {
            // Act
            var result = RenderTemplate("Result : {{ \"test it\" | upcase }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : TEST IT"));
        }

        [Test]
        public void It_Should_Cast_From_Numeric_To_String_When_Filters_Mismatched()
        {
            // Act
            var result = RenderTemplate("Result : {{ 33 | upcase }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 33"));
        }

        [Test]
        public void It_Should_Cast_From_String_To_Numeric_When_Filters_Mismatched()
        {
            // Act
            var result = RenderTemplate("Result : {{ \"33\" | plus: 3 }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 36"));
        }

        [Test]
        public void It_Should_Pass_A_Numeric_Arg()
        {
            // Act
            var result = RenderTemplate("Result : {{ 33 | plus: 3 }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : 36"));
        }

        [Test]
        public void It_Should_Render_An_Error_When_Conversion_From_Object_Fails()
        {
            // Act
            var result = RenderTemplate("Result : {{ true | plus: 3 }}");

            // Assert
            Assert.That(result, Is.StringContaining("Can't convert")); // note: it doesn't remove that extra space.
        }

        [Test]
        public void It_Should_Render_An_Error_When_Cant_Pass_Argument()
        {
            // Act
            var result = RenderTemplate("Result : {{ true | plus }}");
            Console.WriteLine(result);
            // Assert
            Assert.That(result, Is.StringContaining("Can't convert")); // note: it doesn't remove that extra space.
        }



        private static string RenderTemplate(string resultHello)
        {
            return RenderingHelper.RenderTemplate(resultHello);
        }

    }
}
