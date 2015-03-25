using System;
using System.Collections.Generic;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;

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

        public static NumericValue Parse(String str)
        {
            try
            {
                return new NumericValue(Convert.ToDecimal(str));
            }
            catch
            {
                var errorSymbol = new NumericValue(0) {ErrorMessage = "Unable to convert " + str + " to a number."};
                return errorSymbol;
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

        // Todo: move all these up one level.
        public override IExpressionConstant Eval(SymbolTableStack symbolTableStack, IEnumerable<IExpressionConstant> childresults)
        {
            return this;
        }

        public static NumericValue CreateError(string message)
        {
            var result = new NumericValue(0);
            result.ErrorMessage = message;
            return result;
        }
    }
}
