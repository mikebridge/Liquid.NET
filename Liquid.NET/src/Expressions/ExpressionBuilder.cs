using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{

    public class ExpressionBuilder
    {

        private readonly Stack<TreeNode<IExpressionDescription>> _currentStack = new Stack<TreeNode<IExpressionDescription>>();

        private TreeNode<IExpressionDescription> _rootNode;

        public void MarkExpressionComplete()
        {
            _currentStack.Pop();
        }

        public void AddExpression(IExpressionDescription expressionDescription)
        {
            var child = new TreeNode<IExpressionDescription>(expressionDescription);
            Console.WriteLine("Appending " + expressionDescription);
            
            if (_currentStack.Any())
            {
                Console.WriteLine(" to " + _currentStack.Peek());
                _currentStack.Peek().AddChild(child);
            }
            else
            {
                Console.WriteLine(" to ROOT ");
                _rootNode = child;
            }
            _currentStack.Push(child);
        }

        public TreeNode<IExpressionDescription> ConstructedExpression
        {
            get
            {
                if (_rootNode != null)
                {
                    Console.WriteLine("Expr "+_rootNode.Data.GetType()+" Returns " + _rootNode.Data);
                    IExpressionConstant constant = _rootNode.Data as IExpressionConstant;
                    if (constant != null)
                    {
                        Console.WriteLine(constant.Value);
                    }

                }
                return _rootNode;
            }
        }
       
    }
}
