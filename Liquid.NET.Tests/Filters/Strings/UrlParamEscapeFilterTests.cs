using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Liquid.NET.Tests.Filters.Strings
{
    [TestFixture]
    public class UrlParamEscapeFilterTests
    {
        [Test]
        public void It_Should_Render_Url_Params()
        {
            // Arrange
            var result = RenderingHelper.RenderTemplate("Result : {{ \"<hello> & <shopify>\" | url_param_escape }}");

            // Assert
            Assert.That(result, Is.EqualTo("Result : %3Chello%3E%20%26%20%3Cshopify%3E"));
        }
    }
}
