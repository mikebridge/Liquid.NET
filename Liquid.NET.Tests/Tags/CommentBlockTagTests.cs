using System;
using Xunit;

namespace Liquid.NET.Tests.Tags
{
    
    public class CommentBlockTagTests
    {
        [Fact]
        public void It_Should_Remove_The_Commented_Text()
        {
            // Arrange
            const string tmpl = "Result : {% comment %} This is a comment {% endcomment %}";
            Logger.Log(tmpl);
            String result = RenderingHelper.RenderTemplate(tmpl);

            // Assert
            Assert.Equal("Result : ", result);

        }

        [Fact]
        public void It_Should_Remove_Nested_Comments()
        {
            // Arrange
            String result = RenderingHelper.RenderTemplate("Result : {% comment %} This is {% comment %} nested comment {% endcomment %} {% endcomment %}");

            // Assert
            Assert.Equal("Result : ", result);

        }


        [Fact]
        public void It_Should_Not_Remove_Text_Between_Two_Comments()
        {
            // Arrange
            String result = RenderingHelper.RenderTemplate("Result : {% comment %} comment 1 {% endcomment %}OK{% comment %} comment 2{% endcomment %}");

            // Assert
            Assert.Equal("Result : OK", result);

        }

        [Fact]
        public void It_Should_Remove_Tags_And_Filters()
        {
            // Arrange
            const string tmpl = "Result : {% comment %} test {{ item }} test  {% if done %}test {% endcomment %}";
            Logger.Log(tmpl);
            String result = RenderingHelper.RenderTemplate(tmpl);

            // Assert
            Assert.Equal("Result : ", result);

        }

    }
}
