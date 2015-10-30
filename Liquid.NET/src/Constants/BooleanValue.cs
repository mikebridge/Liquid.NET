using System;
using Liquid.NET.Expressions;

namespace Liquid.NET.Constants
{
    public class BooleanValue : ExpressionConstant
    {
        private readonly bool _val;

        public BooleanValue(bool val)
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