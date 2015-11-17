using System;
using System.Collections.Generic;
using System.Linq;

using Liquid.NET.Constants;
using Liquid.NET.Expressions;
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

        public void Render(IncludeTag includeTag,
                ITemplateContext templateContext)
        {
            if (templateContext.FileSystem == null)
            {
                AddRenderingErrorToResult(new LiquidError{Message = " ERROR: FileSystem is not defined"});
                return;
            }

            String virtualFileName = null;
            new LiquidExpressionVisitor(templateContext).Traverse(includeTag.VirtualFileExpression).Result

            //LiquidExpressionEvaluator.Eval(includeTag.VirtualFileExpression, templateContext)
                .WhenError(AddRenderingErrorToResult)
                .WhenSuccess(result => { virtualFileName = ValueCaster.RenderAsString(result); });

            if (virtualFileName == null) { return; }

            //virtualFileName = ValueCaster.RenderAsString(virtualFilenameVar.SuccessResult.Value);

            String snippet = templateContext.FileSystem.Include(templateContext, virtualFileName);
            
            templateContext.SymbolTableStack.DefineLocalRegistry(LOCALREGISTRY_FILE_KEY, virtualFileName);

            RenderSnippet(includeTag, templateContext, snippet, virtualFileName);
        }

        private void RenderSnippet(IncludeTag includeTag, ITemplateContext templateContext, String snippet,
            String virtualFileName)
        {
            var snippetAstTry = CreateAstFromSnippet(templateContext, snippet, virtualFileName);
            if (snippetAstTry.IsLeft)
            {
                foreach (var err in snippetAstTry.Left)
                {
                    AddParsingErrorToResult(err);
                }
                return;
            }
            var snippetAst = snippetAstTry.Right;
            //zzz
            if (includeTag.ForExpression != null)
            {
                LiquidExpressionEvaluator.Eval(includeTag.ForExpression, templateContext)
                    .WhenError(AddRenderingErrorToResult)
                    .WhenSuccess(result =>
                    {
                        if (result.Value is LiquidHash)
                        {
                            // it seems to render as a single element if it's a dictionary.
                            RenderFromLiquidHash(includeTag, templateContext, virtualFileName, snippetAst);
                        }
                        else
                        {
                            RenderFromLiquidExpression(includeTag, templateContext, virtualFileName, result, snippetAst);
                        }
                    });
            }
            else
            {
                RenderIncludeBlock(includeTag, templateContext, virtualFileName, snippetAst);
            }
        }

        private void RenderIncludeBlock(IncludeTag includeTag, ITemplateContext templateContext, String virtualFileName,
            LiquidAST snippetAst)
        {
            Action<SymbolTable> action = localBlockScope =>
            {
                if (includeTag.WithExpression != null)
                {
                    var withExpression = LiquidExpressionEvaluator.Eval(includeTag.WithExpression, templateContext);
                    localBlockScope.DefineLocalVariable(virtualFileName, withExpression.SuccessResult);
                }
            };
            RenderBlock(includeTag, templateContext, snippetAst, action);
        }

        private void RenderFromLiquidExpression(IncludeTag includeTag, ITemplateContext templateContext, String virtualFileName,
            Option<ILiquidValue> forExpressionOption, LiquidAST snippetAst)
        {
            ValueCaster.Cast<ILiquidValue, LiquidCollection>(forExpressionOption.Value)
                .WhenError(AddRenderingErrorToResult)
                .WhenSuccess(result =>
                {
                    foreach (Option<ILiquidValue> val in (LiquidCollection) result.Value)
                    {
                        var val1 = val;
                        RenderBlock(includeTag, templateContext, snippetAst, localBlockScope => localBlockScope.DefineLocalVariable(virtualFileName, val1));
                    }
                });
        }

        private void RenderFromLiquidHash(IncludeTag includeTag, ITemplateContext templateContext, String virtualFileName,
            LiquidAST snippetAst)
        {
            Action<SymbolTable> action = localBlockScope => localBlockScope.DefineLocalVariable(
                virtualFileName, LiquidExpressionEvaluator.Eval(includeTag.ForExpression, templateContext).SuccessResult);
            
            RenderBlock(includeTag, templateContext, snippetAst, action);
        }


        private static Either<IList<LiquidError>, LiquidAST> CreateAstFromSnippet(ITemplateContext templateContext, String snippet,
            String virtualFileName)
        {
            IList<LiquidError> parsingErrors = new List<LiquidError>();
            var snippetAst = templateContext.ASTGenerator(snippet, parsingErrors.Add);

            foreach (var error in parsingErrors.Where(error => String.IsNullOrEmpty(error.TokenSource)))
            {
                error.TokenSource = virtualFileName;
            }
            return parsingErrors.Any() ? 
                Either.Left<IList<LiquidError>, LiquidAST>(parsingErrors) : 
                Either.Right<IList<LiquidError>, LiquidAST>(snippetAst);
        }

        private void AddRenderingErrorToResult(LiquidError errorResult)
        {          
            _renderingVisitor.RegisterRenderingError(errorResult);
        }

        private void AddParsingErrorToResult(LiquidError errorResult)
        {
            _renderingVisitor.RegisterParsingError(errorResult);
        }


//        public static LiquidAST GenerateSnippetAst(string snippet)
//        {
//            return new CachingLiquidASTGenerator(new LiquidASTGenerator()).Generate(snippet);
//            //return new LiquidASTGenerator().Generate(snippet);
//        }

        private void RenderWithLocalScope(ITemplateContext templateContext, SymbolTable localBlockScope, TreeNode<IASTNode> rootNode)
        {
            templateContext.SymbolTableStack.Push(localBlockScope);
            _renderingVisitor.StartWalking(rootNode);
            templateContext.SymbolTableStack.Pop();
        }

        private static void DefineLocalVariables(
            ITemplateContext templateContext,
            SymbolTable localBlockScope,
            IDictionary<string, TreeNode<IExpressionDescription>> definitions)
        {
            foreach (var def in definitions)
            {
                var def1 = def;
                LiquidExpressionEvaluator.Eval(def.Value, templateContext)
                    //.WhenError( err =>   //TODO: Is this necessary?
                    .WhenSuccess( result => 
                        localBlockScope.DefineLocalVariable(def1. Key,result));
            }
        }

        private void RenderBlock(
            IncludeTag includeTag,
            ITemplateContext templateContext,
            LiquidAST snippetAst,
            Action<SymbolTable> renderAction)
        {
            var localBlockScope = new SymbolTable();
            DefineLocalVariables(templateContext, localBlockScope, includeTag.Definitions);

            renderAction(localBlockScope);

            RenderWithLocalScope(templateContext, localBlockScope, snippetAst.RootNode);
        }


    }
}
