using System;
using System.Collections.Generic;

namespace Liquid.NET.Constants
{
    /// <summary>
    /// Compare objects by Value
    /// // SEE: https://msdn.microsoft.com/en-us/library/vstudio/bb338049%28v=vs.100%29.aspx
    /// </summary>
    public class EasyValueComparer : IEqualityComparer<IExpressionConstant>
    {
        public bool Equals(IExpressionConstant x, IExpressionConstant y)
        {
            //Console.WriteLine("Comparing " + x.Value + " to " + y.Value);
            //Check whether the compared objects reference the same data.
            if (ReferenceEquals(x, y))
            {
                Console.WriteLine("1 EQUAL");
                return true;
            }

            //Check whether any of the compared objects is null.
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                Console.WriteLine("2 UNEQUAL");
                return false;
            }

            //Check whether the products' properties are equal.
            //Console.WriteLine("3 EQ: " + (x.Value == y.Value));
            Console.WriteLine("3 EQ: " + (x.Value.Equals(y.Value)));
            //return x.Value == y.Value;
            return x.Value.Equals(y.Value);
        }

        
        public int GetHashCode(IExpressionConstant obj)
        {
            Console.WriteLine("Hashing " + obj.Value);
            //Check whether the object is null

            if (ReferenceEquals(obj, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hasConstantValue = obj.Value == null ? 0 : obj.Value.GetHashCode();

            //Calculate the hash code for the object.
            Console.WriteLine("HASH FOR " + obj.Value + " is " + hasConstantValue);
            return hasConstantValue;
        }
    }
}
