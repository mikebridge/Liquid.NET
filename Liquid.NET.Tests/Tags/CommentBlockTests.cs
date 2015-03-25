using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Tags
{
    [TestFixture]
    public class CommentBlockTests
    {
        [Test]
        public void It_Should_Remove_The_Commented_Text()
        {
            // Arrange
            const string tmpl = "Result : {% comment %} This is a comment {% endcomment %}";
            Console.WriteLine(tmpl);
            String result = RenderingHelper.RenderTemplate(tmpl);

            // Assert
            Assert.That(result, Is.EqualTo("Result : "));

        }

        [Test]
        public void It_Should_Remove_Nested_Comments()
        {
            // Arrange
            String result = RenderingHelper.RenderTemplate("Result : {% comment %} This is {% comment %} nested comment {% endcomment %} {% endcomment %}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : "));

        }


        [Test]
        public void It_Should_Not_Remove_Text_Between_Two_Comments()
        {
            // Arrange
            String result = RenderingHelper.RenderTemplate("Result : {% comment %} comment 1 {% endcomment %}OK{% comment %} comment 2{% endcomment %}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : OK"));

        }

        [Test]
        public void It_Should_Remove_Tags_And_Filters()
        {
            // Arrange
            const string tmpl = "Result : {% comment %} test {{ item }} test  {% if done %}test {% endcomment %}";
            Console.WriteLine(tmpl);
            String result = RenderingHelper.RenderTemplate(tmpl);

            // Assert
            Assert.That(result, Is.EqualTo("Result : "));

        }

    }
}
