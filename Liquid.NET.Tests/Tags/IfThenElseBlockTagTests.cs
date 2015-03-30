using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var result = RenderingHelper.RenderTemplate("Result : {% if false %}OK{% else true %}Else{% endif %}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : Else"));
        }

        [Test]
        public void It_Should_Render_Elsif()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% if false %}OK{% elsif true %}Else If{% else true %}Else{% endif %}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : Else If"));
        }

        [Test]
        public void It_Should_Render_Second_Elsif()
        {
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% if false %}OK{% elsif false %}Else If{% elsif true %}second else{% else true %}Else{% endif %}");

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

        [Test]
        public void It_Should_Break_Out_Of_A_Loop()
        {
            // Arrange
            var tmpl = GetForLoop("{% break %}");

            // Act
            var result = RenderingHelper.RenderTemplate(tmpl);

            // Assert
            Assert.That(result, Is.EqualTo("Result : 1,2"));
        }

        [Test]
        public void It_Should_Skip_Part_Of_A_Loop()
        {
            // Arrange
            var tmpl = GetForLoop("{% continue %}");

            // Act 
            var result = RenderingHelper.RenderTemplate(tmpl);

            // Assert
            Assert.That(result, Is.EqualTo("Result : 1,2"));
        }

        private static string GetForLoop(string txt)
        {
            return @"{%assign coll = ""1,2,3,4"" | split: ','%}"
                   + "{% for item in coll %}"
                   +"{% if item > 2 %}ITEM:{{item}}"
                   +txt
                   +"{% endif %}"
                   +"{{item}}"
                   +"{% endfor %}";
        }



    }
}
