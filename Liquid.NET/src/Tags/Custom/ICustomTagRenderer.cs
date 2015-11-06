using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags.Custom
{
    public interface ICustomTagRenderer
    {
        //LiquidString Render(SymbolTableStack symbolTableStack, IList<Option<ILiquidValue>> args);
        LiquidString Render(ITemplateContext symbolTableStack, IList<Option<ILiquidValue>> args);
    }
}
