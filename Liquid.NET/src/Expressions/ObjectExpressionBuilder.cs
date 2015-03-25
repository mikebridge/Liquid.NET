using System;
using System.Collections.Generic;
using System.Linq;

using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{

    public delegate void OnExpressionCompleteEventHandler(TreeNode<ObjectExpression> expressionTree);

    public class ObjectExpressionTreeBuilder
    {

        public event OnExpressionCompleteEventHandler ExpressionCompleteEvent;

        private TreeNode<ObjectExpression> _objectExpressionTree;
        private TreeNode<ObjectExpression> _lastExpression; // we need to keep track of the previous one because we don't know if we're completely done or if there are filters.
        private readonly Stack<TreeNode<ObjectExpression>> _objectExpressionStack = new Stack<TreeNode<ObjectExpression>>(); 

        public void StartObjectExpression(IExpressionDescription expressionDescription)
        {
            Console.WriteLine("ObjectExpressionBuilder >>> PUSH");
            ObjectExpression objectExpression = new ObjectExpression { Expression = expressionDescription };
            var child = new TreeNode<ObjectExpression>(objectExpression);
            Console.WriteLine("Appending " + objectExpression + " TO TREE.");
            
            if (_objectExpressionStack.Any())
            {
                Console.WriteLine(" to " + _objectExpressionStack.Peek());
                _objectExpressionStack.Peek().AddChild(child);
            }
            else
            {
                Console.WriteLine(" to ROOT ");
                _objectExpressionTree = child;
            }
            _objectExpressionStack.Push(child);
        }

        public void AddFilterSymbolToLastExpression(FilterSymbol filter)
        {
            _lastExpression.Data.AddFilterSymbol(filter);
        }
        public void AddFilterArgToLastExpressionsFilter(IExpressionDescription filterArg)
        {
            _lastExpression.Data.FilterSymbols.Last().AddArg(filterArg);
        }

        public void SetRawArgsForLastExpressionsFilter(string argstring)
        {
            _lastExpression.Data.FilterSymbols.Last().RawArgs = argstring;
        }
        public void AddFilterSymbolToCurrentExpression(FilterSymbol filter)
        {
            _objectExpressionStack.Peek().Data.AddFilterSymbol(filter);
        }
        public void AddFilterArgToCurrentExpressionsFilter(IExpressionDescription filterArg)
        {
            _objectExpressionStack.Peek().Data.FilterSymbols.Last().AddArg(filterArg);
        }

        public void SetRawArgsForCurrentExpressionsFilter(string argstring)
        {
            _objectExpressionStack.Peek().Data.FilterSymbols.Last().RawArgs = argstring;
        }


        public void EndObjectExpression()
        {
            Console.WriteLine("ObjectExpressionBuilder >>> POP");
            _lastExpression = _objectExpressionStack.Pop();
            Console.WriteLine("invoking end event...");
            InvokeExpressionCompleteEvent(_lastExpression);
        }


        public TreeNode<ObjectExpression> ConstructedObjectExpressionTree
        {
            get
            {
                return _objectExpressionTree;
            }
        }

        public void InvokeExpressionCompleteEvent(TreeNode<ObjectExpression> expressionTree)
        {
            OnExpressionCompleteEventHandler handler = ExpressionCompleteEvent;
            if (handler != null)
            {
                handler(expressionTree);
            }
        }

         
    }

}
