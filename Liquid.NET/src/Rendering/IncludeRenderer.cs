using System;
using System.Collections.Generic;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Tags;
using Liquid.NET.Utils;

namespace Liquid.NET.Rendering
{
    public class IncludeRenderer
    {
        public static string LOCALREGISTRY_FILE_KEY = "include";

        private readonly RenderingVisitor _renderingVisitor;

        public IncludeRenderer(RenderingVisitor renderingVisitor)
        {
            _renderingVisitor = renderingVisitor;
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
            templateContext.SymbolTableStack.DefineLocalRegistry(LOCALREGISTRY_FILE_KEY, virtualFileName);
            LiquidAST snippetAst;
            try
            {
                //snippetAst = GenerateSnippetAst(snippet);
                snippetAst = templateContext.ASTGenerator(snippet);
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
                if (forExpressionOption.SuccessResult.Value is LiquidHash) // it seems to render as a single element if it's a dictionary.
                {
                    var localBlockScope = new SymbolTable();
                    DefineLocalVariables(templateContext, localBlockScope, includeTag.Definitions);

                    var exprValue = LiquidExpressionEvaluator.Eval(includeTag.ForExpression, templateContext);
                    localBlockScope.DefineLocalVariable(virtualFileName, exprValue.SuccessResult);

                    RenderWithLocalScope(templateContext, localBlockScope, snippetAst.RootNode);
                }
                else
                {
                    //LiquidCollection array = ValueCaster.Cast<ILiquidValue, LiquidCollection>(forExpressionOption.SuccessResult.Value);
                    var arrayResult = ValueCaster.Cast<ILiquidValue, LiquidCollection>(forExpressionOption.SuccessResult.Value);
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
                    foreach (Option<ILiquidValue> val in arrayResult.SuccessValue<LiquidCollection>())
                    {
                        var localBlockScope = new SymbolTable();
                        DefineLocalVariables(templateContext, localBlockScope, includeTag.Definitions);
                        localBlockScope.DefineLocalVariable(virtualFileName, val);
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
                    localBlockScope.DefineLocalVariable(virtualFileName, withExpression.SuccessResult);
                }
                RenderWithLocalScope(templateContext, localBlockScope, snippetAst.RootNode);
            }
           
                       
        }

        public static LiquidAST GenerateSnippetAst(string snippet)
        {
            return new CachingLiquidASTGenerator(new LiquidASTGenerator()).Generate(snippet);
            //return new LiquidASTGenerator().Generate(snippet);
        }

        private void RenderWithLocalScope(ITemplateContext templateContext, SymbolTable localBlockScope, TreeNode<IASTNode> rootNode)
        {
            templateContext.SymbolTableStack.Push(localBlockScope);
            _renderingVisitor.StartWalking(rootNode);
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
                    liquidExpressionREsult.SuccessResult);
            }
        }
    }
}
