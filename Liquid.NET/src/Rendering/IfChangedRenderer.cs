using System;
using System.Collections.Concurrent;

using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Rendering
{
    public class IfChangedRenderer
    {
        private readonly ConcurrentDictionary<String, String> _ifChangedCache = new ConcurrentDictionary<String, String>(); 
        private readonly RenderingVisitor _renderingVisitor;
        private readonly LiquidASTRenderer _astRenderer;

        public IfChangedRenderer(RenderingVisitor renderingVisitor, LiquidASTRenderer astRenderer)
        {
            _renderingVisitor = renderingVisitor;
            _astRenderer = astRenderer;
        }

        public String Next(string identifier, TreeNode<IASTNode> liquidBlock)
        {
            var hiddenText = CaptureHiddenText(liquidBlock);

            if (!_ifChangedCache.ContainsKey(identifier) || _ifChangedCache[identifier] != hiddenText)
            {
                _ifChangedCache[identifier] = hiddenText;
                return hiddenText;
            }
            else
            {
                return "";
            }
        }

        private string CaptureHiddenText(TreeNode<IASTNode> liquidBlock)
        {
            String hiddenText = "";

            _renderingVisitor.PushTextAccumulator(str => hiddenText += str);
            _astRenderer.StartVisiting(_renderingVisitor, liquidBlock);
            _renderingVisitor.PopTextAccumulator();

            return hiddenText;
        }
    }
}
