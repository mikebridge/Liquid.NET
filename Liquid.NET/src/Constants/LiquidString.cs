using System;
using System.Collections;

namespace Liquid.NET.Constants
{
    public class LiquidString : LiquidValue, IEnumerable
    {
        private readonly String _val;

        public override Object Value { get { return _val; } }

        public String StringVal { get { return _val; } }


        public LiquidString(String val)
        {
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
            return new LiquidString(StringVal + str.StringVal);
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
