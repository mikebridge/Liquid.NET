using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Liquid.NET.Constants;
using Liquid.NET.Expressions;
using Microsoft.SqlServer.Server;


namespace Liquid.NET.Filters
{
    public class RemoveFilter : FilterExpression<StringValue, StringValue>
    {
        private readonly StringValue _replacement;

        public RemoveFilter(StringValue replacement)
        {
            _replacement = replacement;
        }

        public override StringValue Apply(StringValue objectExpression)
        {
            // TODO: Return errors
            Console.WriteLine("APPLYING REMOVE "+_replacement.Value+"TO "+objectExpression.Value);
            if (_replacement == null || _replacement.Value == null)
            {
                var result = new StringValue(null) {ErrorMessage = "Please specify a replacement string."};
                return result;
            }

            return Remove((String) objectExpression.Value, _replacement);

        }

        private StringValue Remove(String orig, StringValue stringToRemove)
        {
            var result = orig.Replace((String) stringToRemove.Value, "");
            Console.WriteLine("  REMOVE RESULT : "+result);
            return new StringValue(result);
        }

    }
}
