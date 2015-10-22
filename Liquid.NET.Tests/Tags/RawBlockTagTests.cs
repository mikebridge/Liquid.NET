using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Tags
{
    [TestFixture]
    public class RawBlockTagTests
    {
        [Test]
        public void It_Should_Not_Format_The_Raw_Text()
        {
            // Arrange
            const string tmpl = "Result : {% raw %}This is a comment{% endraw %}";
            Logger.Log(tmpl);
            String result = RenderingHelper.RenderTemplate(tmpl);

            // Assert
            Assert.That(result, Is.EqualTo("Result : This is a comment"));

        }

        [Test]
        public void It_Should_Render_Nested_Raw_Text()
        {
            // Arrange
            var raw = "This is {% raw %} nested raw {% endraw %}";
            String result = RenderingHelper.RenderTemplate("Result : {% raw %}" + raw + "{% endraw %}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : "+ raw));

        }


        [Test]
        public void It_Should_Not_Escape_Text_Between_Two_Raw_Areas()
        {
            // Arrange
            String result = RenderingHelper.RenderTemplate("Result : {% raw %}raw 1{% endraw %}{% if true %}HELLO{% endif %}{% raw %}raw 2{% endraw %}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : raw 1HELLOraw 2"));

        }

        [Test]
        public void It_Should_Escape_Tags_And_Filters()
        {
            // Arrange
            const string txt = "test {{ item }} test  {% if done %}test";
            const string tmpl = "Result : {% raw %}"+txt+"{% endraw %}";
            Logger.Log(tmpl);
            String result = RenderingHelper.RenderTemplate(tmpl);

            // Assert
            Assert.That(result, Is.EqualTo("Result : "+txt));

        }

    }
}
