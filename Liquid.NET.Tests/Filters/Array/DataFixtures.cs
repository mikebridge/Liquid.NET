using System.Collections.Generic;
using Liquid.NET.Constants;

namespace Liquid.NET.Tests.Filters.Array
{
    public static class DataFixtures
    {
        public static DictionaryValue CreateDictionary(int id, string field1, string field2)
        {
            return new DictionaryValue(new Dictionary<string, IExpressionConstant>
            {
                {"id", new NumericValue(id)},
                {"field1", new StringValue(field1)},
                {"field2", new StringValue(field2)},

            });
        }
    }
}
