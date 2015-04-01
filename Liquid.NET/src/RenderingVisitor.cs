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

namespace Liquid.NET
{

    /// <summary>
    /// Render the AST nodes as a String
    /// </summary>
    public class RenderingVisitor : IASTVisitor
    {
        private String _result = "";
        
        private readonly LiquidEvaluator _evaluator;
        private readonly SymbolTableStack _symbolTableStack;
        private readonly ConcurrentDictionary<String, int> _counters = new ConcurrentDictionary<string, int>();

        public RenderingVisitor(LiquidEvaluator evaluator, SymbolTableStack symbolTableStack)
        {
            _evaluator = evaluator;
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
                Console.WriteLine("...");
                IEnumerable<IExpressionConstant> args =
                    customTag.LiquidExpressionTrees.Select(x => LiquidExpressionEvaluator.Eval(x, _symbolTableStack));

                _result += RenderMacro(macroDescription, args);
                return;
            }
            _result += " ERROR: There is no macro or tag named "+  customTag.TagName+ " ";
        }

        private string RenderMacro(MacroBlockTag macroBlockTag, IEnumerable<IExpressionConstant> args)
        {
            var macroRenderer = new MacroRenderer();
            return ValueCaster.RenderAsString(macroRenderer.Render(macroBlockTag, _symbolTableStack, args.ToList()));
        }

        private string RenderCustomTag(CustomTag customTag, Type tagType)
        {
            var tagRenderer = CustomTagRendererFactory.Create(tagType);
            if (tagRenderer == null)
            {
                throw new Exception("Unregistered Tag: " + customTag.TagName);
            }
            IEnumerable<IExpressionConstant> args =
                customTag.LiquidExpressionTrees.Select(x => LiquidExpressionEvaluator.Eval(x, _symbolTableStack));
            var result = tagRenderer.Render(_symbolTableStack, args.ToList()).StringVal;
            return result;
        }

        public void Visit(CustomBlockTag customBlockTag)
        {
            var tagType = _symbolTableStack.LookupCustomBlockTagRendererType(customBlockTag.TagName);
            var tagRenderer = CustomBlockTagRendererFactory.Create(tagType);
            if (tagRenderer == null)
            {
                throw new Exception("Unregistered Tag: " + customBlockTag.TagName);
            }
            IEnumerable<IExpressionConstant> args =
                customBlockTag.LiquidExpressionTrees.Select(x => LiquidExpressionEvaluator.Eval(x, _symbolTableStack));
            _result += tagRenderer.Render(_symbolTableStack, customBlockTag.LiquidBlock, args.ToList()).StringVal;

            

        }

        public void Visit(CycleTag cycleTag)
        {

            _result += GetNextCycleText(cycleTag);
        }


        public void Visit(AssignTag assignTag)
        {
            var result = LiquidExpressionEvaluator.Eval(assignTag.LiquidExpressionTree, _symbolTableStack);
            _symbolTableStack.DefineGlobal(assignTag.VarName, result);
        }

        public void Visit(CaptureBlockTag captureBlockTag)
        {
            var hiddenVisitor = new RenderingVisitor(_evaluator, _symbolTableStack);
            _evaluator.StartVisiting(hiddenVisitor, captureBlockTag.RootContentNode);            
            _symbolTableStack.DefineGlobal(captureBlockTag.VarName, new StringValue(hiddenVisitor.Text) );
        }


        public void Visit(IncrementTag incrementTag)
        {
            int currentIndex;
            var key = "incr_" + incrementTag.VarName;

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
            throw new NotImplementedException();
        }


        public void Visit(DecrementTag decrementTag)
        {
            int currentIndex;
            var key = "decr_" + decrementTag.VarName;

            while (true)
            {
                currentIndex = _counters.GetOrAdd(key, -1);
                var newindex = (currentIndex - 1);
                if (_counters.TryUpdate(key, newindex, currentIndex))
                {
                    break;
                }
            }

            _result += currentIndex;
        }

        /// <summary>
        /// Side effect: state is managed in the _counters dictionary.
        /// </summary>
        /// <param name="cycleTag"></param>
        /// <returns></returns>
        private String GetNextCycleText(CycleTag cycleTag)
        {
            int currentIndex;
            var key = "cycle_" + cycleTag.Group +"_"+String.Join("|", cycleTag.CycleList.Select(x => x.Value.ToString()));
            
            while (true)
            {                
                currentIndex = _counters.GetOrAdd(key, 0);
                var newindex = (currentIndex + 1)%cycleTag.Length;

                // fails if updated concurrently by someone else.
                if (_counters.TryUpdate(key, newindex, currentIndex))
                {
                    break;
                }
            }

            return cycleTag.ElementAt(currentIndex).Value.ToString();

        }

        public void Visit(ForBlockTag forBlockTag)
        {
            new ForRenderer(this, _evaluator).Render(forBlockTag, _symbolTableStack);
        }

        public void Visit(IfThenElseBlockTag ifThenElseBlockTag)
        {

            // find the first place where the expression tree evaluates to true (i.e. which of the if/elsif/else clauses)
            var match = ifThenElseBlockTag.IfElseClauses.FirstOrDefault(
                                expr => LiquidExpressionEvaluator.Eval(expr.LiquidExpressionTree, _symbolTableStack).IsTrue);
            if (match != null)
            {
                _evaluator.StartVisiting(this, match.LiquidBlock); // then render the contents
            }
        }

        public void Visit(CaseWhenElseBlockTag caseWhenElseBlockTag)
        {
            var valueToMatch = LiquidExpressionEvaluator.Eval(caseWhenElseBlockTag.LiquidExpressionTree, _symbolTableStack);
            //Console.WriteLine("Value to Match: "+valueToMatch);

            var match =
                caseWhenElseBlockTag.WhenClauses.FirstOrDefault(
                    expr =>
                        // Take the valueToMatch "Case" expression result value
                        // and check if it's equal to the expr.LiquidExpressionTree expression.
                        // THe "EasyValueComparer" is supposed to match stuff fairly liberally,
                        // though it doesn't cast values---TODO: probably it should.
                        new EasyValueComparer().Equals(valueToMatch,
                            LiquidExpressionEvaluator.Eval(expr.LiquidExpressionTree, _symbolTableStack)));

            if (match != null)
            {
                _evaluator.StartVisiting(this, match.LiquidBlock); // then eval + render the HTML
            }
            else if (caseWhenElseBlockTag.HasElseClause)
            {
                _evaluator.StartVisiting(this, caseWhenElseBlockTag.ElseClause.LiquidBlock);
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
            
            Console.WriteLine("Creating a macro "+ macroBlockTag.Name);
            Console.WriteLine("That takes args " + String.Join(",", macroBlockTag.Args));
            Console.WriteLine("and has body " + macroBlockTag.LiquidBlock);
            _symbolTableStack.DefineMacro(macroBlockTag.Name, macroBlockTag);
        }

        public void Visit(RootDocumentNode rootDocumentNode)
        {
           // noop
        }

        public void Visit(VariableReference variableReference)
        {
            variableReference.Eval(_symbolTableStack, new List<IExpressionConstant>());
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

            var constResult = LiquidExpressionEvaluator.Eval(liquidExpression, _symbolTableStack);

            _result += Render(constResult); 

        }

        public void Visit(LiquidExpressionTree liquidExpressionTree)
        {
            Console.WriteLine("Visiting Object Expression Tree ");

            var constResult = LiquidExpressionEvaluator.Eval(liquidExpressionTree, _symbolTableStack);

            _result += Render(constResult); 
        }

        public String Render(IExpressionConstant result)
        {
            Console.WriteLine("Rendering IExpressionConstant " + result.Value);

            if (result.HasError)
            {
                return result.ErrorMessage;
            }

            return ValueCaster.RenderAsString(result);
        }



    }

    public class ContinueException : Exception { }

    public class BreakException : Exception { }
}
