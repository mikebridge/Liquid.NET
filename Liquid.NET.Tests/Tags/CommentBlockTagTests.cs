using System;
using NUnit.Framework;

namespace Liquid.NET.Tests.Tags
{
    [TestFixture]
    public class CommentBlockTagTests
    {
        [Test]
        public void It_Should_Remove_The_Commented_Text()
        {
            // Arrange
            const string tmpl = "Result : {% comment %} This is a comment {% endcomment %}";
            Logger.Log(tmpl);
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
            Logger.Log(tmpl);
            String result = RenderingHelper.RenderTemplate(tmpl);

            // Assert
            Assert.That(result, Is.EqualTo("Result : "));

        }

    }
}
