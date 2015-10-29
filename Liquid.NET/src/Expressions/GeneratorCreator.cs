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
        IEnumerable<IExpressionConstant> Eval(ITemplateContext templateContext);
    }

    public class StringValueIterableCreator : IIterableCreator
    {
        private readonly StringValue _stringValue;

        public StringValueIterableCreator(StringValue stringValue)
        {
            _stringValue = stringValue;
        }

        public IEnumerable<IExpressionConstant> Eval(ITemplateContext templateContext)
        {
//            return _stringValue.StringVal
//                .ToCharArray()
//                .Select(x => new StringValue("" + x));
            // In ruby liquid, it will not iterate through a string's characters---it will treat it as an array of one string.
            return new List<IExpressionConstant>{_stringValue};
        }
    }

    public class ArrayValueCreator : IIterableCreator
    {
        private readonly TreeNode<LiquidExpression> _arrayValueExpression;

        public ArrayValueCreator(TreeNode<LiquidExpression> arrayValueExpression)
        {
            _arrayValueExpression = arrayValueExpression;
        }

        public IEnumerable<IExpressionConstant> Eval(ITemplateContext templateContext)
        {
            var expressionConstant = LiquidExpressionEvaluator.Eval(_arrayValueExpression, templateContext);

            if (expressionConstant.IsError || !expressionConstant.SuccessResult.HasValue)
            {
                return new List<IExpressionConstant>();
            }
            var castResult = ValueCaster.Cast<IExpressionConstant, ArrayValue>(expressionConstant.SuccessResult.Value);
            if (castResult.IsError)
            {
                // ??
                return new List<IExpressionConstant>();
            }
            else
            {
                return castResult.SuccessValue<ArrayValue>().Select(x => x.HasValue? x.Value : null);
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

        public IEnumerable<IExpressionConstant> Eval(ITemplateContext templateContext)
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
            var generatorValue = new GeneratorValue(startValue, endValue);
            return generatorValue;

        }

        private NumericValue ValueAsNumeric(TreeNode<LiquidExpression> expr, ITemplateContext templateContext)
        {
            var liquidExpressionResult = LiquidExpressionEvaluator.Eval(expr, templateContext);
            if (liquidExpressionResult.IsError)
            {
                return NumericValue.Create(0);
            }
            var valueAsNumeric = liquidExpressionResult.SuccessResult.Value as NumericValue;

            if (valueAsNumeric == null)
            {
                var castedValue =
                    ValueCaster.Cast<IExpressionConstant, NumericValue>(liquidExpressionResult.SuccessResult.Value);

                return liquidExpressionResult.IsSuccess && liquidExpressionResult.SuccessResult.HasValue
                    ? castedValue.SuccessValue<NumericValue>()
                    : NumericValue.Create(0);
            }
            else
            {
                return valueAsNumeric;
            }

        }
    }
}
