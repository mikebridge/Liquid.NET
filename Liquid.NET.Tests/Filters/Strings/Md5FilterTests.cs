using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class Md5FilterTests
    {
        [Test]
        public void It_Should_MD5_Hash_A_String()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.Define("email", new StringValue("tetsuro.takara@shopify.com"));
            String tmpl =
                "<img src=\"https://www.gravatar.com/avatar/{{ comment.email | remove: ' ' | strip_newlines | downcase | md5 }}";
            var result = RenderingHelper.RenderTemplate("", ctx);

            // Assert
            Assert.That(result,
                Is.EqualTo("<img src=\"https://www.gravatar.com/avatar/2a95ab7c950db9693c2ceb767784c201\" />"));

        }
    }
}
