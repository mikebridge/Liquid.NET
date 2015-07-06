using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Symbols;

namespace Liquid.NET.Tests.Helpers
{
    public class StackHelper
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
