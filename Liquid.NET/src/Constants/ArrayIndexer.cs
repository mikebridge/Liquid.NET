using System;
using System.Collections.Generic;
using Liquid.NET.Utils;

namespace Liquid.NET.Constants
{
    public static class ArrayIndexer
    {
        public static Option<IExpressionConstant> ValueAt(IList<Option<IExpressionConstant>> array, int key)
        {
            if (key >= array.Count || key < -array.Count)
            {
                //return ConstantFactory.CreateNilValueOfType<StringValue>("index "+key+" is outside the bounds of the array.");
                //return new NilValue();
                return new None<IExpressionConstant>();
            }
            key = WrapMod(key, array.Count);
            
            //Console.WriteLine("KEY IS "+ key);
            return array[key];
        }

        public static int WrapMod(int index, int length)
        {
            return (index % length + length) % length;
        }

    }
}
