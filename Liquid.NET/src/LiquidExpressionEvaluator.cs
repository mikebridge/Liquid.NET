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
            // until the expression is changed to an option type, an expression may be null.  If it's null, it evaluates to null.
            if (expression.Expression == null)
            {
                //return null;
                return LiquidExpressionResult.Success(new None<IExpressionConstant>());
            }


            LiquidExpressionResult objResult = expression.Expression.Eval(templateContext, leaves);
            if (objResult.IsError)
            {
                return objResult;
            }
            // Compose a chain of filters, making sure type-casting
            // is done between them.
            IEnumerable<Tuple<FilterSymbol, IFilterExpression>> filterExpressionTuples;
            try
            {
                filterExpressionTuples = expression.FilterSymbols.Select(symbol =>
                    new Tuple<FilterSymbol, IFilterExpression>(symbol, InstantiateFilter(templateContext, symbol)))
                    .ToList();
            }
            catch (Exception ex)
            {
                return LiquidExpressionResult.Error(ex.Message);
            }
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
            
            return filterChain(objResult.SuccessResult);

        }


        private static IFilterExpression InstantiateFilter(ITemplateContext templateContext, FilterSymbol filterSymbol)
        {
            var filterType = templateContext.SymbolTableStack.LookupFilterType(filterSymbol.Name);
            if (filterType == null)
            {
                //TODO: make this return an error filter or something?
                //throw new Exception("Invalid filter: " + filterSymbol.Name);
                //return new Tuple<String, IFilterExpression>(filterSymbol.Name, null);
                return null;
            }
            var expressionConstants = filterSymbol.Args.Select(x => x.Eval(templateContext, new List<Option<IExpressionConstant>>())).ToList();
            // TODO: Handle the error if any
            //return FilterFactory.InstantiateFilter(filterSymbol.Name, filterType, expressionConstants.Select(x => x.IsSuccess? x.SuccessResult));
            if (expressionConstants.Any(x => x.IsError))
            {
                //return null; // eval-ing a constant failed.
                throw new Exception(String.Join("," ,expressionConstants.Where(x => x.IsError).Select(x => x.ErrorResult.Message)));
            }
            return FilterFactory.InstantiateFilter(filterSymbol.Name, filterType, expressionConstants.Select(x => x.SuccessResult));
        }


    }
}
