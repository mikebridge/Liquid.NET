using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.NET
{

    public class LiquidError
    {
        public String Message { get; set; }
        public int Line { get; set; }
        public int CharPositionInLine { get; set; }
        public String OffendingSymbol { get; set; }

        public override string ToString()
        {
            return "line " + Line + ":" + CharPositionInLine + " at " + OffendingSymbol + ": " + Message;
        }
    }
}
