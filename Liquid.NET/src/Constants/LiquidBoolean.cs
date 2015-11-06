using System;

namespace Liquid.NET.Constants
{
    public class LiquidBoolean : LiquidValue
    {
        private readonly bool _val;

        public LiquidBoolean(bool val)
        {
            _val = val;       
        }

        public override Object Value { get { return _val; } }

        public override bool IsTrue { get { return _val;  } }

        public override string LiquidTypeName
        {
            get { return "bool"; }
        }

        public bool BoolValue { get { return _val; } }

        public override string ToString()
        {
            return Value.ToString().ToLower();
        }
    }
}