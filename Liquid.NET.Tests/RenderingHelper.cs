using System;

namespace Liquid.NET.Tests
{
    public static class RenderingHelper
    {
        public static string RenderTemplate(
            string resultHello, 
            ITemplateContext ctx = null,
            Action<LiquidError> onRenderingError = null)
        {
            if (ctx == null)
            {
                ctx = new TemplateContext();
            }
            ctx.WithAllFilters();
            var template = LiquidTemplate.Create(resultHello);
            return template.Render(ctx, onRenderingError);
            
        }
    }
}
