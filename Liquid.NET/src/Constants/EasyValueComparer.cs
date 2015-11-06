using System.Collections.Generic;

namespace Liquid.NET.Constants
{
    /// <summary>
    /// Compare objects by Value
    /// // SEE: https://msdn.microsoft.com/en-us/library/vstudio/bb338049%28v=vs.100%29.aspx
    /// </summary>
    public class EasyValueComparer : IEqualityComparer<ILiquidValue>
    {
        public bool Equals(ILiquidValue x, ILiquidValue y)
        {
            //Check whether the compared objects reference the same data.
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            //Check whether any of the compared objects is null.
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }
            return x.Value.Equals(y.Value);
        }

        
        public int GetHashCode(ILiquidValue obj)
        {
            //Console.WriteLine("Hashing " + obj.Value);
            //Check whether the object is null

            if (ReferenceEquals(obj, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hasConstantValue = obj.Value == null ? 0 : obj.Value.GetHashCode();

            //Calculate the hash code for the object.
            //Console.WriteLine("HASH FOR " + obj.Value + " is " + hasConstantValue);
            return hasConstantValue;
        }
    }
}
