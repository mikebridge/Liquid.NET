using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.NET.Constants
{
    /// <summary>
    //
    /// </summary>
    [Obsolete]
    public class NilValue : ExpressionConstant
    {
        public override object Value
        {
            get { return null; }
        }

        public override bool IsTrue
        {
            get { return false; }
        }

        public override string LiquidTypeName
        {
            get { return "null"; }
        }
    }
}
