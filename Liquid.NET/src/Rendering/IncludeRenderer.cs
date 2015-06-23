using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Tags;

namespace Liquid.NET.Rendering
{
    public class IncludeRenderer
    {
        private readonly RenderingVisitor _renderingVisitor;
        private readonly LiquidASTRenderer _astRenderer;

        public IncludeRenderer(RenderingVisitor renderingVisitor, LiquidASTRenderer astRenderer)
        {
            _renderingVisitor = renderingVisitor;
            _astRenderer = astRenderer;
        }

        public void Render(IncludeTag includeTag, SymbolTableStack symbolTableStack)
        {
            var virtualFilenameVar = LiquidExpressionEvaluator.Eval(includeTag.VirtualFileExpression, symbolTableStack);
            //IExpressionConstant withExpression = null;
            //IExpressionConstant forExpression = null;
            String virtualFileName = ValueCaster.RenderAsString(virtualFilenameVar);

            Console.WriteLine("Loading Template '" + virtualFileName + "'");

            if (symbolTableStack.FileSystem == null)
            {
                _renderingVisitor.Errors.Add(new LiquidError{Message = " ERROR: FileSystem is not defined"});
                return;
            }

            String snippet = symbolTableStack.FileSystem.Include(virtualFileName);
            Console.WriteLine("   snippet: " + snippet);
            var snippetAst = new LiquidASTGenerator().Generate(snippet);
            var localBlockScope = new SymbolTable();
            if (includeTag.WithExpression != null)
            {
                var withExpression = LiquidExpressionEvaluator.Eval(includeTag.WithExpression, symbolTableStack);
                localBlockScope.DefineVariable(virtualFileName, withExpression);
            }
            if (includeTag.ForExpression != null)
            {
                var forExpression = LiquidExpressionEvaluator.Eval(includeTag.ForExpression, symbolTableStack);
                localBlockScope.DefineVariable(virtualFileName, forExpression);
            }

            symbolTableStack.Push(localBlockScope);

            _astRenderer.StartVisiting(_renderingVisitor, snippetAst.RootNode);
            symbolTableStack.Pop();
            //_result += _astRenderer.EvalTree(_symbolTableStack, snippetAst);
        }
    }
}
