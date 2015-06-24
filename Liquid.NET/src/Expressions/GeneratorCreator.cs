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
        IEnumerable<IExpressionConstant> Eval(SymbolTableStack symbolTableStack);
    }

    public class StringValueIterableCreator : IIterableCreator
    {
        private readonly StringValue _stringValue;

        public StringValueIterableCreator(StringValue stringValue)
        {
            _stringValue = stringValue;
        }

        public IEnumerable<IExpressionConstant> Eval(SymbolTableStack symbolTableStack)
        {
            return _stringValue.StringVal
                .ToCharArray()
                .Select(x => new StringValue("" + x));
            
        }
    }

    public class ArrayValueCreator : IIterableCreator
    {
        private readonly TreeNode<LiquidExpression> _arrayValueExpression;

        public ArrayValueCreator(TreeNode<LiquidExpression> arrayValueExpression)
        {
            _arrayValueExpression = arrayValueExpression;
        }

//        private readonly ArrayValue _arrayValue;
//
//        public ArrayValueCreator(ArrayValue arrayValue)
//        {
//            _arrayValue = arrayValue;
//        }

        // TODO: SHould this return a LiquidExpressionResult?
        public IEnumerable<IExpressionConstant> Eval(SymbolTableStack symbolTableStack)
        {            
            var expressionConstant = LiquidExpressionEvaluator.Eval(_arrayValueExpression,
                symbolTableStack);
            
            return
                ValueCaster.Cast<IExpressionConstant, ArrayValue>(expressionConstant.SuccessResult.Value);
        }
    }

    public class GeneratorCreator : IIterableCreator
    {
        private readonly TreeNode<LiquidExpression> _startExpression;
        private readonly TreeNode<LiquidExpression> _endExpression;

        public GeneratorCreator(TreeNode<LiquidExpression> start, TreeNode<LiquidExpression> end)
        {
            _startExpression = start;
            _endExpression = end;
        }

        public IEnumerable<IExpressionConstant> Eval(SymbolTableStack symbolTableStack)
        {
            var startValue = ValueAsNumeric(_startExpression, symbolTableStack);
            var endValue = ValueAsNumeric(_endExpression, symbolTableStack);
            //Console.WriteLine("*** Generating sequence from "+ startValue.IntValue+ " to " +endValue.IntValue);
            var generatorValue = new GeneratorValue(startValue, endValue);
            return generatorValue;

        }

        private NumericValue ValueAsNumeric(TreeNode<LiquidExpression> expr, SymbolTableStack symbolTableStack)
        {
            var liquidExpressionResult = LiquidExpressionEvaluator.Eval(expr, symbolTableStack);
            return liquidExpressionResult.SuccessResult.HasValue ? ValueCaster.Cast<IExpressionConstant, NumericValue>(liquidExpressionResult.SuccessResult.Value)
                : new NumericValue(0);
        }
    }
}
