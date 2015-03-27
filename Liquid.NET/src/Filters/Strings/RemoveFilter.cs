using System;
using Liquid.NET.Constants;

namespace Liquid.NET.Filters.Strings
{
    public class RemoveFilter : FilterExpression<StringValue, StringValue>
    {
        private readonly StringValue _replacement;

        public RemoveFilter(StringValue replacement)
        {
            _replacement = replacement;
        }
        // TODO: use StringResult.Eval
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
