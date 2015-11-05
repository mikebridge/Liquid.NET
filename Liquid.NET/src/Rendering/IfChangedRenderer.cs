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

        public IfChangedRenderer(RenderingVisitor renderingVisitor)
        {
            _renderingVisitor = renderingVisitor;
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
            _renderingVisitor.StartWalking(liquidBlock);
            _renderingVisitor.PopTextAccumulator();

            return hiddenText;
        }
    }
}
