using System;
using System.Linq;
using Liquid.NET.Filters;
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
            globalScope.DefineFilter<TruncateWordsFilter>("truncate_words");
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





/*
        //private readonly SymbolTable _globalSymbolTable;

        private readonly BufferedTokenStream _tokenStream;
        public event RawArgsEventHandler RawArgsEvent;

        private ObjectSymbol _currentObjectSymbol;
        private readonly IList<FilterSymbol> _currentFilterSymbols = new List<FilterSymbol>();
        private readonly IList<Action<TemplateContext>> _templateActions = new List<Action<TemplateContext>>();

        private readonly TokenStreamRewriter _tokenStreamRewriter;

        public LiquidEvaluator(BufferedTokenStream tokenStream)
        {
            _tokenStream = tokenStream;
            _tokenStreamRewriter = new TokenStreamRewriter(tokenStream);
        }

        #region Output / Filter Chain
        /// <summary>
        /// Enter the {{ ... }} filter, and delete the "{{" and the "}}" tokens.
        /// </summary>
        /// <param name="context"></param>
        public override void EnterOutputmarkup(LiquidParser.OutputmarkupContext context)
        {
            base.EnterOutputmarkup(context);
            _tokenStreamRewriter.Delete(context.Start); // Delete the opening
            _tokenStreamRewriter.Delete(context.Stop); // and closing braces
        }

        /// <summary>
        /// Save the filter reference
        /// </summary>
        /// <param name="context"></param>
        public override void EnterFiltername(LiquidParser.FilternameContext context)
        {
            base.EnterFiltername(context);
            Console.WriteLine("CREATING FILTER " + context.GetText());
            _currentFilterSymbols.Add(new FilterSymbol(context.GetText()));
            ;
        }

        public override void EnterFilterarg(LiquidParser.FilterargContext context)
        {
            base.EnterFilterarg(context);
            var arg = new ObjectSymbol(context.GetText(), CreateObjectType(context));
            _currentFilterSymbols.Last().AddArg(ExpressionFactory.CreateExpression(arg));
        }


        /// <summary>
        /// Save the raw filter argument string.  Liquid says that a filter has
        /// one argument, so this is it.
        /// </summary>
        /// <param name="context"></param>
        public override void EnterFilterargs(LiquidParser.FilterargsContext context)
        {
            base.EnterFilterargs(context);
            var originalTokens = _tokenStream.GetTokens(context.Start.TokenIndex, context.Stop.TokenIndex);
            // This removes extra whitespace---which may not be what we want...?
            // May need to figure out how to put the whitespace in the hidden stream for this only.
            String normalizedArgs = String.Join(" ", originalTokens.Select(x => x.Text)); 
            Console.WriteLine("Saving raw args " + normalizedArgs);
            InvokeRawArgsEvent(_currentFilterSymbols.Last().Name, normalizedArgs);
            _currentFilterSymbols.Last().RawArgs = normalizedArgs;
        }


        public override void EnterOutputexpression(LiquidParser.OutputexpressionContext context)
        {
            base.EnterOutputexpression(context);
            LiquidParser.ObjectContext objContext = context.@object();
            //if (objContext == null) return; // can this actually happen?
            //context.@object.
            Console.WriteLine("CREATING OBJECT >" + objContext.GetText()+"<");
            _currentObjectSymbol = new ObjectSymbol(objContext.GetText(), CreateObjectType(objContext));
        }

        public override void ExitOutputexpression(LiquidParser.OutputexpressionContext context)
        {
            var objectExpression = ExpressionFactory.CreateExpression(_currentObjectSymbol);
            
            if (objectExpression == null)
            {
                // TODO: defer this to the _templateActions, don't do it inline
                _tokenStreamRewriter.Replace(context.Start, context.Stop, "Unrecognized object " + context.GetText());
            }
            else
            {
                //var stringCreationFunction = CreateTransformationFunction(_currentObjectSymbol, _currentFilterSymbols);

                _templateActions.Add(ctx => 
                {
                    // TODO: pass in ctx 
                    //Console.WriteLine("Replacing "+context.Start.Channel)
                    _tokenStream.GetHiddenTokensToLeft(context.Start.TokenIndex, Lexer.Hidden);
                    //_tokenStream.
                    //Console.WriteLine("START: " + context.Start.Text);
                    //Console.WriteLine("END: " + context.Stop.Text);
                    //_tokenStreamRewriter.Replace(context.Start, context.Stop, stringCreationFunction());
                });
               
            }
            CleanUpStateData();
            

        }

        #endregion

      


        private ObjectSymbolType CreateObjectType(ParserRuleContext objContext)
        {
            if (objContext.GetToken(LiquidParser.BOOLEAN, 0) != null)
            {
                return ObjectSymbolType.Boolean;
            } 
            else if (objContext.GetToken(LiquidParser.NUMBER, 0) != null)
            {
                return ObjectSymbolType.Number;
            }
            else if (objContext.GetToken(LiquidParser.STRING, 0) != null)
            {
                return ObjectSymbolType.String;
            }
            //else if (objContext.GetToken(LiquidParser.VARIABLE, 0) != null)
            //{
            //return ObjectSymbolType.Variable;
            //}
            else if (objContext.GetToken(LiquidParser.ARRAY, 0) != null)
            {
                return ObjectSymbolType.Array;
            }
            else return ObjectSymbolType.Unknown;
        }


        private void CleanUpStateData()
        {
            _currentObjectSymbol = null;
            _currentFilterSymbols.Clear();
        }

        public void InvokeRawArgsEvent(String filterName, String rawText)
        {
            RawArgsEventHandler handler = RawArgsEvent;
            if (handler != null)
            {
                handler(filterName, rawText);
            }
        }



        // TODO: Move these elsewhere

        /*  public string GetText(TemplateContext context)
  {
      // todo: this needs to be rearranged so that the data is pulled in here
      ExecuteTransformationActions(context);
      return _tokenStreamRewriter.GetText();
  }
  private void ExecuteTransformationActions(TemplateContext context)
  {
      foreach (var fn in _templateActions)
      {
          fn(context);
      }
  }#1#

        //        public string GetDebugTree(TemplateContext context)

        //        {

        //            ExecuteTransformationActions(context);

        //            //return _tokenStream.;

        //        }

        //        // The func will need more info like the context, later.

        //        // This transforms one object/filter chain.

        //        private Func<String> CreateTransformationFunction(ObjectSymbol currentObjectSymbol, IList<FilterSymbol> currentFilterSymbols)

        //        {

        //            var filterFactory = new FilterFactory(_globalSymbolTable.FilterRegistry);

        //            var objectExpression = ExpressionFactory.CreateExpression(currentObjectSymbol);

        //            //_globalSymbolTable.FilterRegistry.Find(filterSymbol.Name)

        //            var filterChain = currentFilterSymbols.Select(filterFactory.CreateFilter);

        //            var filterChainWithCasting = FilterFactory.InterpolateCastFilters(objectExpression, filterChain);

        //

        //            return () =>

        //            {

        //                var current = filterChainWithCasting.Aggregate(

        //                    objectExpression, 

        //                    (current1, filter1) => current1.Bind(filter1.Apply));

        //

        //                return current.HasError ? current.ErrorMessage : current.Render();

        //            };

        //

        //        }

*/
    }
}


