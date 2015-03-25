using System;
using System.Collections.Generic;
using System.Linq;

using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Filters;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET
{
    public static class LiquidExpressionEvaluator
    {
        
        public static IExpressionConstant Eval(TreeNode<ObjectExpression> expr, SymbolTableStack symbolTableStack)
        {
            // Evaluate the children, depth first
            var leaves = expr.Children.Select(x => Eval(x, symbolTableStack));

            // pass the results to the parent
            //return Eval((ObjectExpression) symbolTableStack, (SymbolTableStack) leaves);
            //throw new ApplicationException("SCrewed this up");
            return Eval(expr.Data, leaves, symbolTableStack);
        }

        public static IExpressionConstant Eval(ObjectExpressionTree expressiontree, SymbolTableStack symbolTableStack)
        {
            // Evaluate the children, depth first
            var leaves = expressiontree.ExpressionTree.Children.Select(x => Eval(x, symbolTableStack));

            // pass the results to the parent
            return Eval(expressiontree.ExpressionTree.Data, leaves, symbolTableStack);
        }

        public static IExpressionConstant Eval(ObjectExpression expression, SymbolTableStack symbolTableStack)
        {
            return Eval(expression, new List<IExpressionConstant>(), symbolTableStack);
        }

        public static IExpressionConstant Eval(ObjectExpression expression, IEnumerable<IExpressionConstant> leaves, SymbolTableStack symbolTableStack)
        {


            //Console.WriteLine("In ObjectExpression.Eval, Expression is " + Expression);


            IExpressionConstant objResult = Eval(expression.Expression, leaves, symbolTableStack);
           
            // Compose a chain of filters, making sure type-casting
            // is done between them.
            var filterChain = FilterChain.CreateChain(
                objResult.GetType(),
                expression.FilterSymbols.Select(symbol => Lookup(symbolTableStack, symbol)));

            // apply the composed function to the object
            return filterChain(objResult);

        }


        public static IExpressionConstant Eval(IExpressionDescription expr, IEnumerable<IExpressionConstant> leaves, SymbolTableStack symbolTableStack)
        {
            return expr.Eval(symbolTableStack, leaves);
        }

        private static IFilterExpression Lookup(SymbolTableStack stack, FilterSymbol filterSymbol)
        {
            Console.WriteLine("LOOKUP");
            var expressionConstants = filterSymbol.Args.Select(x => x.Eval(stack, new List<IExpressionConstant>()));
            var filterExpression = stack.ReferenceFunction(filterSymbol.Name, expressionConstants);
            return filterExpression;
        }

        // obsolete?
        //        public static IExpressionConstant Eval(TreeNode<IExpressionDescription> expr, SymbolTableStack symbolTableStack)
        //        {
        //            // Evaluate the children, depth first
        //            var leaves = expr.Children.Select(x => Eval(x, symbolTableStack));
        //
        //            // pass the results to the parent
        //            return expr.Data.Eval(symbolTableStack, leaves);
        //
        //        }


    }
}
