using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Symbols;
using Liquid.NET.Utils;

namespace Liquid.NET.Expressions
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public interface IIterableCreator
    {
        IEnumerable<ILiquidValue> Eval(ITemplateContext templateContext);
    }

    public class StringValueIterableCreator : IIterableCreator
    {
        private readonly LiquidString _liquidString;

        public StringValueIterableCreator(LiquidString liquidString)
        {
            _liquidString = liquidString;
        }

        public IEnumerable<ILiquidValue> Eval(ITemplateContext templateContext)
        {
//            return _liquidString.StringVal
//                .ToCharArray()
//                .Select(x => new LiquidString("" + x));
            // In ruby liquid, it will not iterate through a string's characters---it will treat it as an array of one string.
            return new List<ILiquidValue>{_liquidString};
        }
    }

    public class ArrayValueCreator : IIterableCreator
    {
        private readonly TreeNode<LiquidExpression> _liquidCollectionExpression;

        public ArrayValueCreator(TreeNode<LiquidExpression> liquidCollectionExpression)
        {
            _liquidCollectionExpression = liquidCollectionExpression;
        }

        public IEnumerable<ILiquidValue> Eval(ITemplateContext templateContext)
        {
            var expressionConstant = LiquidExpressionEvaluator.Eval(_liquidCollectionExpression, templateContext);

            if (expressionConstant.IsError || !expressionConstant.SuccessResult.HasValue)
            {
                return new List<ILiquidValue>();
            }
            var castResult = ValueCaster.Cast<ILiquidValue, LiquidCollection>(expressionConstant.SuccessResult.Value);
            if (castResult.IsError)
            {
                // ??
                return new List<ILiquidValue>();
            }
            else
            {
                return castResult.SuccessValue<LiquidCollection>().Select(x => x.HasValue? x.Value : null);
            }
        }
    }

    public class GeneratorCreator : IIterableCreator
    {
        private TreeNode<LiquidExpression> _startExpression;
        private TreeNode<LiquidExpression> _endExpression;

//        public GeneratorCreator(TreeNode<LiquidExpression> start, TreeNode<LiquidExpression> end)
//        {
//            _startExpression = start;
//            _endExpression = end;
//        }

        public TreeNode<LiquidExpression> StartExpression
        {
            get { return _startExpression; }
            set { _startExpression = value; }
        }

        public TreeNode<LiquidExpression> EndExpression
        {
            get { return _endExpression; }
            set { _endExpression = value; }
        }

        public IEnumerable<ILiquidValue> Eval(ITemplateContext templateContext)
        {
            if (_startExpression == null)
            {
                // this shouldn't happen
                throw new Exception("The Generator start expression is null");
            }
            if (_endExpression == null)
            {
                // this shouldn't happen
                throw new Exception("The Generator end expression is null");
            }
            var startValue = ValueAsNumeric(_startExpression, templateContext);
            var endValue = ValueAsNumeric(_endExpression, templateContext);
            //Console.WriteLine("*** Generating sequence from "+ startValue.IntValue+ " to " +endValue.IntValue);
            var generatorValue = new LiquidRange(startValue, endValue);
            return generatorValue;

        }

        private LiquidNumeric ValueAsNumeric(TreeNode<LiquidExpression> expr, ITemplateContext templateContext)
        {
            var liquidExpressionResult = LiquidExpressionEvaluator.Eval(expr, templateContext);
            if (liquidExpressionResult.IsError)
            {
                return LiquidNumeric.Create(0);
            }
            var valueAsNumeric = liquidExpressionResult.SuccessResult.Value as LiquidNumeric;

            if (valueAsNumeric == null)
            {
                var castedValue =
                    ValueCaster.Cast<ILiquidValue, LiquidNumeric>(liquidExpressionResult.SuccessResult.Value);

                return liquidExpressionResult.IsSuccess && liquidExpressionResult.SuccessResult.HasValue
                    ? castedValue.SuccessValue<LiquidNumeric>()
                    : LiquidNumeric.Create(0);
            }
            else
            {
                return valueAsNumeric;
            }

        }
    }
}
