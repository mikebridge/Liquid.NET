using System;
using System.Collections.Generic;
using System.Linq;

using Liquid.NET.Constants;
using Liquid.NET.Filters;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

//using ExpressionResult = Liquid.NET.Utils.Either<Liquid.NET.LiquidError, Liquid.NET.Utils.Option<Liquid.NET.Constants.IExpressionConstant>>;

namespace Liquid.NET
{
    // Take an AST Expression and turn it into something
    // that can be evaluated
    public static class LiquidExpressionEvaluator
    {
        
        public static LiquidExpressionResult Eval(
            TreeNode<LiquidExpression> expr, 
            SymbolTableStack symbolTableStack)
        {
            // Evaluate the children, depth first
            var leaves = expr.Children.Select(x => Eval(x, symbolTableStack)).ToList();
            if (leaves.Any(x => x.IsError))
            {
                return leaves.First(x => x.IsError); // TODO: maybe aggregate tehse
            }
            return Eval(expr.Data, leaves.Select(x => x.SuccessResult), symbolTableStack);
        }

        public static LiquidExpressionResult Eval(
            LiquidExpressionTree expressiontree, 
            SymbolTableStack symbolTableStack)
        {
            // Evaluate the children, depth first
            var leaves = expressiontree.ExpressionTree.Children.Select(x => Eval(x, symbolTableStack)).ToList();
            if (leaves.Any(x => x.IsError))
            {
                return leaves.First(x => x.IsError); // TODO: maybe aggregate tehse
            }
            // pass the results to the parent
            return Eval(expressiontree.ExpressionTree.Data, leaves.Select(x => x.SuccessResult), symbolTableStack);
        }

        public static LiquidExpressionResult Eval(
            LiquidExpression expression,
            IEnumerable<Option<IExpressionConstant>> leaves, 
            SymbolTableStack symbolTableStack)
        {

            LiquidExpressionResult objResult = expression.Expression.Eval(symbolTableStack, leaves);
            if (objResult.IsError)
            {
                return objResult;
            }
            // Compose a chain of filters, making sure type-casting
            // is done between them.

            var filterExpressionTuples = expression.FilterSymbols.Select(symbol => 
                new Tuple<FilterSymbol, IFilterExpression>(symbol, InstantiateFilter(symbolTableStack, symbol))).ToList();

            var erroringFilternames = filterExpressionTuples.Where(x => x.Item2 == null).Select(x => x.Item1).ToList();

            if (erroringFilternames.Any())
            {
                //throw new Exception("Missing filters..."); 
                //return ConstantFactory.CreateError<StringValue>();
                return LiquidExpressionResult.Error("Missing filters: " + String.Join(", ", erroringFilternames.Select(x => x.Name)));
            }

            var filterChain = FilterChain.CreateChain(
                objResult.GetType(),
                filterExpressionTuples.Select(x => x.Item2));

            // apply the composed function to the object
            return filterChain(objResult);

        }


        private static IFilterExpression InstantiateFilter(SymbolTableStack stack, FilterSymbol filterSymbol)
        {
            var filterType = stack.LookupFilterType(filterSymbol.Name);
            if (filterType == null)
            {
                //TODO: make this return an error filter or something?
                //throw new Exception("Invalid filter: " + filterSymbol.Name);
                //return new Tuple<String, IFilterExpression>(filterSymbol.Name, null);
                return null;
            }
            var expressionConstants = filterSymbol.Args.Select(x => x.Eval(stack, new List<Option<IExpressionConstant>>()));
            // TODO: Handle the error if any
            return FilterFactory.InstantiateFilter(filterSymbol.Name, filterType, expressionConstants.Select(x => x.SuccessResult));
        }


    }
}
