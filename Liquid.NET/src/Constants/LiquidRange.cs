using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Liquid.NET.Constants
{
    public class LiquidRange : LiquidValue, IEnumerable<LiquidValue>
    {
        private readonly LiquidNumeric _start;
        private readonly LiquidNumeric _end;

        public LiquidRange(LiquidNumeric start, LiquidNumeric end)
        {
            _start = start;
            _end = end;
        }

        public override object Value
        {
            get { throw new NotImplementedException(); }
        }

        public override bool IsTrue
        {
            get { return true; }
        }

        public override string LiquidTypeName
        {
            get { return "range"; }
        }

        public int Length
        {
            get
            {
                return Math.Abs(_start.IntValue - _end.IntValue) + 1;
            }
        }

        public IEnumerator<LiquidValue> GetEnumerator()
        {
            var start = _start.IntValue;
            var end = _end.IntValue;

            return end < start ?
                CreateEnumerable(end, start).Reverse().GetEnumerator() : 
                CreateEnumerable(start, end).GetEnumerator();

        }

        private IEnumerable<LiquidNumeric> CreateEnumerable(int start, int end)
        {
            var length = end - start + 1;
            return Enumerable.Range(start, length)
                .Select(LiquidNumeric.Create);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
