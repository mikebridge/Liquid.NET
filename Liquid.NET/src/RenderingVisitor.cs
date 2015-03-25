using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Rendering;
using Liquid.NET.Symbols;
using Liquid.NET.Tags;

namespace Liquid.NET
{

    /// <summary>
    /// Render the AST nodes as a String
    /// </summary>
    public class RenderingVisitor : IASTVisitor
    {
        private readonly LiquidEvaluator _evaluator;
        private readonly SymbolTableStack _symbolTableStack;
        private readonly ConcurrentDictionary<String, int> _counters = new ConcurrentDictionary<string, int>();

        private String _result = "";


        public RenderingVisitor(LiquidEvaluator evaluator, SymbolTableStack symbolTableStack)
        {
            _evaluator = evaluator;
            _symbolTableStack = symbolTableStack;
        }

        public String Text
        {
            get { return _result; }
        }

        public void Visit(RawBlock rawBlock)
        {
            Console.WriteLine("Visiting raw text = " + rawBlock.Value);
            _result += rawBlock.Value;
        }


        public void Visit(CommentBlock commentBlock)
        {
            // do nothing
        }

        public void Visit(CustomTag customTag)
        {
            _result += "RENDERING CustomTag";
        }

        public void Visit(CycleTag cycleTag)
        {

            _result += GetNextCycleText(cycleTag);
        }

        public void Visit(UnlessBlock unlessBlock)
        {
            throw new NotImplementedException();
        }

        public void Visit(CaseBlock caseBlock)
        {
            throw new NotImplementedException();
        }

        public void Visit(AssignTag assignTag)
        {
            Console.WriteLine("RENDERING ASSIGN " + assignTag.VarName);
            var result = LiquidExpressionEvaluator.Eval(assignTag.ObjectExpressionTree, _symbolTableStack);
            _symbolTableStack.DefineGlobal(assignTag.VarName, result);
        }

        public void Visit(CaptureBlock captureBlock)
        {
            Console.WriteLine("RENDERING CAPTURE " + captureBlock.VarName);
            var hiddenVisitor = new RenderingVisitor(this._evaluator, this._symbolTableStack);
            _evaluator.StartVisiting(hiddenVisitor, captureBlock.RootContentNode);
            
            _symbolTableStack.DefineGlobal(captureBlock.VarName, new StringValue(hiddenVisitor.Text) );
        }


        public void Visit(IncrementTag incrementTag)
        {
            int currentIndex;
            var key = "incr_" + incrementTag.VarName;
            Console.WriteLine("KEY: " + key);
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
            Console.WriteLine("KEY: " + key);
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


        //private String

        /// <summary>
        /// Side effect: state is managed in the _counters dictionary.
        /// </summary>
        /// <param name="cycleTag"></param>
        /// <returns></returns>
        private String GetNextCycleText(CycleTag cycleTag)
        {
            int currentIndex;
            // this appears to be the implementation of the dictionary key in shopify liquid
            var key = "cycle_" + cycleTag.Group +"_"+String.Join("|", cycleTag.CycleList.Select(x => x.Value.ToString()));
            Console.WriteLine("KEY: "+key);
            while (true)
            {                
                currentIndex = _counters.GetOrAdd(key, 0);
                Console.WriteLine("FOUND "+currentIndex);
                var newindex = (currentIndex + 1)%cycleTag.Length;

                // fails if updated concurrently by someone else.
                if (_counters.TryUpdate(key, newindex, currentIndex))
                {
                    break;
                }
            }
            return cycleTag.ElementAt(currentIndex).Value.ToString();

        }

        public void Visit(ForBlock forBlock)
        {
            new ForRenderer(this, _evaluator).Render(forBlock, _symbolTableStack);
        }



        public void Visit(IfThenElseBlock ifThenElseBlock)
        {

            // find the first place where the expression tree evaluates to true (i.e. which of the if/elsif/else clauses)
            //var match = IfThenElseBlock.IfExpressions.FirstOrDefault(expr => LiquidExpressionEvaluator.Eval(_symbolTableStack, expr.ObjectExpression).IsTrue);
            var match =
                ifThenElseBlock.IfExpressions.FirstOrDefault(
                    expr =>
                        //ValueCaster.Cast<IExpressionConstant, BooleanValue>(
                        LiquidExpressionEvaluator.Eval(expr.ObjectExpressionTree, _symbolTableStack).IsTrue);
                        //expr.Eval(_symbolTableStack).IsTrue);
                        //).BoolValue);
            if (match != null)
            {
                _evaluator.StartVisiting(this, match.RootContentNode); // then render the contents
            }
        }

        public void Visit(RootDocumentSymbol rootDocumentSymbol)
        {
           // Console.WriteLine("Visiting Root Node");
        }

        public void Visit(VariableReference variableReference)
        {
            Console.WriteLine("Rendering variable "+variableReference.Name);
            //_result += "RENDERING VariableReference";
            variableReference.Eval(_symbolTableStack, new List<IExpressionConstant>());
        }

        public void Visit(StringValue stringValue)
        {
            Console.WriteLine(")))) Rendering "+stringValue.Value);
            //_result += StringValue.Value;
            _result += Render(stringValue); 
        }

        /// <summary>
        /// Process the object / filter chain
        /// </summary>
        /// <param name="objectExpression"></param>
        public void Visit(ObjectExpression objectExpression)
        {
            Console.WriteLine("Visiting Object Expression ");

            var constResult = LiquidExpressionEvaluator.Eval(objectExpression, _symbolTableStack);
            //var constResult = LiquidExpressionEvaluator.Eval(_symbolTableStack,objectExpression);

            _result += Render(constResult); 

        }

        public void Visit(ObjectExpressionTree objectExpressionTree)
        {
            Console.WriteLine("Visiting Object Expression Tree ");

            var constResult = LiquidExpressionEvaluator.Eval(objectExpressionTree, _symbolTableStack);
            //var constResult = LiquidExpressionEvaluator.Eval(_symbolTableStack,objectExpression);

            _result += Render(constResult); 
        }

        public String Render(IExpressionConstant result)
        {
            Console.WriteLine("Rendering IExpressionConstant " + result.Value);
            if (result.HasError)
            {
                return result.ErrorMessage;
            }
            var stringResult = ValueCaster.Cast<IExpressionConstant, StringValue>(result);
            return stringResult.Value.ToString();
        }



    }
}
