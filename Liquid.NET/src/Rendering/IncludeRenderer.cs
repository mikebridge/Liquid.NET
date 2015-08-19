using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;

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

        public void Render(IncludeTag includeTag, ITemplateContext templateContext)
        {
            var virtualFilenameVar = LiquidExpressionEvaluator.Eval(includeTag.VirtualFileExpression, templateContext);
            if (virtualFilenameVar.IsError)
            {
                _renderingVisitor.Errors.Add(virtualFilenameVar.ErrorResult);
                return;  
            }

            String virtualFileName = ValueCaster.RenderAsString(virtualFilenameVar.SuccessResult.Value);

            if (templateContext.FileSystem == null)
            {
                _renderingVisitor.Errors.Add(new LiquidError{Message = " ERROR: FileSystem is not defined"});
                return;
            }

            String snippet = templateContext.FileSystem.Include(templateContext, virtualFileName);

            LiquidAST snippetAst;
            try
            {
                snippetAst = GenerateSnippetAst(snippet);
            }
            catch (LiquidParserException ex)
            {
                foreach (var error in ex.LiquidErrors)
                {
                    if (String.IsNullOrEmpty(error.TokenSource))
                    {
                        error.TokenSource = virtualFileName;
                    }
                }
                throw;
            }

            if (includeTag.ForExpression != null)
            {
                var forExpressionOption = LiquidExpressionEvaluator.Eval(includeTag.ForExpression, templateContext);
                if (forExpressionOption.IsError)
                {
                    _renderingVisitor.Errors.Add(forExpressionOption.ErrorResult);
                    return;
                }
                if (forExpressionOption.SuccessResult.Value is DictionaryValue) // it seems to render as a single element if it's a dictionary.
                {
                    var localBlockScope = new SymbolTable();
                    DefineLocalVariables(templateContext, localBlockScope, includeTag.Definitions);

                    var exprValue = LiquidExpressionEvaluator.Eval(includeTag.ForExpression, templateContext);
                    localBlockScope.DefineLocalVariable(virtualFileName, exprValue.SuccessResult.Value);

                    RenderWithLocalScope(templateContext, localBlockScope, snippetAst.RootNode);
                }
                else
                {
                    //ArrayValue array = ValueCaster.Cast<IExpressionConstant, ArrayValue>(forExpressionOption.SuccessResult.Value);
                    var arrayResult = ValueCaster.Cast<IExpressionConstant, ArrayValue>(forExpressionOption.SuccessResult.Value);
                    if (arrayResult.IsError)
                    {
                        _renderingVisitor.Errors.Add(arrayResult.ErrorResult);
                        return;
                    }

//                    if (array.HasError)
//                    {
//                        _renderingVisitor.Errors.Add(new LiquidError {Message = array.ErrorMessage});
//                        return;
//                    }
                    foreach (Option<IExpressionConstant> val in arrayResult.SuccessValue<ArrayValue>())
                    {
                        var localBlockScope = new SymbolTable();
                        DefineLocalVariables(templateContext, localBlockScope, includeTag.Definitions);
                        if (val.HasValue)
                        {
                            localBlockScope.DefineLocalVariable(virtualFileName, val.Value);
                        }
                        else
                        {
                            localBlockScope.DefineLocalVariable(virtualFileName, null);
                        }
                        RenderWithLocalScope(templateContext, localBlockScope, snippetAst.RootNode);
                    }
                }
            }
            else
            {
                var localBlockScope = new SymbolTable();
                DefineLocalVariables(templateContext, localBlockScope, includeTag.Definitions);
                if (includeTag.WithExpression != null)
                {
                    var withExpression = LiquidExpressionEvaluator.Eval(includeTag.WithExpression, templateContext);
                    localBlockScope.DefineLocalVariable(virtualFileName, withExpression.SuccessResult.Value);
                }
                RenderWithLocalScope(templateContext, localBlockScope, snippetAst.RootNode);
            }
            
                       
        }

        private static LiquidAST GenerateSnippetAst(string snippet)
        {
            return new CachingLiquidASTGenerator(new LiquidASTGenerator()).Generate(snippet);
            //return new LiquidASTGenerator().Generate(snippet);
        }

        private void RenderWithLocalScope(ITemplateContext templateContext, SymbolTable localBlockScope, TreeNode<IASTNode> rootNode)
        {
            templateContext.SymbolTableStack.Push(localBlockScope);
            _astRenderer.StartVisiting(_renderingVisitor, rootNode);
            templateContext.SymbolTableStack.Pop();
        }

        private static void DefineLocalVariables(
            ITemplateContext templateContext,
            SymbolTable localBlockScope, 
            IDictionary<string, TreeNode<LiquidExpression>> definitions)
        {
            foreach (var def in definitions)
            {
                var liquidExpressionREsult = LiquidExpressionEvaluator.Eval(def.Value, templateContext);
                if (liquidExpressionREsult.IsError)
                {
                    // TODO: check if this should ignore this or not.
                }
                localBlockScope.DefineLocalVariable(def.Key,
                    liquidExpressionREsult.SuccessResult.HasValue
                        ? liquidExpressionREsult.SuccessResult.Value
                        : null);
            }
        }
    }
}
