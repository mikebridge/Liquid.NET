using System;
using System.Linq;
using Liquid.NET.Filters;
using Liquid.NET.Filters.Array;
using Liquid.NET.Filters.Math;
using Liquid.NET.Filters.Strings;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET
{
    public class LiquidEvaluator
    {

        public String Render(TemplateContext context, LiquidAST liquidAst)
        {
            //Stack<SymbolTable> symbolStack = new Stack<SymbolTable>();
            var symbolTableStack = new SymbolTableStack(context);
            SymbolTable globalScope = new SymbolTable();
            // TODO: The filters should be injected
            AddGlobalFilters(globalScope);


            symbolTableStack.Push(globalScope);
            
            // TODO: Merge the context in here.
            return EvalTree(symbolTableStack, liquidAst);
        }

        private static void AddGlobalFilters(SymbolTable globalScope)
        {
            globalScope.DefineFilter<UpCaseFilter>("upcase");
            globalScope.DefineFilter<DownCaseFilter>("downcase");
            globalScope.DefineFilter<PlusFilter>("plus");
            globalScope.DefineFilter<RemoveFilter>("remove");
            globalScope.DefineFilter<ReplaceFilter>("replace");
            globalScope.DefineFilter<LookupFilter>("lookup");
            globalScope.DefineFilter<AppendFilter>("append");
            globalScope.DefineFilter<JoinFilter>("join");


            // TODO: Register these separately
            globalScope.DefineFilter<CamelCaseFilter>("camelcase");
            globalScope.DefineFilter<CapitalizeFilter>("capitalize");
            globalScope.DefineFilter<EscapeFilter>("escape");
            globalScope.DefineFilter<HandleizeFilter>("handleize");
            globalScope.DefineFilter<LStripFilter>("lstrip");
            globalScope.DefineFilter<Md5Filter>("md5");
            globalScope.DefineFilter<NewlineToBrFilter>("newline_to_br");
            globalScope.DefineFilter<PluralizeFilter>("pluralize");
            globalScope.DefineFilter<PrependFilter>("prepend");
            globalScope.DefineFilter<RemoveFirstFilter>("remove_first");
            globalScope.DefineFilter<StripFilter>("strip");
            globalScope.DefineFilter<StripHtmlFilter>("strip_html");
            globalScope.DefineFilter<StripNewlinesFilter>("strip_newlines");
            globalScope.DefineFilter<ReplaceFirstFilter>("replace_first");
            globalScope.DefineFilter<TruncateFilter>("truncate");
            globalScope.DefineFilter<RStripFilter>("rstrip");
            globalScope.DefineFilter<LStripFilter>("lstrip");
            globalScope.DefineFilter<StripFilter>("strip");
            globalScope.DefineFilter<TruncateWordsFilter>("truncatewords");
            globalScope.DefineFilter<SliceFilter>("slice");
            globalScope.DefineFilter<SplitFilter>("split");
            globalScope.DefineFilter<UniqFilter>("uniq");
            globalScope.DefineFilter<UrlEscapeFilter>("url_escape");
            globalScope.DefineFilter<UrlParamEscapeFilter>("url_param_escape");
        }

        private string EvalTree(SymbolTableStack symbolStack, LiquidAST liquidAst)
        {
            var renderingVisitor = new RenderingVisitor(this, symbolStack);
            StartVisiting(renderingVisitor, liquidAst.RootNode);
            return renderingVisitor.Text;
        }

        public void StartVisiting(IASTVisitor visitor, TreeNode<IASTNode> rootNode)
        {
            rootNode.Data.Accept(visitor);
            rootNode.Children.ForEach(child => StartVisiting(visitor, child));
        }




    }
}


