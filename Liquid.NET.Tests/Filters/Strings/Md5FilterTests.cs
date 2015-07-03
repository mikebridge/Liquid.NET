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
            ctx.DefineLocalVariable("email", new StringValue("mike@bridgecanada.com"));
            const string tmpl = "<img src=\"https://www.gravatar.com/avatar/{{ email | md5 }}\" />";
            var result = RenderingHelper.RenderTemplate(tmpl, ctx);
            Console.WriteLine(result);
            // Assert
            Assert.That(result,
                Is.EqualTo("<img src=\"https://www.gravatar.com/avatar/517ea04cf362ddc08f107f6ef98a12d9\" />"));

        }
    }
}
