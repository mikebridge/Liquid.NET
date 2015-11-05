using Liquid.NET.Symbols;

namespace Liquid.NET.Tests.Helpers
{
    public static class StackHelper
    {
        public static SymbolTableStack CreateSymbolTableStack(TemplateContext ctx = null)
        {
            
            if (ctx == null)
            {
                ctx = new TemplateContext();    
            }
            ctx.WithAllFilters();
            return ctx.SymbolTableStack;
            //return SymbolTableStackFactory.CreateSymbolTableStack(ctx);
        }

    }
}
