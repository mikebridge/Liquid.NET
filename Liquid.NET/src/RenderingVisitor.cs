using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Liquid.NET.Constants;
using Liquid.NET.Expressions;
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
        //private String _result = "";
        
        private readonly ITemplateContext _templateContext;
        private readonly ConcurrentDictionary<String, int> _counters = new ConcurrentDictionary<string, int>();
        public readonly IList<LiquidError> Errors = new List<LiquidError>();
        private IfChangedRenderer _isChangedRenderer;

        public bool HasErrors { get { return Errors.Any();  } }

        private readonly Stack<Action<String>> _accumulators = new Stack<Action<string>>();

        //public RenderingVisitor(LiquidASTRenderer astRenderer, SymbolTableStack symbolTableStack)
        public RenderingVisitor(
            ITemplateContext templateContext)
        {
            _templateContext = templateContext;
            //_accumulators.Push(accumulator);
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
                //var evalResult = LiquidExpressionEvaluator.Eval(customTag.LiquidExpressionTrees, _templateContext.SymbolTableStack);
                var evalResults =
                    customTag.LiquidExpressionTrees.Select(x => LiquidExpressionEvaluator.Eval(x, _templateContext)).ToList();
                if (evalResults.Any(x => x.IsError))
                {
                    RenderErrors(evalResults);
                    return;
                }
                AppendTextToCurrentAccumulator(RenderMacro(macroDescription, evalResults.Select(x => x.SuccessResult)));
                return;
            }
            //_result += " ERROR: There is no macro or tag named "+  customTag.TagName+ " ";
            AddError("Liquid syntax error: Unknown tag '" + customTag.TagName + "'", customTag);
            //_result += "Liquid syntax error: Unknown tag '"+customTag.TagName+"'";
        }

        private void RenderError(LiquidError liquidError)
        {
            AppendTextToCurrentAccumulator(FormatErrors(new List<LiquidError>{liquidError}));
        }
        private void RenderErrors(IEnumerable<LiquidError> liquidErrors)
        {
            AppendTextToCurrentAccumulator(FormatErrors(liquidErrors));
        }

        private String FormatErrors(IEnumerable<LiquidError> liquidErrors)
        {
            //return "ERROR: " + String.Join("; ", liquidErrors.Select(x => x.Message));
            return String.Join("; ", liquidErrors.Select(x => x.Message));
        }

        private void RenderErrors(IEnumerable<LiquidExpressionResult> liquidErrors)
        {
            var errors = liquidErrors.Where(x => x.IsError).Select(x => x.ErrorResult);
            RenderErrors(errors);
        }

        // ReSharper disable once UnusedParameter.Local
        private void AddError(String message, IASTNode node)
        { 
            Errors.Add(new LiquidError{Message = message});
        }

        private string RenderMacro(MacroBlockTag macroBlockTag, IEnumerable<Option<IExpressionConstant>> args)
        {
            var macroRenderer = new MacroRenderer();
            //IList<LiquidError> macroErrors = new List<LiquidError>();
            //var macro = ValueCaster.RenderAsString((IExpressionConstant) macroRenderer.Render(macroBlockTag, _templateContext, args.ToList(), macroErrors));
            var macro = ValueCaster.RenderAsString((IExpressionConstant)macroRenderer.Render(this, macroBlockTag, _templateContext, args.ToList()));
            //foreach (var error in macroErrors)
            //{
                //Errors.Add(error);
            //}
            return macro;
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
                AddError("Liquid syntax error: Unknown tag '" + customBlockTag.TagName + "'", customBlockTag);              
                return;
            }

            EvalExpressions(customBlockTag.LiquidExpressionTrees,
               args => AppendTextToCurrentAccumulator(tagRenderer.Render(this, _templateContext, customBlockTag.LiquidBlock, args.ToList()).StringVal),
               errors => AppendTextToCurrentAccumulator(FormatErrors(errors)));

        }

        public void Visit(CycleTag cycleTag)
        {
            String groupName = null;
            if (cycleTag.GroupNameExpressionTree != null)
            {
                LiquidError error = null;
                // figure out the group name if any, otherwise use null.
                LiquidExpressionEvaluator.Eval(cycleTag.GroupNameExpressionTree, _templateContext)
                    .WhenSuccess(x => groupName = x.HasValue ? ValueCaster.RenderAsString(x.Value) : null)
                    .WhenError(x => error = x);
                if (error!=null)
                {
                    RenderError(error);
                    return;
                } 

            }
            AppendTextToCurrentAccumulator(GetNextCycleText(groupName, cycleTag));
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
            if (assignTag.LiquidExpressionTree == null)
            {
                _templateContext.SymbolTableStack.DefineGlobal(assignTag.VarName, null);
            }
            else
            {
                LiquidExpressionEvaluator.Eval(assignTag.LiquidExpressionTree, _templateContext)
                    .WhenSuccess(x => x.WhenSome(some => _templateContext.SymbolTableStack.DefineGlobal(assignTag.VarName, some))
                        .WhenNone(() => _templateContext.SymbolTableStack.DefineGlobal(assignTag.VarName, null)))
                    .WhenError(RenderError);
            }

        }

        public void Visit(CaptureBlockTag captureBlockTag)
        {
            String capturedText = "";
            //var hiddenVisitor = new RenderingVisitor(_astRenderer, _templateContext, str => capturedText += str);
            PushTextAccumulator(str => capturedText += str);
            StartWalking(captureBlockTag.RootContentNode);            
            _templateContext.SymbolTableStack.DefineGlobal(captureBlockTag.VarName, new StringValue(capturedText) );
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
                                expr => {
                                    var result = LiquidExpressionEvaluator.Eval(expr.LiquidExpressionTree, _templateContext);
                                    return result.IsSuccess && result.SuccessResult.HasValue && result.SuccessResult.Value.IsTrue;
                                });
            if (match != null)
            {
                StartWalking(match.LiquidBlock); // then render the contents
            }
        }

        public void Visit(CaseWhenElseBlockTag caseWhenElseBlockTag)
        {
            var valueToMatchResult = LiquidExpressionEvaluator.Eval(caseWhenElseBlockTag.LiquidExpressionTree, _templateContext);
            //Console.WriteLine("Value to Match: "+valueToMatch);
            if (valueToMatchResult.IsError)
            {
                RenderError(valueToMatchResult.ErrorResult);
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


            if (match != null)
            {
                StartWalking(match.LiquidBlock); // then eval + render the HTML
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

        public void Visit(VariableReference variableReference)
        {
            variableReference.Eval(_templateContext, new List<Option<IExpressionConstant>>());
        }

        public void Visit(StringValue stringValue)
        {          
           AppendTextToCurrentAccumulator(Render(stringValue)); 
        }

        /// <summary>
        /// Process the object / filter chain
        /// </summary>
        /// <param name="liquidExpression"></param>
        public void Visit(LiquidExpression liquidExpression)
        {
            //Console.WriteLine("Visiting Object Expression ");
            LiquidExpressionEvaluator.Eval(liquidExpression, new List<Option<IExpressionConstant>>(), _templateContext)
                .WhenSuccess(x => x.WhenSome(some => AppendTextToCurrentAccumulator(Render(x.Value)))
                                   .WhenNone(() => AppendTextToCurrentAccumulator("")))
                .WhenError(RenderError);
        }

        public void Visit(LiquidExpressionTree liquidExpressionTree)
        {
            LiquidExpressionEvaluator.Eval(liquidExpressionTree, _templateContext)
                .WhenSuccess(success => success.WhenSome(x => AppendTextToCurrentAccumulator(Render(x))))
                .WhenError(RenderError);
        }

        public String Render(IExpressionConstant result)
        {
            return ValueCaster.RenderAsString(result);
        }

        public void EvalExpressions(
            IEnumerable<TreeNode<LiquidExpression>> expressionTrees,
            Action<IEnumerable<Option<IExpressionConstant>>> successAction = null,
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

        public void StartWalking(TreeNode<IASTNode> rootNode, Action<String> accumulator)
        {
            PushTextAccumulator(accumulator);
            StartWalking(rootNode);
            PopTextAccumulator();
        }

    }

    public class ContinueException : Exception { }

    public class BreakException : Exception { }



}
