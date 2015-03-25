using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;


namespace Liquid.NET.Filters
{
    public class UpCaseFilter : FilterExpression<StringValue, StringValue>
    {
//        public UpCaseFilter(string name, IList<IObjectExpression> args, string rawArgs) : base(name, args, rawArgs)
//        {
//        }

        public override StringValue Apply(StringValue stringExpression)
        {
            // TODO: parameterize the type
            String val = (String) stringExpression.Value;
            return new StringValue(val.ToUpper());
        }
    }
}
