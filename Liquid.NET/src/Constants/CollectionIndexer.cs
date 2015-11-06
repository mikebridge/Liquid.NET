using System.Collections.Generic;
using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{
    public static class CollectionIndexer
    {
        public static Option<ILiquidValue> ValueAt(IList<Option<ILiquidValue>> array, int key)
        {
            if (key >= array.Count || key < -array.Count)
            {
                return new None<ILiquidValue>();
            }
            key = WrapMod(key, array.Count);

            return array[key];
        }

        private static int WrapMod(int index, int length)
        {
            return (index % length + length) % length;
        }

    }
}
