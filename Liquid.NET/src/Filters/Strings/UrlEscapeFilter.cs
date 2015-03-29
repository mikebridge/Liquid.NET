using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    public class UrlEscapeFilter : FilterExpression<IExpressionConstant, StringValue>
    {

        public override StringValue ApplyTo(IExpressionConstant liquidExpression)
        {
            //return StringUtils.Eval(liquidStringExpression, x => WebUtility.UrlEncode(x));
            // Dunno if this is right but it seems to do the same as CGI::escape
            return StringUtils.Eval(liquidExpression, Uri.EscapeUriString);
            
        }

    }
}
