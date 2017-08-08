using System;

namespace Liquid.NET.Tests
{
    public static class RenderingHelper
    {
        public static string RenderTemplate(
            string resultHello,
            ITemplateContext ctx = null)
        {
            if (ctx == null)
            {
                ctx = new TemplateContext();
            }
            ctx.WithAllFilters();
            var template = LiquidTemplate.Create(resultHello);
            var result= template.LiquidTemplate.Render(ctx);
            if (result.HasRenderingErrors || result.HasParsingErrors)
            {
                throw new Exception("Errors occurred....");
            }
            return result.Result;
        }

//        public static string RenderTemplate(
//            string resultHello, 
//            ITemplateContext ctx = null,
//            Action<LiquidError> onRenderingError = null,
//            Action<LiquidError> onParsingError = null)
//        {
//            if (ctx == null)
//            {
//                ctx = new TemplateContext();
//            }
//            ctx.WithAllFilters();
//            var template = LiquidTemplate.Create(resultHello);
//            return template.LiquidTemplate.Render(ctx, onRenderingError: onRenderingError, onParsingError: onParsingError);
//            
//        }
    }
}
