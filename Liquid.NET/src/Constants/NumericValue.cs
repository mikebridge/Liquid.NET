using System;

using Liquid.NET.Expressions;
using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{
    public class NumericValue: ExpressionConstant
    {
        private readonly Decimal _val;

        public override Object Value { get { return _val; } }

        public decimal DecimalValue { get { return _val; } }

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
                //return ValueCaster.Cast<StringValue,NumericValue>((dynamic) new StringValue(str));
                //return new NumericValue(Convert.ToDecimal(str));
            }
            catch
            {
                // This shouldn't actually fail...
                //var errorSymbol = new NumericValue(0) {ErrorMessage = "Unable to convert '" + str + "' to a number."};
                //return errorSymbol;
                return LiquidExpressionResult.Error("Unable to convert '" + str + "' to a number.");
            }
        }

        public NumericValue(decimal val)
        {
            _val = val;
        }


        public override bool IsTrue
        {
            get { return _val != 0; }
        }


        



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
