using System.Collections.Generic;
using Liquid.NET.Constants;

namespace Liquid.NET.Tests.Filters.Array
{
    public static class DataFixtures
    {
        public static LiquidHash CreateDictionary(int id, string field1, string field2)
        {
            return new LiquidHash
            {
                {"id", LiquidNumeric.Create(id)},
                {"field1", new LiquidString(field1)},
                {"field2", new LiquidString(field2)},

            };
        }

        public static LiquidCollection CreateArrayValue()
        {
           return new LiquidCollection{
                new LiquidString("a string"), 
                LiquidNumeric.Create(123), 
                LiquidNumeric.Create(456m),
                new LiquidBoolean(false)
            };
        }

    }
}
