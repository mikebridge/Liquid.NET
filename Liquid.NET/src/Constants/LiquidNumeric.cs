using System;
using System.Numerics;
using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{

    public abstract class LiquidNumeric: LiquidValue
    {


        public static LiquidNumeric Create(int val)
        {
            return new IntLiquidNumeric(val);
        }

        public static LiquidNumeric Create(long val)
        {
            return new LongLiquidNumeric(val);
        }

        public static LiquidNumeric Create(BigInteger val)
        {
            return new BigIntegerLiquidNumeric(val);
        }

        public static LiquidNumeric Create(decimal val)
        {
            return new DecimalLiquidNumeric(val);
        }

        public override bool IsTrue
        {
            get { return true; }
        }

        public abstract decimal DecimalValue { get; }

        public abstract int IntValue { get;}

        public abstract BigInteger BigIntValue { get; }

        public abstract long LongValue { get; }

        public abstract bool IsInt { get; }

        public static LiquidExpressionResult Parse(String str)
        {

            return ValueCaster.Cast<ILiquidValue, LiquidNumeric>(LiquidString.Create(str));

        }

        public override string LiquidTypeName { get { return "numeric"; } }

        public override bool Equals(object obj)
        {
            var nv = obj as LiquidNumeric;
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

        private bool Equals(LiquidNumeric nv)
        {
            return nv != null && nv.DecimalValue == DecimalValue;
        }

        
    }

    public class IntLiquidNumeric : LiquidNumeric
    {
        private readonly int _val;

        public IntLiquidNumeric(int val)
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

        public override long LongValue
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

    public class DecimalLiquidNumeric : LiquidNumeric
    {
        private readonly decimal _val;

        public DecimalLiquidNumeric(decimal val)
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

        public override long LongValue
        {
            get { return ValueCaster.ConvertToLong(_val); }
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

    public class LongLiquidNumeric : LiquidNumeric
    {
        private readonly long _val;

        public LongLiquidNumeric(long val)
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

        public override long LongValue
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

    public class BigIntegerLiquidNumeric : LiquidNumeric
    {
        private readonly BigInteger _val;

        public BigIntegerLiquidNumeric(BigInteger val)
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

        /// <summary>
        /// This may overflow!
        /// </summary>
        public override long LongValue
        {
            get { return ValueCaster.ConvertToLong((decimal)_val); }
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
