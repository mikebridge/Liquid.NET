using System.Collections.Generic;
using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{
    public class EasyOptionComparer : IEqualityComparer<Option<ILiquidValue>>
    {
        public bool Equals(Option<ILiquidValue> x, Option<ILiquidValue> y)
        {
            if (!x.HasValue && !y.HasValue)
            {
                return true;
            }
            if (x.HasValue != y.HasValue)
            {
                return false;
            }
            return new EasyValueComparer().Equals(x.Value, y.Value);
        }

        public int GetHashCode(Option<ILiquidValue> obj)
        {
            return obj.HasValue ? new EasyValueComparer().GetHashCode(obj.Value) : obj.GetHashCode();
        }
    }
}
