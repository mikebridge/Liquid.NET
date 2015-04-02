using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Symbols;

namespace Liquid.NET.Constants
{
    public class KeyValueArg : ExpressionConstant
    {
//        public override IExpressionConstant Eval(SymbolTableStack symbolTableStack, IEnumerable<IExpressionConstant> expressions)
//        {
//            throw new NotImplementedException();
//        }

        public override object Value
        {
            get { throw new NotImplementedException(); }
        }

        public override bool IsTrue
        {
            get { throw new NotImplementedException(); }
        }
    }
}
