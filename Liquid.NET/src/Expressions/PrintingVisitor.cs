using System;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    public class PrintingVisitor : ILiquidExpressionVisitor
    {
        public String Result { get; private set; }

        public String Traverse(TreeNode<IExpressionDescription> tree)
        {
            //_result.Push(new LiquidExpressionResult())
            foreach (var child in tree.Children)
            {
                // TODO: when this is all cleaned up, this can be removed:
                //IEnumerable<Option<ILiquidValue>> dummy = new List<Option<ILiquidValue>>();
                //child.Data.Accept(this, dummy);
                //child.Data.Accept(this);
                Traverse(child);
            }
            tree.Data.Accept(this);
            return Result;
        }

 
        public void Visit(LiquidValue expr)
        {
            Result = Result + expr.Value + ":" + expr.LiquidTypeName;
            //throw new System.NotImplementedException();
        }

        public void Visit(VariableReferenceTree expr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(VariableReference expr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(AndExpression expr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(NotExpression expr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(ContainsExpression expr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GroupedExpression expr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(EqualsExpression expr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(FalseExpression expr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GreaterThanExpression expr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(LessThanExpression expr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GreaterThanOrEqualsExpression expr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(LessThanOrEqualsExpression expr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(IsBlankExpression expr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(IsEmptyExpression expr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(IsPresentExpression expr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(NotEqualsExpression expr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(OrExpression expr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(LiquidExpression expr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(FilterSymbol expr)
        {
            //var argStrings = expr.Args.Select(x => Visit(x));
            Result += Result + " | " + expr.Name + " " + String.Join(",", expr.Args.ToString());
            
        }
    }
}
