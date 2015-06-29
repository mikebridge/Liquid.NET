using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
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
        private readonly SymbolTableStack _symbolTableStack;
        private readonly ConcurrentDictionary<String, int> _counters = new ConcurrentDictionary<string, int>();

        public readonly IList<LiquidError> Errors = new List<LiquidError>();

        public bool HasErrors { get { return Errors.Any();  } }

        public RenderingVisitor(LiquidASTRenderer astRenderer, SymbolTableStack symbolTableStack)
        {
            _astRenderer = astRenderer;
            _symbolTableStack = symbolTableStack;
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
            Console.WriteLine("Looking up Custom Tag " + customTag.TagName);
            var tagType = _symbolTableStack.LookupCustomTagRendererType(customTag.TagName);
            if (tagType != null)
            {
                _result += RenderCustomTag(customTag, tagType);
                return;
            }

            Console.WriteLine("Looking up Macro "+ customTag.TagName);
            var macroDescription = _symbolTableStack.LookupMacro(customTag.TagName);
            if (macroDescription != null)
            {
                //Console.WriteLine("...");
                //var evalResult = LiquidExpressionEvaluator.Eval(customTag.LiquidExpressionTrees, _symbolTableStack);
                var evalResults =
                    customTag.LiquidExpressionTrees.Select(x => LiquidExpressionEvaluator.Eval(x, _symbolTableStack)).ToList();
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
            var macro = ValueCaster.RenderAsString((IExpressionConstant) macroRenderer.Render(macroBlockTag, _symbolTableStack, args.ToList(), macroErrors));
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
                args => result = tagRenderer.Render(_symbolTableStack, args.ToList()).StringVal,
                errors => result = FormatErrors(errors));
            return result;

//            var evaledArgs = customTag.LiquidExpressionTrees.Select(x => LiquidExpressionEvaluator.Eval(x, _symbolTableStack)).ToList();
//            if (evaledArgs.Any(x => x.IsError))
//            {
//                return FormatErrors(evaledArgs.Where(x => x.IsError).Select(x => x.ErrorResult));
//            }
//            else
//            {
//                return tagRenderer.Render(_symbolTableStack, evaledArgs.Select(x => x.SuccessResult).ToList()).StringVal;
//            }
        }


     



        public void Visit(CustomBlockTag customBlockTag)
        {
            var tagType = _symbolTableStack.LookupCustomBlockTagRendererType(customBlockTag.TagName);
            var tagRenderer = CustomBlockTagRendererFactory.Create(tagType);
            if (tagRenderer == null)
            {
                AddError("Liquid syntax error: Unknown tag '" + customBlockTag.TagName + "'", customBlockTag);              
                return;
            }

            EvalExpressions(customBlockTag.LiquidExpressionTrees,
               args => _result += tagRenderer.Render(_symbolTableStack, customBlockTag.LiquidBlock, args.ToList()).StringVal,
               errors => _result += FormatErrors(errors));
            //IEnumerable<IExpressionConstant> args =
            //    customBlockTag.LiquidExpressionTrees.Select(x => LiquidExpressionEvaluator.Eval(x, _symbolTableStack));
            //_result += tagRenderer.Render(_symbolTableStack, customBlockTag.LiquidBlock, args.ToList()).StringVal;
        }

        public void Visit(CycleTag cycleTag)
        {
            String groupName = null;
            if (cycleTag.GroupNameExpressionTree != null)
            {
                LiquidError error = null;
                LiquidExpressionEvaluator.Eval(cycleTag.GroupNameExpressionTree, _symbolTableStack)
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
        /// <param name="cycleTag"></param>
        /// <returns></returns>
        private String GetNextCycleText(String groupName, CycleTag cycleTag)
        {

            int currentIndex = 0;
            //var groupNameAsString = groupName== null ? "" : ValueCaster.RenderAsString(groupName);
            //Console.WriteLine("Evaluating " + groupName);
            //var key = "cycle_" + groupNameAsString + "_" + String.Join("|", cycleTag.CycleList.Select(x => x.Value.ToString()));
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
            LiquidExpressionEvaluator.Eval(cycleTag.ElementAt(currentIndex), _symbolTableStack)
                .WhenSuccess(x => result = ValueCaster.RenderAsString(LiquidExpressionEvaluator.Eval(cycleTag.ElementAt(currentIndex), _symbolTableStack).SuccessResult.Value))
                .WhenError(err => result = FormatErrors(new List<LiquidError> {err}));

            return result;
            //return ValueCaster.RenderAsString(LiquidExpressionEvaluator.Eval(cycleTag.ElementAt(currentIndex), _symbolTableStack));
            //return cycleTag.ElementAt(currentIndex).Value.ToString();

        }

        public void Visit(AssignTag assignTag)
        {
            if (assignTag.LiquidExpressionTree == null)
            {
                _symbolTableStack.DefineGlobal(assignTag.VarName, null);
            }
            else
            {
                LiquidExpressionEvaluator.Eval(assignTag.LiquidExpressionTree, _symbolTableStack)
                    .WhenSuccess(x => x.WhenSome(some => _symbolTableStack.DefineGlobal(assignTag.VarName, some))
                        .WhenNone(() => _symbolTableStack.DefineGlobal(assignTag.VarName, null)))
                    .WhenError(RenderError);
            }
//                result =
//                )
//            if (result.HasValue)
//            {
//                _symbolTableStack.DefineGlobal(assignTag.VarName, result.Value);
//            }
//            else
//            {
//                _symbolTableStack.DefineGlobal(assignTag.VarName, new NilValue());
//            }
        }

        public void Visit(CaptureBlockTag captureBlockTag)
        {
            var hiddenVisitor = new RenderingVisitor(_astRenderer, _symbolTableStack);
            _astRenderer.StartVisiting(hiddenVisitor, captureBlockTag.RootContentNode);            
            _symbolTableStack.DefineGlobal(captureBlockTag.VarName, new StringValue(hiddenVisitor.Text) );
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
            includeRenderer.Render(includeTag, _symbolTableStack);
 
        }



//        private void AlterNumericvalue(string key, int defaultValue, Func<NumericValue, NumericValue> newValueFunc)
//        {
//            _symbolTableStack.FindVariable(key,
//                (st, foundExpression) =>
//                {
//                    var numref = foundExpression as NumericValue;
//                    st.DefineVariable(key,
//                        numref != null ? newValueFunc(numref) : new NumericValue(defaultValue));
//                },
//                () => _symbolTableStack.Define(key, new NumericValue(defaultValue)));
//        }

        public void Visit(ForBlockTag forBlockTag)
        {
            new ForRenderer(this, _astRenderer).Render(forBlockTag, _symbolTableStack);
        }

        public void Visit(IfThenElseBlockTag ifThenElseBlockTag)
        {

            // find the first place where the expression tree evaluates to true (i.e. which of the if/elsif/else clauses)
            // This ignores "eval" errors in clauses.
            var match = ifThenElseBlockTag.IfElseClauses.FirstOrDefault(
                                expr => {
                                    var result = LiquidExpressionEvaluator.Eval(expr.LiquidExpressionTree, _symbolTableStack);
                                    return result.IsSuccess && result.SuccessResult.HasValue && result.SuccessResult.Value.IsTrue;
                                });
            if (match != null)
            {
                _astRenderer.StartVisiting(this, match.LiquidBlock); // then render the contents
            }
        }

        public void Visit(CaseWhenElseBlockTag caseWhenElseBlockTag)
        {
            var valueToMatchResult = LiquidExpressionEvaluator.Eval(caseWhenElseBlockTag.LiquidExpressionTree, _symbolTableStack);
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
                        //    LiquidExpressionEvaluator.Eval(expr.LiquidExpressionTree, _symbolTableStack)));
                        expr.LiquidExpressionTree.Any(val =>
                            new EasyOptionComparer().Equals(valueToMatchResult.SuccessResult,
                                        LiquidExpressionEvaluator.Eval(val, _symbolTableStack).SuccessResult)));


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
            // not implemented yet
            
            //Console.WriteLine("Creating a macro "+ macroBlockTag.Name);
            //Console.WriteLine("That takes args " + String.Join(",", macroBlockTag.Args));
            //Console.WriteLine("and has body " + macroBlockTag.LiquidBlock);
            _symbolTableStack.DefineMacro(macroBlockTag.Name, macroBlockTag);
        }

        public void Visit(ErrorNode errorNode)
        {
            //Console.WriteLine("TODO: Render error : " + errorNode.ToString());
            _result += errorNode.LiquidError.ToString();
        }

        public void Visit(RootDocumentNode rootDocumentNode)
        {
           // noop
        }

        public void Visit(VariableReference variableReference)
        {
            variableReference.Eval(_symbolTableStack, new List<Option<IExpressionConstant>>());
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
            Console.WriteLine("Visiting Object Expression ");
            var liquidResult = LiquidExpressionEvaluator.Eval(liquidExpression, new List<Option<IExpressionConstant>>(), _symbolTableStack)
                .WhenSuccess(x => x.WhenSome(some => _result += Render(x.Value))
                                   .WhenNone(() => _result += Render(new NilValue())))
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

        public void Visit(NilValue nilValue)
        {
            // do not render anything for nil
        }

        public void Visit(LiquidExpressionTree liquidExpressionTree)
        {
            Console.WriteLine("Visiting Object Expression Tree ");

            var constResult = LiquidExpressionEvaluator.Eval(liquidExpressionTree, _symbolTableStack)
                .WhenSuccess(success => success.WhenSome(x => _result += Render(x)))
                .WhenError(RenderError);
        }

        public String Render(IExpressionConstant result)
        {
            Console.WriteLine("Rendering IExpressionConstant " + result.Value);

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
            var evaledArgs = expressionTrees.Select(x => LiquidExpressionEvaluator.Eval(x, _symbolTableStack)).ToList();
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
