using Liquid.NET.Constants;
using Xunit;

namespace Liquid.NET.Tests.Filters.Strings
{
    
    public class Md5FilterTests
    {
        [Fact]
        public void It_Should_MD5_Hash_A_String()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("email", LiquidString.Create("mike@bridgecanada.com"));
            const string tmpl = "<img src=\"https://www.gravatar.com/avatar/{{ email | md5 }}\" />";
            var result = RenderingHelper.RenderTemplate(tmpl, ctx);
            Logger.Log(result);
            // Assert
            Assert.Equal("<img src=\"https://www.gravatar.com/avatar/517ea04cf362ddc08f107f6ef98a12d9\" />", result);

        }
    }
}
