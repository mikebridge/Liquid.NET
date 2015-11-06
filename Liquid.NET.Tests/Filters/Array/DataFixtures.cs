using System.Collections.Generic;
using Liquid.NET.Constants;

namespace Liquid.NET.Tests.Filters.Array
{
    public static class DataFixtures
    {
        public static DictionaryValue CreateDictionary(int id, string field1, string field2)
        {
            return new DictionaryValue
            {
                {"id", NumericValue.Create(id)},
                {"field1", new StringValue(field1)},
                {"field2", new StringValue(field2)},

            };
        }

        public static ArrayValue CreateArrayValue()
        {
           return new ArrayValue{
                new StringValue("a string"), 
                NumericValue.Create(123), 
                NumericValue.Create(456m),
                new BooleanValue(false)
            };
        }

    }
}
