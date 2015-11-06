using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Tags.Custom
{

    /// <summary>
    /// A Custom Block Tag looks like this
    /// {% mycustomtag [args] %}
    ///     ...Liquid markup...
    /// {% endmycustomtag %}
    /// 
    /// The CustomBlockTagRenderer receives the "args", a link to the symbol table,
    /// and 
    /// </summary>
    public interface ICustomBlockTagRenderer
    {

        LiquidString Render(
                    RenderingVisitor renderingVisitor,
                    ITemplateContext templateContext,
                    TreeNode<IASTNode> liquidBlock,
                    IList<Option<IExpressionConstant>> args);

    }
}
