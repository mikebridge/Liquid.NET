using System;
using System.Dynamic;
using System.Numerics;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{
    internal class NumericValue<T> : NumericValue
    {
        private readonly T _value;

        internal NumericValue(T value)
        {
            _value = value;
        }
    }

    public abstract class NumericValue: ExpressionConstant
    {
        public static NumericValue Create(int val)
        {
            return new NumericValue<int>(val);
        }

        public static NumericValue Create(long val)
        {
            return new NumericValue<long>(val);
        }

        public static NumericValue Create(BigInteger val)
        {
            return new NumericValue<BigInteger>(val);
        }

        public static NumericValue Create(decimal val)
        {
            return new NumericValue<decimal>(val);
        }
//
//        protected NumericValue(int val)
//        {
//            _val = val;
//            IsInt = true;
//        }
//
//        protected NumericValue(decimal val)
//        {
//            _val = val;
//            IsInt = false;
//        }

        private readonly Decimal _val;

        public override Object Value { get { return _val; } }

        public decimal DecimalValue { get { return _val; } }

        public bool IsInt { get; private set; }

        public int IntValue
        {
            get {
                return ValueCaster.ConvertToInt(_val);
            }
        }

        public static LiquidExpressionResult Parse(String str)
        {
            try
            {
                return ValueCaster.Cast<IExpressionConstant, NumericValue>(new StringValue(str));
            }
            catch
            {
                // This shouldn't actually fail...
                //var errorSymbol = new NumericValue(0) {ErrorMessage = "Unable to convert '" + str + "' to a number."};
                //return errorSymbol;
                return LiquidExpressionResult.Error("Unable to convert '" + str + "' to a number.");
            }
        }

        public override bool IsTrue
        {
            get { return _val != 0; }
        }

        public override string LiquidTypeName { get { return "numeric"; } }


        public override void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
        {
            expressionDescriptionVisitor.Visit(this);
        }

//        public static NumericValue CreateError(string message)
//        {
//            var result = new NumericValue(0);
//            result.ErrorMessage = message;
//            return result;
//        }
    }
}
