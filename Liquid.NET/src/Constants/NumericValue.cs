using System;
using System.Numerics;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{

    public abstract class NumericValue: ExpressionConstant
    {


        public static NumericValue Create(int val)
        {
            return new IntNumericValue(val);
            //return new NumericValue<int>(val, isInt: true);
        }

        public static NumericValue Create(long val)
        {
            //return new NumericValue<long>(val, isInt: true);
            return new LongNumericValue(val);
        }

        public static NumericValue Create(BigInteger val)
        {
            //return new NumericValue<BigInteger>(val, isInt: true);
            return new BigIntegerNumericValue(val);
        }

        public static NumericValue Create(decimal val)
        {
            //return new NumericValue<decimal>(val, isInt: false);
            return new DecimalNumericValue(val);
        }

        public override bool IsTrue
        {
            get { return true; }
        }

        public abstract decimal DecimalValue { get; }

        public abstract int IntValue { get;}

        public abstract BigInteger BigIntValue { get; }

        public abstract bool IsInt { get; }

        public static LiquidExpressionResult Parse(String str)
        {
            try
            {
                return ValueCaster.Cast<IExpressionConstant, NumericValue>(new StringValue(str));
            }
            catch
            {
                // This shouldn't actually fail...
                //var errorSymbol = NumericValue.Create(0) {ErrorMessage = "Unable to convert '" + str + "' to a number."};
                //return errorSymbol;
                return LiquidExpressionResult.Error("Unable to convert '" + str + "' to a number.");
            }
        }

        public override string LiquidTypeName { get { return "numeric"; } }

        public override void Accept(IExpressionDescriptionVisitor expressionDescriptionVisitor)
        {
            expressionDescriptionVisitor.Visit(this);
        }
        public override bool Equals(object obj)
        {
            var nv = obj as NumericValue;
            return Equals(nv);
        }

        public override int GetHashCode()
        {
            unchecked 
            {         
                var hash = 27;
                hash = (13 * hash) + DecimalValue.GetHashCode();
                return hash;
            }
        }

        private bool Equals(NumericValue nv)
        {
            return nv != null && nv.DecimalValue == DecimalValue;
        }

        
    }

    public class IntNumericValue : NumericValue
    {
        private readonly int _val;

        public IntNumericValue(int val)
        {
            _val = val;
        }

        public override object Value
        {
            get { return _val; }
        }

        public override decimal DecimalValue
        {
            get { return _val; }
        }

        public override int IntValue
        {
            get { return _val; }
        }

        public override BigInteger BigIntValue
        {
            get { return new BigInteger(_val); }
        }

        public override string ToString()
        {
            return _val.ToString();
        }

        public override bool IsInt { get { return true; } }
    }

    public class DecimalNumericValue : NumericValue
    {
        private readonly decimal _val;

        public DecimalNumericValue(decimal val)
        {
            _val = val;
        }

        public override object Value
        {
            get { return _val; }
        }



        public override decimal DecimalValue
        {
            get { return _val; }
        }

        public override int IntValue
        {
            get { return ValueCaster.ConvertToInt(_val); }
        }

        public override BigInteger BigIntValue
        {
            get { return ValueCaster.ConvertToBigInt(_val); }
        }

        public override string ToString()
        {
            return _val.ToString("0.0###");
        }

        public override bool IsInt { get { return false; } }
    }

    public class LongNumericValue : NumericValue
    {
        private readonly long _val;

        public LongNumericValue(long val)
        {
            _val = val;
        }

        public override object Value
        {
            get { return _val; }
        }

        public override decimal DecimalValue
        {
            get { return _val; }
        }

        /// <summary>
        /// This may overflow!
        /// </summary>
        public override int IntValue
        {
            get { return ValueCaster.ConvertToInt(_val); }
        }

        public override BigInteger BigIntValue
        {
            get { return new BigInteger(_val); }
        }

        public override string ToString()
        {
            return _val.ToString();
        }

        public override bool IsInt { get { return true; } }
    }

    public class BigIntegerNumericValue : NumericValue
    {
        private readonly BigInteger _val;

        public BigIntegerNumericValue(BigInteger val)
        {
            _val = val;
        }

        public override object Value
        {
            get { return _val; }
        }

        public override decimal DecimalValue
        {
            get { return (decimal) _val; }
        }

        /// <summary>
        /// This may overflow!
        /// </summary>
        public override int IntValue
        {
            get { return ValueCaster.ConvertToInt((decimal)_val); }
        }

        public override BigInteger BigIntValue
        {
            get { return _val; }
        }

        public override string ToString()
        {
            return _val.ToString();
        }

        public override bool IsInt { get { return true; } }
    }

}
