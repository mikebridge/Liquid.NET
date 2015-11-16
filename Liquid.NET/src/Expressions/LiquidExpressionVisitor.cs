using System;
using System.Collections.Generic;
using System.Linq;

using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{


    // This is what the visitor will look like eventually.
    public interface ILiquidExpressionVisitor
    {
        void Visit(LiquidValue expr);
        void Visit(VariableReferenceTree expr);
        void Visit(AndExpression expr);
        void Visit(NotExpression expr);
        void Visit(ContainsExpression expr);
        void Visit(GroupedExpression expr);
        void Visit(EqualsExpression expr);
        void Visit(FalseExpression expr);
        void Visit(GreaterThanExpression expr);
        void Visit(LessThanExpression expr);
        void Visit(GreaterThanOrEqualsExpression expr);
        void Visit(LessThanOrEqualsExpression expr);
        void Visit(IsBlankExpression expr);
        void Visit(IsEmptyExpression expr);
        void Visit(IsPresentExpression expr);
        void Visit(NotEqualsExpression expr);
        void Visit(OrExpression expr);
        //void Visit(LiquidExpression expr) // TODO: "LiquidExpression" should be "FilterExpression" maybe, then it should implement the same interface.

    }

    /// <summary>
    /// This isn't a visitor yet.  What is currently IExpressionDescription.Eval should ultimately be 
    /// refactored into a visitor pattern.  This is where they will end up.
    /// </summary>
    public class LiquidExpressionVisitor //: ILiquidExpressionVisitor
    {
        private readonly ITemplateContext _templateContext;

        private readonly Stack<LiquidExpressionResult> _result = new Stack<LiquidExpressionResult>();
        private readonly Stack<List<LiquidExpressionResult>> _childResults = new Stack<List<LiquidExpressionResult>>();

        public LiquidExpressionVisitor(ITemplateContext templateContext)
        {
            _templateContext = templateContext;
        }


        public LiquidExpressionResult Result
        {
            get { return _result.Peek(); }
        }

        public LiquidExpressionVisitor Traverse(TreeNode<IExpressionDescription> tree)
        {
            //_result.Push(new LiquidExpressionResult())
            var childResultCollector = new List<LiquidExpressionResult>();
            _childResults.Push(childResultCollector);
            foreach (var child in tree.Children)
            {
                // TODO: when this is all cleaned up, this can be removed:
                IEnumerable<Option<ILiquidValue>> dummy = new List<Option<ILiquidValue>>();
                //child.Data.Accept(this, dummy);
                //child.Data.Accept(visitor);
            }
            return this;
        }




        public static LiquidExpressionResult Visit(LiquidValue liquidValue)
        {
            return LiquidExpressionResult.Success(liquidValue);
        }

        public static VariableReferenceTreeEvalResult Visit(VariableReferenceTree variableReferenceTree, ITemplateContext templateContext, IEnumerable<Option<ILiquidValue>> childresults)
        {
            var errorWhenValueMissing = templateContext.Options.ErrorWhenValueMissing;

            var childResultsList = childresults as IList<Option<ILiquidValue>> ?? childresults.ToList();

            //TODO: THis isn't apssing on errorWhenValueMissing. :(

            var valueResult = variableReferenceTree.Value.Accept(templateContext, childResultsList);
            // var valueResult = ((VariableReferenceTree )Value(.PartialEval(ITemplateContext templateContext, IEnumerable<Option<ILiquidValue>> childresults, bool errorWhenValueMissing)
            if (valueResult.IsError)
            {
                return new VariableReferenceTreeEvalResult(valueResult, new None<ILiquidValue>(), new None<ILiquidValue>());
            }
            LiquidExpressionResult indexResult;
            if (variableReferenceTree.IndexExpression != null)
            {
                indexResult = variableReferenceTree.IndexExpression.Accept(templateContext, childResultsList);
                if (indexResult.IsError)
                {
                    //return indexResult;
                    return new VariableReferenceTreeEvalResult(indexResult, valueResult.SuccessResult, new None<ILiquidValue>());
                }
            }
            else
            {

                return new VariableReferenceTreeEvalResult(valueResult, valueResult.SuccessResult, new None<ILiquidValue>());
            }

            if (!valueResult.SuccessResult.HasValue)
            {
                return new VariableReferenceTreeEvalResult(LiquidExpressionResult.Success(new None<ILiquidValue>()), valueResult.SuccessResult, new None<ILiquidValue>());
                //return LiquidExpressionResult.Success(new None<ILiquidValue>());
                //return LiquidExpressionResult.Error(SymbolTable.NotFoundError(valueResult));
            }
            if (!indexResult.SuccessResult.HasValue)
            {
                return new VariableReferenceTreeEvalResult(LiquidExpressionResult.Success(new None<ILiquidValue>()), valueResult.SuccessResult, indexResult.SuccessResult);
                //return LiquidExpressionResult.Success(new None<ILiquidValue>());
                //return LiquidExpressionResult.Error("ERROR: the index for "+valueResult.SuccessResult.Value+" has no value");
            }
            var result = new IndexDereferencer().Lookup(
                valueResult.SuccessResult.Value,
                indexResult.SuccessResult.Value,
                errorWhenValueMissing);
            return new VariableReferenceTreeEvalResult(result, valueResult.SuccessResult, indexResult.SuccessResult);
        }

        public static LiquidExpressionResult Visit(VariableReference variableReference, ITemplateContext templateContext)
        {
            var lookupResult = templateContext.SymbolTableStack.Reference(variableReference.Name);
            return lookupResult.IsSuccess
                ? lookupResult
                : ErrorOrNone(templateContext, lookupResult);
        }





        public static LiquidExpressionResult ErrorOrNone(ITemplateContext templateContext, LiquidExpressionResult failureResult)
        {
            if (templateContext.Options.ErrorWhenValueMissing)
            {
                return failureResult;
            }
            else
            {
                return LiquidExpressionResult.Success(new None<ILiquidValue>());
            }
        }

        public class VariableReferenceTreeEvalResult
        {
            public VariableReferenceTreeEvalResult(
                LiquidExpressionResult result,
                Option<ILiquidValue> lastValue,
                Option<ILiquidValue> index)
            {
                LiquidExpressionResult = result;
                LastValue = lastValue;
                Index = index;
            }

            public LiquidExpressionResult LiquidExpressionResult { get; private set; }
            public Option<ILiquidValue> LastValue { get; private set; }
            public Option<ILiquidValue> Index { get; private set; }
        }

        public static LiquidExpressionResult Visit(EqualsExpression equalsExpression, IEnumerable<Option<ILiquidValue>> expressions)
        {
            IList<Option<ILiquidValue>> exprList = expressions.ToList();


            if (exprList.Count != 2)
            {
                // This shouldn't happen if the parser is correct.
                return LiquidExpressionResult.Error("Equals is a binary expression but received " + exprList.Count + ".");
            }

            if (!exprList[0].HasValue && !exprList[1].HasValue)
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(true));
            }
            if (!exprList[0].HasValue || !exprList[1].HasValue)
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(false));
            }
            if (exprList[0].GetType() == exprList[1].GetType())
            {
                var isEqual = exprList[0].Value.Equals(exprList[1].Value);
                return LiquidExpressionResult.Success(new LiquidBoolean(isEqual));
            }

            return LiquidExpressionResult.Error("\"Equals\" implementation can't cast that yet");
        }

        public static LiquidExpressionResult Visit(GroupedExpression expr, IEnumerable<Option<ILiquidValue>> expressions)
        {
            var childExpressions = expressions.ToList();

            return childExpressions.Count != 1
                ? LiquidExpressionResult.Error("Unable to parse expression in parentheses")
                : LiquidExpressionResult.Success(childExpressions.First());
        }

        public static LiquidExpressionResult Visit(AndExpression expr, IEnumerable<Option<ILiquidValue>> expressions)
        {
            var exprList = expressions.ToList();
            if (exprList.Count != 2)
            {
                throw new Exception("An AND expression must have two values");
                // TODO: when the Eval is separated this will be redundant.
            }
            return
                LiquidExpressionResult.Success(
                    new LiquidBoolean(exprList.All(x => x.HasValue) && exprList.All(x => x.Value.IsTrue)));
        }

        public static LiquidExpressionResult Visit(ContainsExpression expr, IEnumerable<Option<ILiquidValue>> expressions)
        {
            IList<Option<ILiquidValue>> exprList = expressions.ToList();
            if (exprList.Count != 2)
            {
                return LiquidExpressionResult.Error("Contains is a binary expression but received " + exprList.Count + ".");
            }
            if (!exprList[0].HasValue || !exprList[1].HasValue)
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(false));
            }

            //return Contains((dynamic) exprList[0].Value, exprList[1].Value);
            var arr = exprList[0].Value as LiquidCollection;
            if (arr != null)
            {
                return expr.Contains(arr, exprList[1].Value);
            }
            var dict = exprList[0].Value as LiquidHash;
            if (dict != null)
            {
                return expr.Contains(dict, exprList[1].Value);
            }
            var str = exprList[0].Value as LiquidString;
            if (str != null)
            {
                return expr.Contains(str, exprList[1].Value);
            }
            return expr.Contains(exprList[0].Value, exprList[1].Value);
        }

        public static LiquidExpressionResult Visit(NotEqualsExpression expr, IEnumerable<Option<ILiquidValue>> expressions)
        {

            IList<Option<ILiquidValue>> exprList = expressions.ToList();

            //Console.WriteLine("EqualsExpression is Eval-ing expressions ");
            if (exprList.Count != 2)
            {
                //return LiquidExpressionResult.Error("Equals is a binary expression but received " + exprList.Count() + ".");
                return LiquidExpressionResult.Error("Equals is a binary expression but received " + exprList.Count + ".");
            }

            if (exprList.All(x => !x.HasValue)) // both null
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(false));
            }
            if (exprList.Any(x => !x.HasValue)) // one null
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(true));
            }

            if (exprList[0].GetType() == exprList[1].GetType())
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(!exprList[0].Value.Equals(exprList[1].Value)));
            }

            return LiquidExpressionResult.Error("\"Not Equals\" implementation can't cast yet");
        }

        public static LiquidExpressionResult Visit(IsBlankExpression expr, IEnumerable<Option<ILiquidValue>> expressions)
        {
            var list = expressions.ToList();
            if (!list.Any())
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(true));
            }
            if (list.Count != 1)
            {
                return LiquidExpressionResult.Error("Expected one variable to compare with \"blank\"");
            }

            if (!list[0].HasValue)
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(true)); // null is blank.
            }
            return LiquidExpressionResult.Success(new LiquidBoolean(BlankChecker.IsBlank(list[0].Value)));
        }

        public static LiquidExpressionResult Visit(OrExpression expr, IEnumerable<Option<ILiquidValue>> expressions)
        {
            var exprList = expressions.ToList();
            return LiquidExpressionResult.Success(new LiquidBoolean(exprList.Any(x => x.HasValue && x.Value.IsTrue)));
        }

        public static LiquidExpressionResult Visit(FalseExpression expr)
        {
            return LiquidExpressionResult.Success(new Some<ILiquidValue>(new LiquidBoolean(false)));
        }

        public static LiquidExpressionResult Visit(GreaterThanExpression expr, IEnumerable<Option<ILiquidValue>> expressions)
        {
            var expressionList = expressions.ToList();
            if (expressionList.Any(x => !x.HasValue))
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(false));
            }
            return
                LiquidExpressionResult.Success(ComparisonExpressions.Compare(expressionList[0].Value, expressionList[1].Value,
                    (x, y) => x > y));
        }

        public static LiquidExpressionResult Visit(LessThanOrEqualsExpression expr, IEnumerable<Option<ILiquidValue>> expressions)
        {
            var expressionList = expressions.ToList();
            if (expressionList.Any(x => !x.HasValue))
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(false));
            }
            return
                LiquidExpressionResult.Success(ComparisonExpressions.Compare(expressionList[0].Value, expressionList[1].Value,
                    (x, y) => x <= y));
        }

        public static LiquidExpressionResult Visit(GreaterThanOrEqualsExpression expr, IEnumerable<Option<ILiquidValue>> expressions)
        {
            var expressionList = expressions.ToList();
            if (expressionList.Any(x => !x.HasValue))
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(false));
            }
            return
                LiquidExpressionResult.Success(ComparisonExpressions.Compare(expressionList[0].Value, expressionList[1].Value,
                    (x, y) => x >= y));
        }

        public static LiquidExpressionResult Visit(LessThanExpression expr, IEnumerable<Option<ILiquidValue>> expressions)
        {
            var expressionList = expressions.ToList();
            if (expressionList.Any(x => !x.HasValue))
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(false));
            }

            var val1 = expressionList[0].Value;
            var val2 = expressionList[1].Value;
            return LiquidExpressionResult.Success(ComparisonExpressions.Compare(val1, val2, (x, y) => x < y));
        }

        public static LiquidExpressionResult Visit(IsPresentExpression expr, IEnumerable<Option<ILiquidValue>> expressions)
        {
            var list = expressions.ToList();
            if (list.Count != 1)
            {
                return LiquidExpressionResult.Error("Expected one variable to compare with \"present\"");
            }
            if (!list[0].HasValue)
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(false)); // null is not present.
            }
            return LiquidExpressionResult.Success(new LiquidBoolean(!BlankChecker.IsBlank(list[0].Value)));
        }

        public static LiquidExpressionResult Visit(IsEmptyExpression expr, IEnumerable<Option<ILiquidValue>> expressions)
        {
            var list = expressions.ToList();
            if (list.Count != 1)
            {
                return LiquidExpressionResult.Error("Expected one variable to compare with \"empty\"");
            }
            if (!list[0].HasValue)
            {
                return LiquidExpressionResult.Success(new LiquidBoolean(true)); // nulls are empty.
            }
            return LiquidExpressionResult.Success(new LiquidBoolean(EmptyChecker.IsEmpty(list[0].Value)));
        }
    }
}
