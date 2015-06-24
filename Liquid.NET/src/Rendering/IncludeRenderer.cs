using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;
using Liquid.NET.Tags;
using Liquid.NET.Utils;

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
            String virtualFileName = ValueCaster.RenderAsString(virtualFilenameVar.Value);

            if (symbolTableStack.FileSystem == null)
            {
                _renderingVisitor.Errors.Add(new LiquidError{Message = " ERROR: FileSystem is not defined"});
                return;
            }

            String snippet = symbolTableStack.FileSystem.Include(virtualFileName);

            var snippetAst = new LiquidASTGenerator().Generate(snippet);


            if (includeTag.ForExpression != null)
            {
                var forExpressionOption = LiquidExpressionEvaluator.Eval(includeTag.ForExpression, symbolTableStack);

                if (forExpressionOption.Value is DictionaryValue) // it seems to render as a single element if it's a dictionary.
                {
                    var localBlockScope = new SymbolTable();
                    DefineLocalVariables(symbolTableStack, localBlockScope, includeTag.Definitions);

                    var exprValue = LiquidExpressionEvaluator.Eval(includeTag.ForExpression, symbolTableStack);
                    localBlockScope.DefineVariable(virtualFileName, exprValue.Value);

                    RenderWithLocalScope(symbolTableStack, localBlockScope, snippetAst.RootNode);
                }
                else
                {
                    ArrayValue array = ValueCaster.Cast<IExpressionConstant, ArrayValue>(forExpressionOption.Value);

                    if (array.HasError)
                    {
                        _renderingVisitor.Errors.Add(new LiquidError {Message = array.ErrorMessage});
                        return;
                    }
                    foreach (IExpressionConstant val in array.ArrValue)
                    {
                        var localBlockScope = new SymbolTable();
                        DefineLocalVariables(symbolTableStack, localBlockScope, includeTag.Definitions);

                        localBlockScope.DefineVariable(virtualFileName, val);
                        RenderWithLocalScope(symbolTableStack, localBlockScope, snippetAst.RootNode);
                    }
                }
            }
            else
            {
                var localBlockScope = new SymbolTable();
                DefineLocalVariables(symbolTableStack, localBlockScope, includeTag.Definitions);
                if (includeTag.WithExpression != null)
                {
                    var withExpression = LiquidExpressionEvaluator.Eval(includeTag.WithExpression, symbolTableStack);
                    localBlockScope.DefineVariable(virtualFileName, withExpression.Value);
                }
                RenderWithLocalScope(symbolTableStack, localBlockScope, snippetAst.RootNode);
            }
            
                       
        }

        private void RenderWithLocalScope(SymbolTableStack symbolTableStack, SymbolTable localBlockScope, TreeNode<IASTNode> rootNode)
        {
            symbolTableStack.Push(localBlockScope);
            _astRenderer.StartVisiting(_renderingVisitor, rootNode);
            symbolTableStack.Pop();
        }

        private static void DefineLocalVariables(
            SymbolTableStack symbolTableStack,
            SymbolTable localBlockScope, 
            IDictionary<string, TreeNode<LiquidExpression>> definitions)
        {
            foreach (var def in definitions)
            {
                var option = LiquidExpressionEvaluator.Eval(def.Value, symbolTableStack);
                if (option.HasValue)
                {
                    localBlockScope.DefineVariable(def.Key, option.Value);
                }
                else
                {
                    localBlockScope.DefineVariable(def.Key, new NilValue());
                }
            }
        }
    }
}
