using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Liquid.NET.Constants;
using Liquid.NET.Rendering;
using Liquid.NET.Symbols;
using Liquid.NET.Tags;
using Liquid.NET.Tags.Custom;
using Liquid.NET.Utils;

namespace Liquid.NET
{

    /// <summary>
    /// Render the AST nodes as a String
    /// </summary>
    public class RenderingVisitor : IASTVisitor
    {
        public event OnRenderingErrorEventHandler RenderingErrorEventHandler;
        public event OnParsingErrorEventHandler ParsingErrorEventHandler;

        private readonly ITemplateContext _templateContext;
        private readonly ConcurrentDictionary<String, int> _counters = new ConcurrentDictionary<string, int>();
        public readonly IList<LiquidError> RenderingErrors = new List<LiquidError>();
        public readonly IList<LiquidError> ParsingErrors = new List<LiquidError>();
        private IfChangedRenderer _isChangedRenderer;

        public bool HasErrors { get { return RenderingErrors.Any() || ParsingErrors.Any();  } }


        private readonly Stack<Action<String>> _accumulators = new Stack<Action<string>>();


        public RenderingVisitor(ITemplateContext templateContext)
        {
            _templateContext = templateContext;
        }

        public void PushTextAccumulator(Action<String> accumulator)
        {
            _accumulators.Push(accumulator);
        }

        public void PopTextAccumulator()
        {
            _accumulators.Pop();
        }

        private void AppendTextToCurrentAccumulator(String str)
        {
            if (!_accumulators.Any())
            {
                throw new InvalidOperationException("Need to call PushTextAppender to capture text.");
            }
            var action = _accumulators.Peek();

            action(str);
        }

        public void Visit(RawBlockTag rawBlockTag)
        {
            AppendTextToCurrentAccumulator(rawBlockTag.Value);
        }

        public void Visit(CustomTag customTag)
        {
            //Console.WriteLine("Looking up Custom Tag " + customTag.TagName);
            var tagType = _templateContext.SymbolTableStack.LookupCustomTagRendererType(customTag.TagName);
            if (tagType != null)
            {
                AppendTextToCurrentAccumulator(RenderCustomTag(customTag, tagType));
                return;
            }

            //Console.WriteLine("Looking up Macro "+ customTag.TagName);
            var macroDescription = _templateContext.SymbolTableStack.LookupMacro(customTag.TagName);
            if (macroDescription != null)
            {
                //Console.WriteLine("...");
                var evalResults =
                    customTag.LiquidExpressionTrees.Select(x => LiquidExpressionEvaluator.Eval(x, _templateContext)).ToList();
                if (evalResults.Any(x => x.IsError))
                {
                    var errors = evalResults.Where(x => x.IsError).Select(x => x.ErrorResult).ToList();
                    foreach (LiquidError error in errors)
                    {
                        RegisterRenderingError(error);
                    }
                    //RenderErrors(errors);
                    return;
                }
                AppendTextToCurrentAccumulator(RenderMacro(macroDescription, evalResults.Select(x => x.SuccessResult)));
                return;
            }

            var err = new LiquidError{Message ="Liquid syntax error: Unknown tag '" + customTag.TagName + "'"};
            //RenderError(err);
            RegisterRenderingError(err);
        }

        private void RenderError(LiquidError liquidError)
        {
            RenderErrors(new List<LiquidError>{liquidError});
        }

        /// <summary>
        /// Render errors inline.
        /// </summary>
        /// <param name="liquidErrors"></param>
        private void RenderErrors(IEnumerable<LiquidError> liquidErrors)
        {
            // TODO: Make this configurable
            //if (_templateContext.RenderErrorsInline)
            //{
                AppendTextToCurrentAccumulator(FormatErrors(liquidErrors));
            //}
        }

        private String FormatErrors(IEnumerable<LiquidError> liquidErrors)
        {            
            return String.Join("; ", liquidErrors.Select(FormatError));
        }

        private static string FormatError(LiquidError x)
        {
            // to remain backwards-compatible, this leaves "Liquid Error:" alone.
            String err = x.Message;
            if (!String.IsNullOrWhiteSpace(x.TokenSource))
            {
                err = x.ToString();
            }
            if (x.Message.IndexOf("Liquid error") >= 0)
            {
                return err;
            }
            else
            {
                return "ERROR: " + err;
            }

        }

        // ReSharper disable once UnusedParameter.Local
        //private void RegisterRenderingError(String message, IASTNode node)
        //{ 
            
       // }

        /// <summary>
        /// Save the rendering error and notify listeners that an error
        /// has occurred.
        /// </summary>
        /// <param name="error"></param>
        internal void RegisterRenderingError(LiquidError error)
        {
            RenderingErrors.Add(error);
            RenderError(error); // write it inline
            OnRenderingErrorEventHandler(error);
        }

        internal void RegisterParsingError(LiquidError error)
        {
            ParsingErrors.Add(error);
            RenderError(error); // write it inline
            OnParsingErrorEventHandler(error);
        }

        private string RenderMacro(MacroBlockTag macroBlockTag, IEnumerable<Option<ILiquidValue>> args)
        {
            var macroRenderer = new MacroRenderer();
            var expressionConstant = (ILiquidValue)macroRenderer.Render(this, macroBlockTag, _templateContext, args.ToList());
            return ValueCaster.RenderAsString(expressionConstant);
        }

        private String RenderCustomTag(CustomTag customTag, Type tagType)
        {
            var tagRenderer = CustomTagRendererFactory.Create(tagType);
            String result = "";
            EvalExpressions(customTag.LiquidExpressionTrees,
                args => result = tagRenderer.Render(_templateContext, args.ToList()).StringVal,
                errors => result = FormatErrors(errors));
            return result;

       }


    

        public void Visit(CustomBlockTag customBlockTag)
        {
            var tagType = _templateContext.SymbolTableStack.LookupCustomBlockTagRendererType(customBlockTag.TagName);
            var tagRenderer = CustomBlockTagRendererFactory.Create(tagType);
            if (tagRenderer == null)
            {
                
                var message = "Liquid syntax error: Unknown tag '" + customBlockTag.TagName + "'";
                RegisterRenderingError(new LiquidError { Message = message });               
                return;
            }

            EvalExpressions(customBlockTag.LiquidExpressionTrees,
               args => AppendTextToCurrentAccumulator(tagRenderer.Render(this, _templateContext, customBlockTag.LiquidBlock, args.ToList()).StringVal),
               errors => AppendTextToCurrentAccumulator(FormatErrors(errors)));

        }

        public void Visit(CycleTag cycleTag)
        {
            if (cycleTag.GroupNameExpressionTree == null)
            {
                AppendTextToCurrentAccumulator(GetNextCycleText(groupName: null, cycleTag: cycleTag));
            }
            else
            {
                // figure out the group name if any, otherwise use null.
                LiquidExpressionEvaluator.Eval(cycleTag.GroupNameExpressionTree, _templateContext)
                    .WhenSuccess(
                        x =>
                            AppendTextToCurrentAccumulator(
                                GetNextCycleText(
                                    groupName: x.HasValue ? ValueCaster.RenderAsString(x.Value) : null, 
                                    cycleTag: cycleTag)))
                    .WhenError(err => {
                        RegisterRenderingError(err);
                        //RenderError(err);
                    });
            }
        }

        /// <summary>
        /// Side effect: state is managed in the _counters dictionary.
        /// </summary>
        /// <returns></returns>
        private String GetNextCycleText(String groupName, CycleTag cycleTag)
        {

            int currentIndex;
            // Create a like dictionary key entry to keep track of this declaration.  THis takes the variable
            // names (not the eval-ed variables) or literals and concatenates them together.
            var key = "cycle_" + groupName + "_" + String.Join("|", cycleTag.CycleList.Select(x => x.Data.Expression.ToString()));
            

            while (true)
            {
                currentIndex = _counters.GetOrAdd(key, 0);
                var newindex = (currentIndex + 1) % cycleTag.Length;

                // fails if updated concurrently by someone else.
                if (_counters.TryUpdate(key, newindex, currentIndex))
                {
                    break;
                }
            }

            String result = "";
            var currentElement = cycleTag.ElementAt(currentIndex);
            LiquidExpressionEvaluator.Eval(currentElement, _templateContext)
                .WhenSuccess(x => result = ValueCaster.RenderAsString(LiquidExpressionEvaluator.Eval(currentElement, _templateContext).SuccessResult.Value))
                .WhenError(err => result = FormatErrors(new List<LiquidError> {err}));

            return result;

        }

        public void Visit(AssignTag assignTag)
        {

            LiquidExpressionEvaluator.Eval(assignTag.LiquidExpressionTree, _templateContext)
                .WhenSuccess(x => _templateContext.SymbolTableStack.DefineGlobal(assignTag.VarName, x))
                .WhenError( err => 
                    {
                        RegisterRenderingError(err);
                        //RenderError(err);
                    });

        }

        public void Visit(CaptureBlockTag captureBlockTag)
        {
            String capturedText = "";
            //var hiddenVisitor = new RenderingVisitor(_astRenderer, _templateContext, str => capturedText += str);
            PushTextAccumulator(str => capturedText += str);
            StartWalking(captureBlockTag.RootContentNode);            
            _templateContext.SymbolTableStack.DefineGlobal(captureBlockTag.VarName, LiquidString.Create(capturedText) );
            PopTextAccumulator();
        }

        /// <summary>
        /// Pre-decrement the counter, i.e. --i
        /// </summary>
        public void Visit(DecrementTag decrementTag)
        {
            int currentIndex;
            var key = decrementTag.VarName;

            while (true)
            {
                currentIndex = _counters.GetOrAdd(key, 0);
                var newindex = (currentIndex - 1);
                if (_counters.TryUpdate(key, newindex, currentIndex))
                {
                    currentIndex = newindex;
                    break;
                }

            }

            AppendTextToCurrentAccumulator(currentIndex.ToString());
        }

        /// <summary>
        /// Post-increment the counter i.e. i++
        /// </summary>
        /// <param name="incrementTag"></param>
        public void Visit(IncrementTag incrementTag)
        {
            int currentIndex;
            var key = incrementTag.VarName;

            while (true)
            {
                currentIndex = _counters.GetOrAdd(key, 0);
                var newindex = (currentIndex + 1);                
                if (_counters.TryUpdate(key, newindex, currentIndex))
                {
                    break;
                }
            }

            AppendTextToCurrentAccumulator(currentIndex.ToString());
        }

        public void Visit(IncludeTag includeTag)
        {
            var includeRenderer = new IncludeRenderer(this);
            includeRenderer.Render(includeTag, _templateContext);
 
        }

        public void Visit(ForBlockTag forBlockTag)
        {
            new ForRenderer(this).Render(forBlockTag, _templateContext);
        }

        public void Visit(IfThenElseBlockTag ifThenElseBlockTag)
        {

            // find the first place where the expression tree evaluates to true (i.e. which of the if/elsif/else clauses)
            // This ignores "eval" errors in clauses.

            var match = ifThenElseBlockTag.IfElseClauses.FirstOrDefault(
                                expr => LiquidExpressionResultIsTrue(LiquidExpressionEvaluator.Eval(expr.LiquidExpressionTree, _templateContext)));
            if (match != null)
            {
                StartWalking(match.LiquidBlock); // then render the contents
            }
        }

        private static bool LiquidExpressionResultIsTrue(LiquidExpressionResult result)
        {
            return result.IsSuccess && result.SuccessResult.HasValue && result.SuccessResult.Value.IsTrue;
        }

        public void Visit(CaseWhenElseBlockTag caseWhenElseBlockTag)
        {
            var valueToMatchResult = LiquidExpressionEvaluator.Eval(caseWhenElseBlockTag.LiquidExpressionTree, _templateContext);
            //Console.WriteLine("Value to Match: "+valueToMatch);
            if (valueToMatchResult.IsError)
            {
                RegisterRenderingError(valueToMatchResult.ErrorResult);
                //RenderError(valueToMatchResult.ErrorResult);
                return;
            }

            var match =
                caseWhenElseBlockTag.WhenClauses.FirstOrDefault(
                    expr =>
                        // Take the valueToMatch "Case" expression result value
                        // and check if it's equal to the expr.GroupNameExpressionTree expression.
                        // THe "EasyValueComparer" is supposed to match stuff fairly liberally,
                        // though it doesn't cast values---probably it should.

                        expr.LiquidExpressionTree.Any(val =>
                            new EasyOptionComparer().Equals(valueToMatchResult.SuccessResult,
                                        LiquidExpressionEvaluator.Eval(val, _templateContext).SuccessResult)));


            if (match != null) // found match
            {
                StartWalking(match.LiquidBlock); // so eval + render the HTML
            }
            else if (caseWhenElseBlockTag.HasElseClause)
            {
                StartWalking(caseWhenElseBlockTag.ElseClause.LiquidBlock);
            }
        }

        public void Visit(ContinueTag continueTag)
        {
            throw new ContinueException();
        }

        public void Visit(BreakTag breakTag)
        {
            throw new BreakException();
        }

        public void Visit(MacroBlockTag macroBlockTag)
        {            
            _templateContext.SymbolTableStack.DefineMacro(macroBlockTag.Name, macroBlockTag);
        }

//        public void Visit(ErrorNode errorNode)
//        {
//            AppendTextToCurrentAccumulator(errorNode.LiquidError.ToString());
//        }

        public void Visit(IfChangedBlockTag ifChangedBlockTag)
        {
            // This maintains state, so there's only one.
            if (_isChangedRenderer == null)
            {
                _isChangedRenderer = new IfChangedRenderer(this);
            }
            AppendTextToCurrentAccumulator(_isChangedRenderer.Next(ifChangedBlockTag.UniqueId, ifChangedBlockTag.LiquidBlock));

        }

        public void Visit(TableRowBlockTag tableRowBlockTag)
        {
            new TableRowRenderer(this)
                .Render(tableRowBlockTag, _templateContext, AppendTextToCurrentAccumulator);
        }

        public void Visit(RootDocumentNode rootDocumentNode)
        {
           // noop
        }

        public void Visit(LiquidExpressionTree liquidExpressionTree)
        {
            var result = LiquidExpressionEvaluator.Eval(liquidExpressionTree, _templateContext);

            result.WhenSuccess(success => success.WhenSome(x => AppendTextToCurrentAccumulator(Render(x))))
                .WhenError(error =>
                {
                    RegisterRenderingError(error);
                    //RenderingErrors.Add(error);
                    //RenderError(error);
                });
        }

        public String Render(ILiquidValue result)
        {
            return ValueCaster.RenderAsString(result);
        }

        public void EvalExpressions(
            IEnumerable<TreeNode<LiquidExpression>> expressionTrees,
            Action<IEnumerable<Option<ILiquidValue>>> successAction = null,
            Action<IEnumerable<LiquidError>> failureAction = null)
        {
            var evaledArgs = expressionTrees.Select(x => LiquidExpressionEvaluator.Eval(x, _templateContext)).ToList();
            if (evaledArgs.Any(x => x.IsError))
            {
                if (failureAction != null)
                {
                    failureAction(evaledArgs.Where(x => x.IsError).Select(x => x.ErrorResult));
                }
            }
            else
            {
                if (successAction != null)
                {
                    successAction(evaledArgs.Select(x => x.SuccessResult));
                }
            }

        }

        public void StartWalking(TreeNode<IASTNode> rootNode)
        {
            if (!_accumulators.Any())
            {
                throw new InvalidOperationException("There is no current accumulator.");
            }
            rootNode.Data.Accept(this);
            rootNode.Children.ForEach(StartWalking);
        }

        public void StartWalking(
            TreeNode<IASTNode> rootNode, 
            Action<String> accumulator)
        {
            PushTextAccumulator(accumulator);
            StartWalking(rootNode);
            PopTextAccumulator();
        }

        protected void OnRenderingErrorEventHandler(LiquidError args)
        {
            var handler = RenderingErrorEventHandler;
            if (handler != null) handler(this, args);
        }

        protected void OnParsingErrorEventHandler(LiquidError liquiderror)
        {
            var handler = ParsingErrorEventHandler;
            if (handler != null) handler(liquiderror);
        }
    }

    public delegate void OnRenderingErrorEventHandler(Object sender, LiquidError args);

    public class ContinueException : Exception { }

    public class BreakException : Exception { }



}
