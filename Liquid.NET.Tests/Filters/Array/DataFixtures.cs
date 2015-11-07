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
                {"field1", LiquidString.Create(field1)},
                {"field2", LiquidString.Create(field2)},

            };
        }

        public static LiquidCollection CreateArrayValue()
        {
           return new LiquidCollection{
                LiquidString.Create("a string"), 
                LiquidNumeric.Create(123), 
                LiquidNumeric.Create(456m),
                new LiquidBoolean(false)
            };
        }

    }
}
