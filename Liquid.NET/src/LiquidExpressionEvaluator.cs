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
        
        public static IExpressionConstant Eval(
            TreeNode<LiquidExpression> expr, 
            SymbolTableStack symbolTableStack)
        {
            // Evaluate the children, depth first
            var leaves = expr.Children.Select(x => Eval(x, symbolTableStack));

            return Eval(expr.Data, leaves, symbolTableStack);
        }

        public static IExpressionConstant Eval(
            LiquidExpressionTree expressiontree, 
            SymbolTableStack symbolTableStack)
        {
            // Evaluate the children, depth first
            var leaves = expressiontree.ExpressionTree.Children.Select(x => Eval(x, symbolTableStack));

            // pass the results to the parent
            return Eval(expressiontree.ExpressionTree.Data, leaves, symbolTableStack);
        }

        public static IExpressionConstant Eval(
            LiquidExpression expression, 
            IEnumerable<IExpressionConstant> leaves, 
            SymbolTableStack symbolTableStack)
        {

            IExpressionConstant objResult = expression.Expression.Eval(symbolTableStack, leaves);
           
            // Compose a chain of filters, making sure type-casting
            // is done between them.
            
            var filterChain = FilterChain.CreateChain(
                objResult.GetType(),
                expression.FilterSymbols.Select(symbol => InstantiateFilter(symbolTableStack, symbol)));

            // apply the composed function to the object
            return filterChain(objResult);

        }


        private static IFilterExpression InstantiateFilter(SymbolTableStack stack, FilterSymbol filterSymbol)
        {
            var filterType = stack.LookupFilterType(filterSymbol.Name);
            if (filterType == null)
            {
                //TODO: make this return an error filter or something?
                throw new Exception("Invalid filter: " + filterSymbol.Name);
            }
            var expressionConstants = filterSymbol.Args.Select(x => x.Eval(stack, new List<IExpressionConstant>()));
            return FilterFactory.InstantiateFilter(filterSymbol.Name, filterType, expressionConstants);
        }


    }
}
