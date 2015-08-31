using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
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
        private String _result = "";
        
        private readonly LiquidASTRenderer _astRenderer;
        private readonly ITemplateContext _templateContext;
        //private readonly SymbolTableStack _templateContext.SymbolTableStack;
        private readonly ConcurrentDictionary<String, int> _counters = new ConcurrentDictionary<string, int>();

        public readonly IList<LiquidError> Errors = new List<LiquidError>();
        private IfChangedRenderer _isChangedRenderer;

        public bool HasErrors { get { return Errors.Any();  } }

        //public RenderingVisitor(LiquidASTRenderer astRenderer, SymbolTableStack symbolTableStack)
        public RenderingVisitor(LiquidASTRenderer astRenderer, ITemplateContext templateContext)
        {
            _astRenderer = astRenderer;
            _templateContext = templateContext;
        }

        public String Text
        {
            get { return _result; }
        }

        public void Visit(RawBlockTag rawBlockTag)
        {
            _result += rawBlockTag.Value;
        }


        public void Visit(CommentBlockTag commentBlockTag)
        {
            // do nothing
        }

        public void Visit(CustomTag customTag)
        {
            //Console.WriteLine("Looking up Custom Tag " + customTag.TagName);
            var tagType = _templateContext.SymbolTableStack.LookupCustomTagRendererType(customTag.TagName);
            if (tagType != null)
            {
                _result += RenderCustomTag(customTag, tagType);
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
                _result += RenderMacro(macroDescription, evalResults.Select(x => x.SuccessResult));
                return;
            }
            //_result += " ERROR: There is no macro or tag named "+  customTag.TagName+ " ";
            AddError("Liquid syntax error: Unknown tag '" + customTag.TagName + "'", customTag);
            //_result += "Liquid syntax error: Unknown tag '"+customTag.TagName+"'";
        }

        private void RenderError(LiquidError liquidError)
        {
            _result += FormatErrors(new List<LiquidError>{liquidError});
        }
        private void RenderErrors(IEnumerable<LiquidError> liquidErrors)
        {
            _result += FormatErrors(liquidErrors);
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

        private void AddError(String message, IASTNode node)
        { 
            // TODO: pass the tag info in...
            Errors.Add(new LiquidError{Message = message});
        }

        private string RenderMacro(MacroBlockTag macroBlockTag, IEnumerable<Option<IExpressionConstant>> args)
        {
            var macroRenderer = new MacroRenderer();
            //var hiddenRenderer = new RenderingVisitor(_a)
            IList<LiquidError> macroErrors = new List<LiquidError>();
            var macro = ValueCaster.RenderAsString((IExpressionConstant) macroRenderer.Render(macroBlockTag, _templateContext, args.ToList(), macroErrors));
            foreach (var error in macroErrors)
            {
                Errors.Add(error);
            }
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

//            var evaledArgs = customTag.LiquidExpressionTrees.Select(x => LiquidExpressionEvaluator.Eval(x, _templateContext.SymbolTableStack)).ToList();
//            if (evaledArgs.Any(x => x.IsError))
//            {
//                return FormatErrors(evaledArgs.Where(x => x.IsError).Select(x => x.ErrorResult));
//            }
//            else
//            {
//                return tagRenderer.Render(_templateContext.SymbolTableStack, evaledArgs.Select(x => x.SuccessResult).ToList()).StringVal;
//            }
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
               args => _result += tagRenderer.Render(_templateContext, customBlockTag.LiquidBlock, args.ToList()).StringVal,
               errors => _result += FormatErrors(errors));
            //IEnumerable<IExpressionConstant> args =
            //    customBlockTag.LiquidExpressionTrees.Select(x => LiquidExpressionEvaluator.Eval(x, _templateContext.SymbolTableStack));
            //_result += tagRenderer.Render(_templateContext.SymbolTableStack, customBlockTag.LiquidBlock, args.ToList()).StringVal;
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
            _result += GetNextCycleText(groupName, cycleTag);
        }

        /// <summary>
        /// Side effect: state is managed in the _counters dictionary.
        /// </summary>
        /// <returns></returns>
        private String GetNextCycleText(String groupName, CycleTag cycleTag)
        {

            int currentIndex = 0;
            //var groupNameAsString = groupName== null ? "" : ValueCaster.RenderAsString(groupName);
            //Console.WriteLine("Evaluating " + groupName);
            //var key = "cycle_" + groupNameAsString + "_" + String.Join("|", cycleTag.CycleList.Select(x => x.Value.ToString()));

            // Create a like dictionary key entry to keep track of this declaration.  THis takes the variable
            // names (not the eval-ed variables) or literals and concatenates them together.
            var key = "cycle_" + groupName + "_" + String.Join("|", cycleTag.CycleList.Select(x =>
            {
                var varresult = "";
                varresult = x.Data.Expression.ToString();
                return varresult;
            }));
            
            
//            var key = "cycle_" + (groupName ?? "NULL") + "_" + String.Join("|", cycleTag.CycleList.Select(x =>
//            {
//                x.
//                return cycleResult;
//            }));

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
            //return ValueCaster.RenderAsString(LiquidExpressionEvaluator.Eval(cycleTag.ElementAt(currentIndex), _templateContext.SymbolTableStack));
            //return cycleTag.ElementAt(currentIndex).Value.ToString();

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
            var hiddenVisitor = new RenderingVisitor(_astRenderer, _templateContext);
            _astRenderer.StartVisiting(hiddenVisitor, captureBlockTag.RootContentNode);            
            _templateContext.SymbolTableStack.DefineGlobal(captureBlockTag.VarName, new StringValue(hiddenVisitor.Text) );
            foreach (var error in hiddenVisitor.Errors)
            {
                Errors.Add(error);
            }
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

            _result += currentIndex;
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

            _result += currentIndex;
        }

        public void Visit(IncludeTag includeTag)
        {

            var includeRenderer = new IncludeRenderer(this, _astRenderer);
            includeRenderer.Render(includeTag, _templateContext);
 
        }



//        private void AlterNumericvalue(string key, int defaultValue, Func<NumericValue, NumericValue> newValueFunc)
//        {
//            _templateContext.SymbolTableStack.FindVariable(key,
//                (st, foundExpression) =>
//                {
//                    var numref = foundExpression as NumericValue;
//                    st.DefineVariable(key,
//                        numref != null ? newValueFunc(numref) : new NumericValue(defaultValue));
//                },
//                () => _templateContext.SymbolTableStack.Define(key, new NumericValue(defaultValue)));
//        }

        public void Visit(ForBlockTag forBlockTag)
        {
            new ForRenderer(this, _astRenderer).Render(forBlockTag, _templateContext);
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
                _astRenderer.StartVisiting(this, match.LiquidBlock); // then render the contents
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
                        // though it doesn't cast values---TODO: probably it should.
                        //new EasyValueComparer().Equals(valueToMatch,
                        //    LiquidExpressionEvaluator.Eval(expr.LiquidExpressionTree, _templateContext.SymbolTableStack)));
                        expr.LiquidExpressionTree.Any(val =>
                            new EasyOptionComparer().Equals(valueToMatchResult.SuccessResult,
                                        LiquidExpressionEvaluator.Eval(val, _templateContext).SuccessResult)));


            if (match != null)
            {
                _astRenderer.StartVisiting(this, match.LiquidBlock); // then eval + render the HTML
            }
            else if (caseWhenElseBlockTag.HasElseClause)
            {
                _astRenderer.StartVisiting(this, caseWhenElseBlockTag.ElseClause.LiquidBlock);
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

        public void Visit(ErrorNode errorNode)
        {
            //Console.WriteLine("TODO: Render error : " + errorNode.ToString());
            _result += errorNode.LiquidError.ToString();
        }

        public void Visit(IfChangedBlockTag ifChangedBlockTag)
        {
            // This maintains state, so there's only one.
            if (_isChangedRenderer == null)
            {
                _isChangedRenderer = new IfChangedRenderer(this, _astRenderer, _templateContext);
            }
            _result += _isChangedRenderer.Next(ifChangedBlockTag.UniqueId, ifChangedBlockTag.LiquidBlock, _astRenderer);

        }

        public void Visit(TableRowBlockTag tableRowBlockTag)
        {
            new TableRowRenderer(this, _astRenderer)
                .Render(tableRowBlockTag, _templateContext, str => _result += str);
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
            _result += Render(stringValue); 
        }

        /// <summary>
        /// Process the object / filter chain
        /// </summary>
        /// <param name="liquidExpression"></param>
        public void Visit(LiquidExpression liquidExpression)
        {
            //Console.WriteLine("Visiting Object Expression ");
            var liquidResult = LiquidExpressionEvaluator.Eval(liquidExpression, new List<Option<IExpressionConstant>>(), _templateContext)
                .WhenSuccess(x => x.WhenSome(some => _result += Render(x.Value))
                                   .WhenNone(() => _result += ""))
                 .WhenError(RenderError);
//            if (liquidResult.HasValue)
//            {
//                _result += Render(liquidResult.Value);
//            }
//            else
//            {
//                _result += Render(new NilValue());
//            }

        }

        public void Visit(LiquidExpressionTree liquidExpressionTree)
        {
            //Console.WriteLine("Visiting Object Expression Tree ");

            var constResult = LiquidExpressionEvaluator.Eval(liquidExpressionTree, _templateContext)
                .WhenSuccess(success => success.WhenSome(x => _result += Render(x)))
                .WhenError(RenderError);
        }

        public String Render(IExpressionConstant result)
        {
            //Console.WriteLine("Rendering IExpressionConstant " + result.Value);

//            if (result.HasError)
//            {
//                return result.ErrorMessage;
//            }

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
    }

    public class ContinueException : Exception { }

    public class BreakException : Exception { }
}
