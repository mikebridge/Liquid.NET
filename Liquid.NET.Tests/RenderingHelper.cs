using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.NET.Tests
{
    public static class RenderingHelper
    {
        public static string RenderTemplate(string resultHello, ITemplateContext ctx = null)
        {
            if (ctx == null)
            {
                ctx = new TemplateContext();
            }
            ctx.WithAllFilters();
            var template = LiquidTemplate.Create(resultHello);
            return template.Render(ctx);
        }
    }
}
