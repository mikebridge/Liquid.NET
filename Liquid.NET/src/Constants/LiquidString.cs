using System;
using System.Collections;

namespace Liquid.NET.Constants
{
    public class LiquidString : LiquidValue, IEnumerable
    {
        /// <summary>
        /// Create an instance of LiquidString.  Will return null if the wrapped string value is null.
        /// (You can pass the null value to a method accepting an Option&lt;ILiquidValue&gt; and it will
        /// be converted to None automatically).
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static LiquidString Create(String val)
        {
            return val == null ? null : new LiquidString(val);
        }

        private readonly String _val;

        public override Object Value { get { return _val; } }

        public String StringVal { get { return _val; } }


        private LiquidString(String val)
        {
            if (val == null)
            {
                throw new ArgumentNullException();
            }
            _val = val;
        }

        public override bool IsTrue
        {
            get { return _val != null; }
        }

        public override string LiquidTypeName { get { return "string"; } }

        /// <summary>
        /// Return a new LiquidString with str appended.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public LiquidString Join(LiquidString str)
        {
            return Create(StringVal + str.StringVal);
        }

        public IEnumerator GetEnumerator()
        {
            if (StringVal == null)
            {
                return new char[] { }.GetEnumerator(); // is this necessary?
            }
            return StringVal.ToCharArray().GetEnumerator();
        }

        public override string ToString()
        {
            return StringVal;
        }
    }
}
