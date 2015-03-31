using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Liquid.NET.Filters;
using Liquid.NET.Symbols;
using Liquid.NET.Tags;
using Liquid.NET.Tags.Custom;
using Liquid.NET.Utils;

namespace Liquid.NET
{
    public static class LiquidExpressionEvaluator
    {
        
        public static IExpressionConstant Eval(TreeNode<LiquidExpression> expr, SymbolTableStack symbolTableStack)
        {
            // Evaluate the children, depth first
            var leaves = expr.Children.Select(x => Eval(x, symbolTableStack));

            // pass the results to the parent
            //return Eval((LiquidExpression) symbolTableStack, (SymbolTableStack) leaves);
            //throw new ApplicationException("SCrewed this up");
            return Eval(expr.Data, leaves, symbolTableStack);
        }

        public static IExpressionConstant Eval(LiquidExpressionTree expressiontree, SymbolTableStack symbolTableStack)
        {
            // Evaluate the children, depth first
            var leaves = expressiontree.ExpressionTree.Children.Select(x => Eval(x, symbolTableStack));

            // pass the results to the parent
            return Eval(expressiontree.ExpressionTree.Data, leaves, symbolTableStack);
        }

        public static IExpressionConstant Eval(LiquidExpression expression, SymbolTableStack symbolTableStack)
        {
            return Eval(expression, new List<IExpressionConstant>(), symbolTableStack);
        }

        public static IExpressionConstant Eval(LiquidExpression expression, IEnumerable<IExpressionConstant> leaves, SymbolTableStack symbolTableStack)
        {


            //Console.WriteLine("In LiquidExpression.Eval, Expression is " + Expression);


            IExpressionConstant objResult = Eval(expression.Expression, leaves, symbolTableStack);
           
            // Compose a chain of filters, making sure type-casting
            // is done between them.
            var filterChain = FilterChain.CreateChain(
                objResult.GetType(),
                expression.FilterSymbols.Select(symbol => InstantiateFilter(symbolTableStack, symbol)));

            // apply the composed function to the object
            return filterChain(objResult);

        }


        public static IExpressionConstant Eval(IExpressionDescription expr, IEnumerable<IExpressionConstant> leaves, SymbolTableStack symbolTableStack)
        {
            return expr.Eval(symbolTableStack, leaves);
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

//        private static ICustomTagRenderer InstantiateCustomTag(SymbolTableStack stack, CustomTag customTagSymbol)
//        {
//            var customTagRendererType = stack.LookupCustomTagRendererType(customTagSymbol.TagName);
//            if (customTagRendererType == null)
//            {
//                //TODO: make this return an error filter or something?
//                throw new Exception("Invalid filter: " + customTagSymbol.TagName);
//            }
//            //return FilterFactory.InstantiateFilter(filterSymbol.Name, filterType, expressionConstants);
//
//            return CustomTagRendererFactory.Create(customTagSymbol.TagName);            
//        }

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
