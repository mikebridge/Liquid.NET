using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters
{
    /// <summary>
    /// See:  See: http://www.ruby-doc.org/core/Time.html#method-i-strftime
    /// </summary>
    [TestFixture]   
    public class DateFilterTests
    {
        [Test]
        [TestCase("%y", "15")]
        [TestCase("%B", "March")]
        [TestCase("%m/%d/%Y", "03/30/2015")]
        public void It_Should_Format_A_Date(String format, string expected)
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            DateTime dateTime = new DateTime(2015, 3, 30, 23, 1, 12);
            ctx.DefineLocalVariable("mydate", new DateValue(dateTime));
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ mydate | date: \""+format+"\" }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : "+expected));
        }

        [Test]
        public void It_Should_Ignore_An_Empty_Date()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("mydate", new DateValue(null));
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ mydate | date: \"%y\" }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : "));

        }

        [Test]
        public void It_Should_Ignore_A_Missing_DateValue()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            //ctx.Define("mydate", null);
            // Act
            var result = RenderingHelper.RenderTemplate("Result : {{ mydate | date: \"%y\" }}", ctx);

            // Assert
            Assert.That(result, Is.EqualTo("Result : "));

        }

    }
}
