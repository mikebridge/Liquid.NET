using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.NET
{

    public class LiquidError
    {
        public String TokenSource { get; set; }
        public String Context { get; set; }
        public String Message { get; set; }
        public int Line { get; set; }
        public int CharPositionInLine { get; set; }
        public String OffendingSymbol { get; set; }

        public override string ToString()
        {
            String filename = TokenSource == null ? "" : TokenSource + ": ";
            string result = filename+ "line " + Line + ":" + CharPositionInLine + " at " + OffendingSymbol + ": " + Message;
            if (String.IsNullOrEmpty(Context))
            {
                result += "\r\n     " + Context;
            }
            return result;
        }
    }
}
