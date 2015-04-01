using System;
using System.Collections.Generic;

using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags.Custom
{
    public interface ICustomTagRenderer
    {
        StringValue Render(SymbolTableStack symbolTableStack, IList<IExpressionConstant> args);
    }
}
