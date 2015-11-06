using System;

namespace Liquid.NET.Constants
{
    public class LiquidDate : LiquidValue
    {
        private readonly DateTime? _val;

        public LiquidDate(DateTime? val)
        {
            _val = val;       
        }

        public override Object Value { get { return _val; } }

        public override bool IsTrue { get { return _val != null;  } }

        public override string LiquidTypeName
        {
            get { return "date"; }
        }

        public DateTime? DateTimeValue { get { return _val; } }

        public override string ToString()
        {
            return Value.ToString().ToLower();
        }
    }
}
