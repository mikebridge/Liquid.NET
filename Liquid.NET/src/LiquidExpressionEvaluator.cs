using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            ITemplateContext templateContext)
        {
            // Evaluate the children, depth first
            var leaves = expr.Children.Select(x => Eval(x, templateContext)).ToList();
            if (leaves.Any(x => x != null && x.IsError))
            {
                return leaves.First(x => x.IsError); // TODO: maybe aggregate tehse
            }
            return Eval(expr.Data, leaves.Select(x => x == null ? new None<IExpressionConstant>() : x.SuccessResult), templateContext);
        }

        public static LiquidExpressionResult Eval(
            LiquidExpressionTree expressiontree, 
            ITemplateContext templateContext)
        {
            // Evaluate the children, depth first
            var leaves = expressiontree.ExpressionTree.Children.Select(x => Eval(x, templateContext)).ToList();
            if (leaves.Any(x => x.IsError))
            {
                return leaves.First(x => x.IsError); // TODO: maybe aggregate tehse
            }
            // pass the results to the parent
            return Eval(expressiontree.ExpressionTree.Data, leaves.Select(x => x.SuccessResult), templateContext);
        }

        public static LiquidExpressionResult Eval(
            LiquidExpression expression,
            IEnumerable<Option<IExpressionConstant>> leaves, 
            ITemplateContext templateContext)
        {
            // calculate the first part of the expression
            var objResult = expression.Expression == null ? 
                LiquidExpressionResult.Success(new None<IExpressionConstant>()) : 
                expression.Expression.Eval(templateContext, leaves);

            if (objResult.IsError)
            {
                return objResult;
            }

            // Compose a chain of filters, making sure type-casting
            // is done between them.
            //IEnumerable<Tuple<FilterSymbol, IFilterExpression>> filterExpressionTuples;
//            try
//            {
            var  filterExpressionTuples = expression.FilterSymbols.Select(symbol =>
                    new Tuple<FilterSymbol, Try<IFilterExpression>>(symbol, InstantiateFilter(templateContext, symbol)))
                    .ToList();
            //}
            //catch (Exception ex)
            //{
             //   return LiquidExpressionResult.Error(ex.Message);
            //}
            if (filterExpressionTuples.Any(x => x.Item2.IsFailure))
            {
                // just return the first error.
                return LiquidExpressionResult.Error(filterExpressionTuples.First().Item2.Exception.Message);
            }

            var erroringFilternames = filterExpressionTuples.Where(x => x.Item2 == null).Select(x => x.Item1).ToList();

            if (erroringFilternames.Any())
            {
                //throw new Exception("Missing filters..."); 
                //return ConstantFactory.CreateError<LiquidString>();
                return LiquidExpressionResult.Error("Missing filters: " + String.Join(", ", erroringFilternames.Select(x => x.Name)));
            }

            var filterChain = FilterChain.CreateChain(
                objResult.GetType(),
                templateContext,
                filterExpressionTuples.Select(x => x.Item2.Value));

            // apply the composed function to the object
            
            return filterChain(objResult.SuccessResult);

        }


        private static Try<IFilterExpression> InstantiateFilter(ITemplateContext templateContext, FilterSymbol filterSymbol)
        {
            var filterType = templateContext.SymbolTableStack.LookupFilterType(filterSymbol.Name);
//            if (filterType == null)
//            {
//                return null;
//            }
            var expressionConstants = filterSymbol.Args.Select(x => Eval(x, templateContext)).ToList();

            if (expressionConstants.Any(x => x.IsError))
            {
                //return null; // eval-ing a constant failed.  TODO: Is this actually possible?
                //throw new Exception(String.Join("," ,expressionConstants.Where(x => x.IsError).Select(x => x.ErrorResult.Message)));
                return
                    new Failure<IFilterExpression>(
                        new Exception(String.Join(",",
                            expressionConstants.Where(x => x.IsError).Select(x => x.ErrorResult.Message))));
            }
            return FilterFactory.InstantiateFilter(filterSymbol.Name, filterType, expressionConstants.Select(x => x.SuccessResult));
        }


    }
}
