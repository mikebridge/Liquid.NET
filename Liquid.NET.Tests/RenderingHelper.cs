using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.NET.Tests
{
    public static class RenderingHelper
    {
        public static string RenderTemplate(string resultHello)
        {
            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create(resultHello);
            return template.Render(ctx);
        }
    }
}
