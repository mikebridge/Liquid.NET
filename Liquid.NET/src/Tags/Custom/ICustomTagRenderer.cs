using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags.Custom
{
    public interface ICustomTagRenderer
    {
        //StringValue Render(SymbolTableStack symbolTableStack, IList<Option<IExpressionConstant>> args);
        StringValue Render(ITemplateContext symbolTableStack, IList<Option<IExpressionConstant>> args);
    }
}
