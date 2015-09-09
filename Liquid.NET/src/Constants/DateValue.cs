using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Expressions;
using Liquid.NET.Symbols;

namespace Liquid.NET.Constants
{
    public class DateValue : ExpressionConstant
    {
        private readonly DateTime? _val;

        public DateValue(DateTime? val)
        {
            _val = val;       
        }

        public override void Accept(IExpressionDescriptionVisitor visitor)
        {
            visitor.Visit(this);
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
