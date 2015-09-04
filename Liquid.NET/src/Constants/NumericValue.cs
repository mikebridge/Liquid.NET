using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Liquid.NET.Expressions;
using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{
    internal class NumericValue<T> : NumericValue
    {
        private readonly T _value;

        /// <param name="value"></param>
        /// <param name="isInt">It is is an int, long or bigint</param>
        internal NumericValue(T value, bool isInt)
        {
            _value = value;
            IsInt = isInt;
        }

        public override object Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Is the value an int/long/BigInteger
        /// </summary>
        public override bool IsInt
        {
            get;
            protected set;
        }
    

        public override bool IsTrue
        {
            //get { return DecimalValue != 0m; }
            get { return true; }
        }

        public override int IntValue
        {
            get
            {
                return ToInt((dynamic) _value); // ValueCaster.ConvertToInt(_value);
            }
        }

        public override BigInteger BigIntValue
        {
            get
            {
                return ToBigInt((dynamic)_value); // ValueCaster.ConvertToInt(_value);
            }
        }

        public override decimal DecimalValue
        {
            get
            {
                return (decimal) (dynamic) Value; // TODO: This may overflow.
            }
        }

        #region ToBigInt
        private static BigInteger ToBigInt(int value)
        {
            return new BigInteger(value);
        }

        public static BigInteger ToBigInt(BigInteger value)
        {
            return value;
        }

        public static BigInteger ToBigInt(Int64 value)
        {
            return new BigInteger(value);
        }

        public static BigInteger ToBigInt(decimal value)
        {
            return ValueCaster.ConvertToBigInt(value);
        }
        #endregion

        #region ToInt
        private static int ToInt(int value)
        {
            return value;
        }

        public static int ToInt(BigInteger value)
        {
            return (int)value;
        }

        public static int ToInt(long value)
        {
            return Convert.ToInt32(value);
        }

        public static int ToInt(decimal value)
        {
            return ValueCaster.ConvertToInt(value);
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            return ToString((dynamic)this.Value);
        }

        private String ToString(int obj)
        {
            return obj.ToString();
        }

        private String ToString(BigInteger obj)
        {
            return obj.ToString();
        }

        private String ToString(long obj)
        {
            return obj.ToString();
        }

        private String ToString(decimal obj)
        {
            return obj.ToString("0.0###");
        }
        #endregion
 
    }

    public abstract class NumericValue: ExpressionConstant
    {
        public static NumericValue Create(int val)
        {
            return new NumericValue<int>(val, isInt: true);
        }

        public static NumericValue Create(long val)
        {
            return new NumericValue<long>(val, isInt: true);
        }

        public static NumericValue Create(BigInteger val)
        {
            return new NumericValue<BigInteger>(val, isInt: true);
        }

        public static NumericValue Create(decimal val)
        {
            return new NumericValue<decimal>(val, isInt: false);
        }

        public abstract decimal DecimalValue { get; }

        public abstract int IntValue { get;}

        public abstract BigInteger BigIntValue { get; }

        public abstract bool IsInt { get; protected set; }
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

        //private readonly Decimal _val;

        //public override Object Value { get { return _val; } }

        //public decimal DecimalValue { get { return _val; } }

     



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

//        public override bool IsTrue
//        {
//            get { return _val != 0; }
//        }

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
