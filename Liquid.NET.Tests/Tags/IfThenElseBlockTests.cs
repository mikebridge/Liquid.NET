using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Tags
{
    [TestFixture]
    public class IfThenElseBlockTests
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

    }
}
