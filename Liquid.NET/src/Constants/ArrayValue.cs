using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using Liquid.NET.Symbols;

namespace Liquid.NET.Constants
{
    public class ArrayValue : ExpressionConstant, IEnumerable<IExpressionConstant>
    {
        private readonly IList<IExpressionConstant> _values;// = new List<IExpressionConstant>(); 

        public ArrayValue(IList<IExpressionConstant> values)
        {
            _values = values;
        }

        public override object Value
        {
            get { return _values; }
        }

        // TODO: Is this correct?
        public override bool IsTrue
        {
            get { return _values.Count != 0; }
        }

        public IList<IExpressionConstant> ArrValue { get { return _values; } }

        public IExpressionConstant ValueAt(int key)
        {
            if (key >= _values.Count || key < -_values.Count)
            {
                return ConstantFactory.CreateError<StringValue>("index "+key+" is outside the bounds of the array.");
            }
            key = WrapMod(key, _values.Count);
            
            Console.WriteLine("KEY IS "+ key);
            return _values[key];

        }

        public static int WrapMod(int index, int length)
        {
            return (index % length + length) % length;
        }

        IEnumerator<IExpressionConstant> IEnumerable<IExpressionConstant>.GetEnumerator()
        {
            return ArrValue.GetEnumerator();
        }


        public IEnumerator GetEnumerator()
        {
            return ArrValue.GetEnumerator();
        }
    }
}
