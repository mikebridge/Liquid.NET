using System;
using System.Collections.Concurrent;
using System.Web.Caching;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Rendering
{
    public class IfChangedRenderer
    {
        private readonly ConcurrentDictionary<String, String> ifChangedCache = new ConcurrentDictionary<String, String>(); 
        private readonly RenderingVisitor _renderingVisitor;
        private readonly LiquidASTRenderer _astRenderer;
        private readonly ITemplateContext _templateContext;

        public IfChangedRenderer(RenderingVisitor renderingVisitor, LiquidASTRenderer astRenderer, ITemplateContext templateContext)
        {
            _renderingVisitor = renderingVisitor;
            _astRenderer = astRenderer;
            _templateContext = templateContext;
        }

        public String Next(string identifier, TreeNode<IASTNode> liquidBlock, LiquidASTRenderer astRenderer)
        {

            var hiddenVisitor = new RenderingVisitor(_astRenderer, _templateContext);
            _astRenderer.StartVisiting(hiddenVisitor, liquidBlock);
            var result = hiddenVisitor.Text;
            foreach (var error in hiddenVisitor.Errors)
            {
                _renderingVisitor.Errors.Add(error);
            }

            if (ifChangedCache.ContainsKey(identifier) && ifChangedCache[identifier] == result)
            {
                return "";
            }
            else
            {
                ifChangedCache[identifier] = result;
                return result;
            }
        }
    }
}
